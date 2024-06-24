using System.Reactive.Subjects;
using QuotesApi.Models;

namespace UserRepository;

public class UserRepository(
    LocalStorage.LocalStorage localStorage,
    QuotesApi.QuotesApi api,
    IUserSecureStorage? secureStorage,
    UserLocalStorage? userLocalStorage)
{
    private readonly IUserSecureStorage _secureStorage = secureStorage ?? new UserSecureStorage();

    private readonly UserLocalStorage _userLocalStorage =
        userLocalStorage ?? new UserLocalStorage(localStorage: localStorage);

    private readonly BehaviorSubject<User?> _userSubject = new(null);
    private readonly BehaviorSubject<DarkModePreference> _darkModePreference = new(DarkModePreference.Unspecified);

    public async Task UpsertDarkModePreference(DarkModePreference preference)
    {
        await _userLocalStorage.UpsertDarkModePreference(preference.ToCacheModel());
        _darkModePreference.OnNext(preference);
    }

    public async IAsyncEnumerable<DarkModePreference> GetDarkModePreference()
    {
        var storedPreference = await _userLocalStorage.GetDarkModePreference();
        _darkModePreference.OnNext(
            storedPreference?.ToDomainModel() ?? DarkModePreference.Unspecified
        );

        yield return _darkModePreference.Value;
    }

    public async Task SignIn(string email, string password)
    {
        try
        {
            var apiUser = await api.SignIn(email, password);
            await _secureStorage.UpsertUserInfo(username: apiUser.Username, email: apiUser.Email, token: apiUser.Token);

            var domainUser = apiUser.ToDomainModel();
            _userSubject.OnNext(domainUser);
        }
        catch (Exception ex) when (ex is InvalidCredentialsQuoteException)
        {
            throw new InvalidCredentialsException();
        }
    }

    public async IAsyncEnumerable<User?> GetUser()
    {
        if (_userSubject.Value is null)
        {
            var userInfo = await Task.WhenAll([_secureStorage.GetUserEmail(), _secureStorage.GetUserEmail()]);
            var email = userInfo[0];
            var username = userInfo[1];

            if (email is not null && username is not null)
            {
                _userSubject.OnNext(new User(Username: username, Email: email));
            }
        }
        else
        {
            _userSubject.OnNext(null);
        }

        yield return _userSubject.Value;
    }

    public Task<string?> GetUserToken() => _secureStorage.GetUserToken();

    public async Task SignUp(string username, string email, string password)
    {
        try
        {
            var userToken = await api.SignUp(username, email, password);
            
            await _secureStorage.UpsertUserInfo(username, email, userToken);
            
            _userSubject.OnNext(new User(Username: username, Email: email));
        }
        catch (Exception ex) when (ex is UsernameAlreadyTakenQuoteException)
        {
            throw new UsernameAlreadyTakenException();
        }
        catch (Exception ex) when (ex is EmailAlreadyRegisteredQuoteException)
        {
            throw new EmailAlreadyRegisteredException();
        }
    }

    public async Task UpdateProfile(string username, string email, string? newPassword)
    {
        try
        {
            await api.UpdateProfile(username, email, password: newPassword);
            await _secureStorage.UpsertUserInfo(username, email);
            _userSubject.OnNext(new User(username, email));
        }
        catch (Exception exception) when(exception is UsernameAlreadyTakenQuoteException)
        {
            throw new UsernameAlreadyTakenException();
        }
    }

    public async Task SignOut()
    {
        await api.SignOut();
        await _secureStorage.DeleteUserInfo();
        _userSubject.OnNext(null);
    }

    public async Task RequestPasswordResetEmail(string email) => await api.RequestPasswordResetEmail(email);
}
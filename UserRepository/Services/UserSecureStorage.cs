namespace UserRepository.Services;

public class UserSecureStorage : IUserSecureStorage
{
    private const string TokenKey = "wonder-words-token";
    private const string UsernameKey = "wonder-words-username";
    private const string EmailKey = "wonder-words-email";

    private readonly IPreferences _secureStorage = Preferences.Default;

    public Task UpsertUserInfo(string username, string email, string? token)
    {
        return Task.WhenAll([
            Task.Run(() => _secureStorage.Set(key: EmailKey, value: email)),
            Task.Run(() => _secureStorage.Set(key: UsernameKey, value: username)),
            token is not null ? Task.Run(() => _secureStorage.Set(key: TokenKey, value: token)) : Task.CompletedTask
        ]);
    }

    public Task DeleteUserInfo()
    {
        return Task.WhenAll([
            Task.Run(() => _secureStorage.Remove(key: TokenKey)),
            Task.Run(() => _secureStorage.Remove(key: UsernameKey)),
            Task.Run(() => _secureStorage.Remove(key: EmailKey)),
        ]);
    }

    public Task<string?> GetUserToken()
    {
        return Task.Run(() => _secureStorage.Get<string?>(key: TokenKey, defaultValue: null));
    }

    public Task<string?> GetUserEmail()
    {
        return Task.Run(() => _secureStorage.Get<string?>(key: EmailKey, null));
    }

    public Task<string?> GetUsername()
    {
        return Task.Run(() => _secureStorage.Get<string?>(key: UsernameKey, null));
    }
}
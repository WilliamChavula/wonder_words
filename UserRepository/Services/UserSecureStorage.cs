using Microsoft.Maui.Storage;

namespace UserRepository.Services;

public class UserSecureStorage : IUserSecureStorage
{
    private const string TokenKey = "wonder-words-token";
    private const string UsernameKey = "wonder-words-username";
    private const string EmailKey = "wonder-words-email";

    private readonly ISecureStorage _secureStorage = SecureStorage.Default;

    public Task UpsertUserInfo(string username, string email, string? token)
    {
        return Task.WhenAll([
            _secureStorage.SetAsync(key: EmailKey, value: email),
            _secureStorage.SetAsync(key: UsernameKey, value: username),
            token is not null ? _secureStorage.SetAsync(key: TokenKey, value: token) : Task.CompletedTask
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
        return _secureStorage.GetAsync(key: UsernameKey);
    }

    public Task<string?> GetUserEmail()
    {
        return _secureStorage.GetAsync(key: EmailKey);
    }

    public Task<string?> GetUsername()
    {
        return _secureStorage.GetAsync(key: UsernameKey);
    }
}
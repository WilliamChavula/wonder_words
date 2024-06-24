namespace UserRepository.Services;

public interface IUserSecureStorage
{
    Task UpsertUserInfo(string username, string email, string? token = null);
    Task DeleteUserInfo();
    Task<string?> GetUserToken();
    Task<string?> GetUserEmail();
    Task<string?> GetUsername();
}
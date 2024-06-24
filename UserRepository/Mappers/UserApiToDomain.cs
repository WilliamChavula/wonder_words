namespace UserRepository.Mappers;

public static class UserApiToDomain
{
    public static User ToDomainModel(this UserRm user)
    {
        return new User(Username: user.Username, Email: user.Email);
    }
}
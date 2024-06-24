namespace UserRepository;

public static class RegisterServices
{
    public static MauiAppBuilder RegisterUserRepositoryServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<IUserSecureStorage, Services.UserSecureStorage>();
        
        return mauiAppBuilder;
    }
}
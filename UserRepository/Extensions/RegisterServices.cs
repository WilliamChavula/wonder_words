namespace UserRepository.Extensions;

public static class RegisterServices
{
    public static MauiAppBuilder RegisterUserRepositoryServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<IUserSecureStorage, UserSecureStorage>();
        mauiAppBuilder.Services.AddSingleton<UserRepository>();
        mauiAppBuilder.Services.AddSingleton<UserLocalStorage>();
        
        return mauiAppBuilder;
    }
}
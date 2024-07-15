using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DomainModels;
using ProfileMenu.Interfaces;
using UserRepositoryImpl = UserRepository.UserRepository;
using QuoteRepositoryImpl = QuoteRepository.QuoteRepository;

namespace ProfileMenu.ViewModels;

public partial class ProfileMenuViewModel : ObservableObject
{
    private readonly UserRepositoryImpl _userRepository;
    private readonly QuoteRepositoryImpl _quoteRepository;
    private readonly INavigationService _navigationService;

    [ObservableProperty] private DarkModePreference _darkModePreference;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsUserAuthenticated))]
    private string? _username;

    [ObservableProperty] private bool _isSignOutInProgress;
    [ObservableProperty] private bool _isLoadingData;

    /// <inheritdoc/>
    public ProfileMenuViewModel(
        UserRepositoryImpl userRepository,
        QuoteRepositoryImpl quoteRepository,
        INavigationService navigationService
    )
    {
        _userRepository = userRepository;
        _quoteRepository = quoteRepository;
        _navigationService = navigationService;

        GetLatest();
    }

    public bool IsUserAuthenticated => Username is not null;

    [RelayCommand]
    private async Task DarkModePreferenceChanged(DarkModePreference preference)
    {
        await _userRepository.UpsertDarkModePreference(preference);
    }

    private async void GetLatest()
    {
        IsLoadingData = true;
        await foreach (var user in _userRepository.GetUser())
        {
            Username = user?.Username;
        }

        await foreach (var preference in _userRepository.GetDarkModePreference())
        {
            DarkModePreference = preference;
        }

        IsLoadingData = false;
    }

    [RelayCommand]
    private async Task SignOut()
    {
        IsSignOutInProgress = true;

        await Task.WhenAll([
            _userRepository.SignOut(),
            _quoteRepository.ClearCache()
        ]);
        
        IsSignOutInProgress = false;
    }

    [RelayCommand]
    private async Task OnSignInTapped()
    {
        await _navigationService.GoToAsync(""); // Todo: Navigate to SignIn Page
    }

    [RelayCommand]
    private async Task OnSignUpTapped()
    {
        await _navigationService.GoToAsync(""); // Todo: Navigate to SignUp Page
    }

    [RelayCommand]
    private async Task OnUpdateProfileTapped()
    {
        await _navigationService.GoToAsync(""); // Todo: Navigate to Update Profile Page
    }
}
using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DomainModels;
using UserRepositoryImpl = UserRepository.UserRepository;
using QuoteRepositoryImpl = QuoteRepository.QuoteRepository;

namespace ProfileMenu.ViewModels;

public partial class ProfileMenuViewModel : ObservableObject
{
    private readonly UserRepositoryImpl _userRepository;
    private readonly QuoteRepositoryImpl _quoteRepository;
    private readonly Func<Task> _onSignInTap;
    private readonly Func<Task> _onSignUpTap;
    private readonly Func<string, string, Task> _onUpdateProfileTap;

    [ObservableProperty] private DarkModePreference _darkModePreference;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsUserAuthenticated))]
    private string? _username;

    [ObservableProperty] private string? _email;

    [ObservableProperty] private bool _isSignOutInProgress;
    [ObservableProperty] private bool _isLoadingData;

    /// <inheritdoc/>
    public ProfileMenuViewModel(
        UserRepositoryImpl userRepository,
        QuoteRepositoryImpl quoteRepository,
        Func<Task> onSignInTap,
        Func<Task> onSignUpTap,
        Func<string, string, Task> onUpdateProfileTap
    )
    {
        _userRepository = userRepository;
        _quoteRepository = quoteRepository;
        _onSignInTap = onSignInTap;
        _onSignUpTap = onSignUpTap;
        _onUpdateProfileTap = onUpdateProfileTap;

        GetLatest();
    }

    public bool IsUserAuthenticated => Username is not null;

    [RelayCommand]
    private void DarkModePreferenceChanged(DarkModePreference preference)
    {
        _userRepository.UpsertDarkModePreference(preference);
    }

    private async void GetLatest()
    {
        IsLoadingData = true;
        await foreach (var user in _userRepository.GetUser())
        {
            Username = user?.Username;
            Email = user?.Email;
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
    private async Task OnSignInTapped() => await _onSignInTap();

    [RelayCommand]
    private async Task OnSignUpTapped() => await _onSignUpTap();

    [RelayCommand]
    private async Task OnUpdateProfileTapped() =>
        await _onUpdateProfileTap(Email ?? string.Empty, Username ?? string.Empty);
}
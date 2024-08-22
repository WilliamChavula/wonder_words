using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DomainModels;
using DomainModels.Delegates;
using QuoteRepositoryImpl = QuoteRepository.QuoteRepository;
using UserRepositoryImpl = UserRepository.UserRepository;

namespace ProfileMenu.ViewModels;

public partial class ProfileMenuViewModel : ObservableObject
{
    private readonly UserRepositoryImpl _userRepository;
    private readonly QuoteRepositoryImpl _quoteRepository;
    private readonly SignInTapDelegate _onSignInTap;
    private readonly SignUpTapDelegate _onSignUpTap;
    private readonly UpdateProfileTapDelegate _onUpdateProfileTap;

    [ObservableProperty]
    private DarkModePreference? _darkModePreference;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsUserAuthenticated))]
    private string? _username;

    [ObservableProperty]
    private string? _email;

    [ObservableProperty]
    private bool _isSignOutInProgress;

    [ObservableProperty]
    private bool _isLoadingData;

    /// <inheritdoc/>
    public ProfileMenuViewModel(
        UserRepositoryImpl userRepository,
        QuoteRepositoryImpl quoteRepository,
        SignInTapDelegate onSignInTap,
        SignUpTapDelegate onSignUpTap,
        UpdateProfileTapDelegate onUpdateProfileTap
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
            IsLoadingData = false;
            Email = user?.Email;
        }

        await foreach (var preference in _userRepository.GetDarkModePreference())
        {
            DarkModePreference = preference;
        }
    }

    [RelayCommand]
    private async Task SignOut()
    {
        IsSignOutInProgress = true;

        await Task.WhenAll([_userRepository.SignOut(), _quoteRepository.ClearCache()]);

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

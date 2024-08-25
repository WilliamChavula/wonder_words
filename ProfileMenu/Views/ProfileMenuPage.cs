using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Maui.Core;
using ControlsLibrary;
using ControlsLibrary.Converters;
using ControlsLibrary.Icons;
using ControlsLibrary.Resources.Styles;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using ProfileMenu.ViewModels;
using UraniumUI.Icons.MaterialSymbols;
using L10n = ProfileMenu.Resources.Resources;

namespace ProfileMenu.Views;

public class ProfileMenuPage : ContentPage
{
    public ProfileMenuPage(ProfileMenuViewModel viewModel)
    {
        Resources.MergedDictionaries.Add(new Styles());
        Shell.SetNavBarIsVisible(this, false);

        _ = Resources.TryGetValue("XxxLargeSpacing", out var xl3Large);
        _ = Resources.TryGetValue("MediumLargeSpacing", out var screenMargin);
        _ = Resources.TryGetValue("XLargeSpacing", out var xlLarge);
        _ = Resources.TryGetValue("LargeSpacing", out var large);
        _ = Resources.TryGetValue("SmallSpacing", out var small);

        On<iOS>().SetUseSafeArea(true);
        BindingContext = viewModel;

#pragma warning disable CA1416 // Validate platform compatibility
        Behaviors.Add(
            new StatusBarBehavior
            {
                StatusBarColor = Colors.White,
                StatusBarStyle = StatusBarStyle.DarkContent
            }
        );
#pragma warning restore CA1416 // Validate platform compatibility

        #region Activity Indicator
        var indicator = new CenteredActivityIndicator();
        indicator.SetBinding(
            IsVisibleProperty,
            new Binding { Source = this, Path = "BindingContext.IsLoadingData" }
        );

        var loadingContainer = new Grid
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Children = { indicator }
        };
        #endregion

        #region Not Signed In Controls
        var userNotSignedInViewsContainer = new Grid
        {
            ColumnDefinitions = [new ColumnDefinition(GridLength.Star)],
            RowDefinitions =
            [
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto)
            ]
        };

        var signInBtn = new ExpandedElevatedButton
        {
            Margin = new Thickness { Bottom = (double)screenMargin },
            Padding = new Thickness
            {
                Left = (double)screenMargin,
                Right = (double)screenMargin,
                Top = (double)xl3Large
            },
            Icon = MaterialRoundIcons.Login,
            TextLabel = L10n.signInButtonLabel
        };
        signInBtn.SetBinding(
            ExpandedElevatedButton.OnTapProperty,
            new Binding { Source = this, Path = "BindingContext.SignInTappedCommand" }
        );

        userNotSignedInViewsContainer.Add(signInBtn);

        var openingTextLabel = new Label
        {
            Text = L10n.signUpOpeningText,
            HorizontalTextAlignment = TextAlignment.Center,
            TextColor = Colors.LightSlateGray
        };
        openingTextLabel.SetBinding(
            IsVisibleProperty,
            new Binding
            {
                Source = this,
                Path = "BindingContext.IsUserAuthenticated",
                Converter = new InvertedBoolConverter()
            }
        );

        userNotSignedInViewsContainer.Add(openingTextLabel, row: 1);

        var signUpBtn = new Button
        {
            Style = (Style)Resources["textButton"],
            Text = L10n.signUpButtonLabel,
            FontAttributes = FontAttributes.Bold,
            FontFamily = "OpenSansSemibold",
            HorizontalOptions = LayoutOptions.Center,
            TextColor = Colors.Black
        };
        signUpBtn.SetBinding(
            Button.CommandProperty,
            new Binding { Source = this, Path = "BindingContext.SignUpTappedCommand" }
        );

        userNotSignedInViewsContainer.Add(signUpBtn, row: 2);
        userNotSignedInViewsContainer.Add(
            new BoxView { HeightRequest = (double)large, BackgroundColor = Colors.Transparent },
            row: 3
        );

        userNotSignedInViewsContainer.SetBinding(
            IsVisibleProperty,
            new Binding
            {
                Source = this,
                Path = "BindingContext.IsUserAuthenticated",
                Converter = new InvertedBoolConverter()
            }
        );

        #endregion


        #region Sign In Controls
        var userSignedInViewsContainer = new Grid
        {
            ColumnDefinitions = [new ColumnDefinition(GridLength.Star)],
            RowDefinitions =
            [
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto)
            ]
        };

        userSignedInViewsContainer.SetBinding(
            IsVisibleProperty,
            new Binding { Source = this, Path = "BindingContext.IsUserAuthenticated", }
        );

        var formattedText = new FormattedString
        {
            Spans =
            {
                new Span { Text = L10n.signedInUserGreeting },
                new Span { Text = " " },
            }
        };

        var usernameSpan = new Span();
        usernameSpan.SetBinding(
            Span.TextProperty,
            new Binding { Source = this, Path = "BindingContext.Username", }
        );
        formattedText.Spans.Add(usernameSpan);

        var greetingControl = new Border
        {
            Stroke = Brush.Transparent,
            Padding = new Thickness((double)screenMargin),
            Content = new Label
            {
                FontSize = 18,
                TextColor = (Color)Resources["OnSecondaryDark"],
                FormattedText = formattedText
            }
        };

        var greetingLayout = new VerticalStackLayout
        {
            Children =
            {
                greetingControl,
                new BoxView { Style = (Style)Resources["divider"] }
            }
        };

        userSignedInViewsContainer.Add(greetingLayout, row: 0);

        var updateProfileLabel = new ChevronListTile
        {
            Text = L10n.updateProfileTileLabel,
            Padding = new Thickness((double)screenMargin),
        };
        updateProfileLabel.SetBinding(
            ChevronListTile.CommandProperty,
            new Binding { Source = this, Path = "BindingContext.UpdateProfileTappedCommand" }
        );

        var updateProfileLayout = new VerticalStackLayout
        {
            Margin = new Thickness { Bottom = (double)screenMargin },
            Children =
            {
                updateProfileLabel,
                new BoxView { Style = (Style)Resources["divider"] }
            }
        };

        userSignedInViewsContainer.Add(updateProfileLayout, row: 1);

        var inProgressBtn = new InProgressExpandedElevatedButton
        {
            TextLabel = L10n.signOutButtonLabel
        };
        var expandedBtn = new ExpandedElevatedButton
        {
            Padding = new Thickness
            {
                Left = (double)screenMargin,
                Right = (double)screenMargin,
                Bottom = (double)xlLarge
            },
            TextLabel = L10n.signOutButtonLabel,
            Icon = MaterialRounded.Logout
        };
        expandedBtn.SetBinding(
            ExpandedElevatedButton.OnTapProperty,
            new Binding { Source = this, Path = "BindingContext.SignOutCommand" }
        );

        var button = new Border { Stroke = Brush.Transparent };
        button.SetBinding(
            Border.ContentProperty,
            new Binding
            {
                Source = this,
                Path = "BindingContext.IsSignOutInProgress",
                Converter = new ButtonBooleanTemplateSelector
                {
                    IfTruthyView = inProgressBtn,
                    IfFalseyView = expandedBtn
                }
            }
        );

        userSignedInViewsContainer.Add(button, row: 3);

        #endregion

        #region DarkMode Preference
        var themePicker = new DarkModePreferencePicker();
        themePicker.SetBinding(
            DarkModePreferencePicker.CurrentValueProperty,
            new Binding { Source = this, Path = "BindingContext.DarkModePreference" }
        );
        
        themePicker.SetBinding(
            DarkModePreferencePicker.RadioOptionsProperty,
            new Binding { Source = this, Path = "BindingContext.DarkModePreferences" }
        );
        themePicker.SetBinding(
            DarkModePreferencePicker.PreferenceChangedProperty,
            new Binding { Source = this, Path = "BindingContext.DarkModePreferenceChangedCommand" }
        );

        #endregion

        #region Page Layout
        var pageViewsContainer = new VerticalStackLayout
        {
            Children =
            {
                loadingContainer,
                userNotSignedInViewsContainer,
                userSignedInViewsContainer,
                themePicker
            }
        };

        Content = pageViewsContainer;

        #endregion
    }
}

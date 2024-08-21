using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using ControlsLibrary.Resources.Styles;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using SignIn.Controls;
using SignIn.ViewModels;
using L10n = SignIn.Resources.Resources;
using MauiScrollView = Microsoft.Maui.Controls.ScrollView; // Normal import conflicting with ScrollView from iOS.

namespace SignIn.Views;

public class SignInPage : ContentPage
{
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    public SignInPage(SignInViewModel viewModel)
    {
        On<iOS>().SetUseSafeArea(true);
        Resources.MergedDictionaries.Add(new Styles());
        BindingContext = viewModel;

        Behaviors.Add(
            new StatusBarBehavior
            {
                StatusBarColor = Colors.Black,
                StatusBarStyle = StatusBarStyle.LightContent
            }
        );

        var behavior = new EventToCommandBehavior
        {
            EventName = nameof(Unfocused),
            Command = new Command(Unfocus)
        };
        Behaviors.Add(behavior);
        Title = L10n.appBarTitle;

        Content = new Border
        {
            Stroke = Brush.Transparent,
            Content = new MauiScrollView
            {
                Margin = new Thickness { Top = (double)Resources["MediumLargeSpacing"] },
                Padding = new Thickness((double)Resources["MediumLargeSpacing"], default),
                Content = new SignInForm(viewModel)
            },
        };
    }
}

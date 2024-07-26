using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using ControlsLibrary.Resources.Styles;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using SignIn.Controls;
using SignIn.ViewModels;
using L10n = SignIn.Resources.Resources;

namespace SignIn.Views;

public class SignInView : ContentPage
{
    public SignInView(SignInViewModel viewModel)
    {
        BindingContext = viewModel;
        On<iOS>().SetUseSafeArea(true);
        Resources.MergedDictionaries.Add(new Styles());

        Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Colors.Black,
            StatusBarStyle = StatusBarStyle.LightContent
        });

        var behavior = new EventToCommandBehavior
        {
            EventName = nameof(Unfocused),
            Command = new Command(Unfocus)
        };
        Behaviors.Add(behavior);
        Title = L10n.appBarTitle;

        Content = new Border
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Content = new Microsoft.Maui.Controls.ScrollView // Normal import conflicting with ScrollView from iOS.
            {
                Padding = new Thickness((double)Resources["MediumLargeSpacing"], default),
                Content = new SignInForm(viewModel)
            },
        };
    }
}
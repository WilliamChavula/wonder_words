using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using ControlsLibrary.Resources.Styles;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using SignUp.Controls;
using SignUp.ViewModels;
using L10n = SignUp.Resources.Resources;
using ScrollView = Microsoft.Maui.Controls.ScrollView;

namespace SignUp.Views;

public class SignUpPage : ContentPage
{
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    public SignUpPage(SignUpViewModel viewModel)
    {
        On<iOS>().SetUseSafeArea(true);
        Resources.MergedDictionaries.Add(new Styles());

        BindingContext = viewModel;

        var statusBar = new StatusBarBehavior
        {
            StatusBarColor = Colors.Black,
            StatusBarStyle = StatusBarStyle.LightContent
        };
        var behavior = new EventToCommandBehavior
        {
            EventName = nameof(Unfocused),
            Command = new Command(Unfocus)
        };

        Behaviors.Add(statusBar);
        Behaviors.Add(behavior);
        Title = L10n.appBarTitle;

        Content = new Border
        {
            Stroke = Brush.Transparent,
            Content = new ScrollView
            {
                Padding = new Thickness
                {
                    Left = (double)Resources["MediumLargeSpacing"],
                    Right = (double)Resources["MediumLargeSpacing"],
                    Top = (double)Resources["MediumLargeSpacing"]
                },

                Content = new SignUpForm(viewModel)
            }
        };
    }
}

using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using ScrollView = Microsoft.Maui.Controls.ScrollView;
using SignUp.ViewModels;
using ControlsLibrary.Resources.Styles;
using SignUp.Controls;
using L10n = SignUp.Resources.Resources;

namespace SignUp.Views;

public class SignUpPage : ContentPage
{
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    public SignUpPage(SignUpViewModel viewModel)
    {
        On<iOS>().SetUseSafeArea(true);
        
        BindingContext = viewModel;
        Resources.MergedDictionaries.Add(new Styles());

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

        Content = new ScrollView
        {
            Padding = new Thickness
            {
                Left = (double)Resources["MediumLargeSpacing"],
                Right = (double)Resources["MediumLargeSpacing"],
                Top = (double)Resources["MediumLargeSpacing"]
            },
            
            Content = new SignUpForm(viewModel)
        };
    }
}
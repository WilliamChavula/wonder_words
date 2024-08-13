using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using ControlsLibrary.Resources.Styles;
using ScrollView = Microsoft.Maui.Controls.ScrollView;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using UpdateProfile.Controls;
using UpdateProfile.ViewModels;
using L10n = UpdateProfile.Resources.Resources;

namespace UpdateProfile.Views;

public class UpdateProfilePage : ContentPage
{
    public UpdateProfilePage(UpdateProfileViewModel viewModel)
    {
        BindingContext = viewModel;
        
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
            
            Content = new UpdateProfileForm(viewModel)
        };
    }
}
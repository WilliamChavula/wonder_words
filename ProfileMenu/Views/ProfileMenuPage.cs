using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Microsoft.Maui.Layouts;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Markup;
using UraniumUI.Icons.MaterialSymbols;
using ControlsLibrary;
using ProfileMenu.ViewModels;
using L10n = ProfileMenu.Resources.Resources;

namespace ProfileMenu.Views;

public class ProfileMenuPage : ContentPage
{
    public ProfileMenuPage(ProfileMenuViewModel viewModel)
    {
        Resources.MergedDictionaries.Add(new ResourceDictionary
        {
            Source = new Uri("Resources/Styles/Styles.xaml", UriKind.RelativeOrAbsolute)
        });

        _ = Resources.TryGetValue("XxxLargeSpacing", out var xl3Large);
        _ = Resources.TryGetValue("MediumLargeSpacing", out var screenMargin);
        _ = Resources.TryGetValue("XLargeSpacing", out var xlLarge);
        _ = Resources.TryGetValue("LargeSpacing", out var large);
        _ = Resources.TryGetValue("SmallSpacing", out var small);

        On<iOS>().SetUseSafeArea(true);
        BindingContext = viewModel;

        Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Colors.Black,
            StatusBarStyle = StatusBarStyle.LightContent
        });

        if (viewModel.IsLoadingData)
        {
            Content = new CenteredActivityIndicator();
        }

        Content = new VerticalStackLayout
        {
            Children =
            {
                new VerticalStackLayout
                {
                    Children =
                    {
                        new ExpandedElevatedButton
                        {
                            Padding = new Thickness
                            {
                                Left = (double)screenMargin,
                                Right = (double)screenMargin,
                                Top = (double)xl3Large
                            },
                            Content = new Label
                            {
                                Text = L10n.signInButtonLabel
                            },
                            Icon = MaterialRounded.Login
                        }.Bind(ExpandedElevatedButton.OnTapProperty, nameof(viewModel.SignInTappedCommand)),

                        new BoxView { HeightRequest = (double)xlLarge, },
                        new Label { Text = L10n.signUpOpeningText },

                        new Button
                        {
                            StyleClass = { "TextButton" },
                            Text = L10n.signUpButtonLabel
                        }.Bind(Button.CommandProperty, nameof(viewModel.SignUpTappedCommand)),

                        new BoxView { HeightRequest = (double)large, },
                    }
                }.Bind(
                    IsVisibleProperty, nameof(viewModel.IsUserAuthenticated),
                    converter: new InvertedBoolConverter()),

                new FlexLayout
                {
                    Direction = FlexDirection.Column,
                    Children =
                    {
                        new Border
                        {
                            Padding = new Thickness((double)small),
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center,
                            Content = new Label
                            {
                                FormattedText = new FormattedString
                                {
                                    Spans =
                                    {
                                        new Span { Text = L10n.signedInUserGreeting },
                                        new Span { Text = " " },
                                        new Span().Bind(Span.TextProperty, nameof(viewModel.Username))
                                    }
                                },
                                FontSize = 24
                            }
                        }.Grow(1),

                        new BoxView { StyleClass = { "Divider" } },

                        new ChevronListTile
                        {
                            Text = L10n.updateProfileTileLabel
                        }.Bind(ChevronListTile.CommandProperty, nameof(viewModel.UpdateProfileTappedCommand)),

                        new BoxView { StyleClass = { "Divider" } },
                        new BoxView { HeightRequest = (double)screenMargin, },


                        new InProgressExpandedElevatedButton
                        {
                            TextLabel = L10n.signOutButtonLabel
                        }.Bind(IsVisibleProperty, nameof(viewModel.IsSignOutInProgress)),
                                
                        new ExpandedElevatedButton
                            {
                                Padding = new Thickness
                                {
                                    Left = (double)screenMargin,
                                    Right = (double)screenMargin,
                                    Bottom = (double)xlLarge
                                },
                                Content = new Label
                                {
                                    Text = L10n.signOutButtonLabel
                                },
                                Icon = MaterialRounded.Logout
                            }.Bind(ExpandedElevatedButton.OnTapProperty, nameof(viewModel.SignOutCommand))
                            .Bind(IsVisibleProperty, nameof(viewModel.IsSignOutInProgress),
                                converter: new InvertedBoolConverter()),
                    }
                }.Bind(IsVisibleProperty, nameof(viewModel.Username), converter: new IsNotNullConverter()),

                new DarkModePreferencePicker()
                    .Bind(DarkModePreferencePicker.CurrentValueProperty, nameof(viewModel.DarkModePreference))
            }
        };
    }
}
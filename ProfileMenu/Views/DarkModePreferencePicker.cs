using System.Windows.Input;
using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls.Shapes;

using RadioButton = UraniumUI.Material.Controls.RadioButton;
using RadioButtonGroupView = InputKit.Shared.Controls.RadioButtonGroupView;

using DomainModels;
using L10n = ProfileMenu.Resources.Resources;

namespace ProfileMenu.Views;

public class DarkModePreferencePicker : ContentView
{
    public DarkModePreference CurrentValue
    {
        get => (DarkModePreference)GetValue(CurrentValueProperty);
        set => SetValue(CurrentValueProperty, value);
    }

    public ICommand OnPreferenceChanged
    {
        get => (ICommand)GetValue(OnPreferenceChangedProperty);
        set => SetValue(OnPreferenceChangedProperty, value);
    }

    public static readonly BindableProperty CurrentValueProperty = BindableProperty.Create(
        nameof(CurrentValue),
        typeof(DarkModePreference),
        typeof(DarkModePreferencePicker)
    );

    public static readonly BindableProperty OnPreferenceChangedProperty = BindableProperty.Create(
        nameof(OnPreferenceChanged),
        typeof(ICommand),
        typeof(DarkModePreferencePicker)
    );

    public DarkModePreferencePicker()
    {
        Resources.MergedDictionaries.Add(new ResourceDictionary
        {
            Source = new Uri("Resources/Styles/Styles.xaml", UriKind.RelativeOrAbsolute)
        });

        _ = Resources.TryGetValue("MediumLarge", out var mediumLarge);

        Content = new VerticalStackLayout
        {
            Children =
            {
                new Border
                {
                    HeightRequest = 60,
                    Padding = 16,
                    StyleClass = { "Elevation0" },
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = 4
                    },
                    Content = new Label
                    {
                        Text = L10n.darkModePreferencesHeaderTileLabel,
                        FontAttributes = FontAttributes.Bold,
                        FontSize = (double)mediumLarge,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center
                    }
                },

                new RadioButtonGroupView
                    {
                        Children =
                        {
                            new RadioButton
                            {
                                Text = L10n.darkModePreferencesAlwaysDarkTileLabel,
                                Value = DarkModePreference.Dark
                            },

                            new RadioButton
                            {
                                Text = L10n.darkModePreferencesAlwaysLightTileLabel,
                                Value = DarkModePreference.Light
                            },

                            new RadioButton
                            {
                                Text = L10n.darkModePreferencesUseSystemSettingsTileLabel,
                                Value = DarkModePreference.Unspecified
                            }
                        }
                    }.Bind(RadioButtonGroupView.SelectedItemProperty, nameof(CurrentValue))
                    .Bind(RadioButtonGroupView.SelectedItemChangedCommandProperty,
                        nameof(OnPreferenceChanged))
            }
        };
    }
}
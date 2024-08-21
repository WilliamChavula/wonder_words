using System.Windows.Input;
using CommunityToolkit.Maui.Markup;
using ControlsLibrary.Resources.Styles;
using DomainModels;
using Microsoft.Maui.Controls.Shapes;
using L10n = ProfileMenu.Resources.Resources;
using RadioButton = UraniumUI.Material.Controls.RadioButton;
using RadioButtonGroupView = InputKit.Shared.Controls.RadioButtonGroupView;

namespace ProfileMenu.Views;

public class DarkModePreferencePicker : ContentView
{
    public DarkModePreference? CurrentValue
    {
        get => (DarkModePreference)GetValue(CurrentValueProperty);
        set => SetValue(CurrentValueProperty, value);
    }

    public ICommand? PreferenceChanged
    {
        get => (ICommand)GetValue(PreferenceChangedProperty);
        set => SetValue(PreferenceChangedProperty, value);
    }

    public static readonly BindableProperty CurrentValueProperty = BindableProperty.Create(
        nameof(CurrentValue),
        typeof(DarkModePreference),
        typeof(DarkModePreferencePicker)
    );

    public static readonly BindableProperty PreferenceChangedProperty = BindableProperty.Create(
        nameof(PreferenceChanged),
        typeof(ICommand),
        typeof(DarkModePreferencePicker)
    );

    public DarkModePreferencePicker()
    {
        Resources.MergedDictionaries.Add(new Styles());

        _ = Resources.TryGetValue("MediumLarge", out var mediumLarge);

        var radioItems = new RadioButtonGroupView
        {
            Padding = 16,
            Spacing = 24,
            Children =
            {
                new RadioButton
                {
                    Text = L10n.darkModePreferencesAlwaysDarkTileLabel,
                    Value = DarkModePreference.Dark,
                    Color = Colors.Black
                },
                new RadioButton
                {
                    Text = L10n.darkModePreferencesAlwaysLightTileLabel,
                    Value = DarkModePreference.Light,
                    Color = Colors.Black
                },
                new RadioButton
                {
                    Text = L10n.darkModePreferencesUseSystemSettingsTileLabel,
                    Value = DarkModePreference.Unspecified,
                    Color = Colors.Black
                }
            }
        };

        radioItems.SetBinding(
            RadioButtonGroupView.SelectedItemProperty,
            new Binding { Source = this, Path = nameof(CurrentValue) }
        );

        radioItems.SetBinding(
            RadioButtonGroupView.SelectedItemChangedCommandProperty,
            new Binding { Source = this, Path = nameof(PreferenceChanged) }
        );

        Content = new VerticalStackLayout
        {
            Children =
            {
                new Border
                {
                    Stroke = Brush.Transparent,
                    HeightRequest = 60,
                    Padding = 16,
                    Style = (Style)Resources["Elevation0"],
                    StrokeShape = new RoundRectangle { CornerRadius = 4 },
                    Content = new Label
                    {
                        Text = L10n.darkModePreferencesHeaderTileLabel,
                        FontAttributes = FontAttributes.Bold,
                        FontFamily = "OpenSansSemibold",
                        FontSize = (double)mediumLarge,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center
                    }
                },
                radioItems
            }
        };
    }
}

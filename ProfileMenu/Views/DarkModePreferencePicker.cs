using System.Windows.Input;
using CommunityToolkit.Maui.Behaviors;
using ControlsLibrary.Resources.Styles;
using DomainModels;
using Microsoft.Maui.Controls.Shapes;
using ProfileMenu.Converters;
using L10n = ProfileMenu.Resources.Resources;

namespace ProfileMenu.Views;

public class DarkModePreferencePicker : ContentView
{
    public DarkModePreference CurrentValue
    {
        get => (DarkModePreference)GetValue(CurrentValueProperty);
        set => SetValue(CurrentValueProperty, value);
    }

    public ICollection<string> RadioOptions
    {
        get => (ICollection<string>)GetValue(RadioOptionsProperty);
        set => SetValue(RadioOptionsProperty, value);
    }

    public ICommand PreferenceChanged
    {
        get => (ICommand)GetValue(PreferenceChangedProperty);
        set => SetValue(PreferenceChangedProperty, value);
    }

    public static readonly BindableProperty CurrentValueProperty = BindableProperty.Create(
        nameof(CurrentValue),
        typeof(DarkModePreference),
        typeof(DarkModePreferencePicker)
    );

    public static readonly BindableProperty RadioOptionsProperty = BindableProperty.Create(
        nameof(RadioOptions),
        typeof(ICollection<string>),
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

        var options = new CollectionView
        {
            ItemTemplate = new DataTemplate(() =>
            {
                var radioBtn = new RadioButton { GroupName = "ThemeSelectionOptions" };
                var evtToCmd = new EventToCommandBehavior { EventName = "CheckedChanged" };
                evtToCmd.SetBinding(EventToCommandBehavior.CommandProperty, new Binding
                {
                    Source = this,
                    Path = nameof(PreferenceChanged),
                    Mode = BindingMode.OneWay
                });

                evtToCmd.SetBinding(EventToCommandBehavior.CommandParameterProperty, ".");

                radioBtn.Behaviors.Add(evtToCmd);

                radioBtn.SetBinding(RadioButton.ContentProperty, ".");
                radioBtn.SetBinding(
                    RadioButton.IsCheckedProperty,
                    new Binding
                    {
                        Path = ".", // Bind to the current string item itself
                        Converter = new DarkModeIsCheckedConverter(),
                        ConverterParameter = CurrentValue,
                        Mode = BindingMode.OneWay
                    }
                );
                return radioBtn;
            })
        };
        options.SetBinding(ItemsView.ItemsSourceProperty, new Binding
        {
            Source = this,
            Path = nameof(RadioOptions)
        });

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
                options
            }
        };
    }
}
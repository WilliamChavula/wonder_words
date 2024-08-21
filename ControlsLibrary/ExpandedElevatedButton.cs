using System.Windows.Input;
using ControlsLibrary.Converters;
using ControlsLibrary.Icons;
using ControlsLibrary.Resources.Styles;
using Microsoft.Maui.Controls.Shapes;
using UraniumUI.Material.Controls;

namespace ControlsLibrary;

public class ExpandedElevatedButton : ContentView
{
    public static readonly BindableProperty TextLabelProperty = BindableProperty.Create(
        nameof(TextLabel),
        typeof(string),
        typeof(ExpandedElevatedButton)
    );
    public static readonly BindableProperty OnTapProperty = BindableProperty.Create(
        nameof(OnTap),
        typeof(ICommand),
        typeof(ExpandedElevatedButton)
    );
    public static readonly BindableProperty IconProperty = BindableProperty.Create(
        nameof(Icon),
        typeof(string),
        typeof(ExpandedElevatedButton)
    );

    public ExpandedElevatedButton()
    {
        Resources.MergedDictionaries.Add(new Styles());

        // SignIn Button Type 1
        var btnLabel = new Label
        {
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            TextColor = Colors.White,
            FontSize = 18,
        };
        btnLabel.SetBinding(
            Label.TextProperty,
            new Binding { Source = this, Path = nameof(TextLabel) }
        );

        var fontImageSource = new FontImageSource
        {
            FontFamily = "MaterialIconsRegular",
            Size = 20,
            Color = Colors.White
        };
        fontImageSource.SetBinding(
            FontImageSource.GlyphProperty,
            new Binding { Source = this, Path = nameof(Icon) }
        );

        var singInIcon = new Image
        {
            VerticalOptions = LayoutOptions.Center,
            Source = fontImageSource,
            HeightRequest = 24,
            WidthRequest = 24,
        };

        var btnView = new ButtonView
        {
            HeightRequest = 48,
            HorizontalOptions = LayoutOptions.Center,
            BackgroundColor = Colors.Black,
            Content = new HorizontalStackLayout
            {
                Spacing = 12,
                Children = { singInIcon, btnLabel }
            }
        };
        btnView.SetBinding(ButtonView.TappedCommandProperty, new Binding(nameof(OnTap)));

        // SignIn Button Type 2
        var btn = new Button
        {
            HeightRequest = 48,

            BackgroundColor = Colors.Black,
        };
        btn.SetBinding(
            Button.TextProperty,
            new Binding { Source = this, Path = nameof(TextLabel) }
        );
        btn.SetBinding(Button.CommandProperty, new Binding { Source = this, Path = nameof(OnTap) });

        var buttonContainer = new Border
        {
            Style = (Style)Resources["Elevation1"],
            Stroke = Brush.Transparent,
            StrokeShape = new RoundRectangle { CornerRadius = 4 },
            BackgroundColor = Colors.Black
        };
        buttonContainer.SetBinding(
            Border.ContentProperty,
            new Binding
            {
                Source = this,
                Path = nameof(Icon),
                Converter = new ButtonTempleteConverter
                {
                    DefaultButtonTemplate = btnView,
                    AlternateButtonTemplate = btn
                }
            }
        );
        Content = buttonContainer;
    }

    public string TextLabel
    {
        get => (string)GetValue(TextLabelProperty);
        set => SetValue(TextLabelProperty, value);
    }

    public string? Icon
    {
        get => (string)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public ICommand? OnTap
    {
        get => (ICommand)GetValue(OnTapProperty);
        set => SetValue(OnTapProperty, value);
    }
}

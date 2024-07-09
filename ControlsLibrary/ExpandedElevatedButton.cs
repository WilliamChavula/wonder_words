using System.Windows.Input;
using UraniumUI.Material.Controls;

namespace ControlsLibrary;
public partial class ExpandedElevatedButton : ContentView
{
    public static readonly BindableProperty TextLabelProperty = BindableProperty.Create(nameof(TextLabel), typeof(string), typeof(ExpandedElevatedButton));
    public static readonly BindableProperty OnTapProperty = BindableProperty.Create(nameof(OnTap), typeof(ICommand), typeof(ExpandedElevatedButton));
    public static readonly BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(string), typeof(ExpandedElevatedButton));
    
    public ExpandedElevatedButton()
    {
        Content = Icon is not null ? BuildIconButton() : BuildButton();
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
    private static ButtonView BuildIconButton()
    {
        var lbl = new Label();
        lbl.SetBinding(Label.TextProperty, nameof(TextLabel));

        var fontImageSource = new FontImageSource();
        fontImageSource.SetBinding(FontImageSource.GlyphProperty, nameof(Icon));
        
        var btnView = new ButtonView
        {
            StyleClass = { "ElevatedButton" },
            Content = new HorizontalStackLayout
            {
                Spacing = 16,
                Children =
                {
                    lbl,
                    new Image
                    {
                        Source = fontImageSource
                    }
                }
            }
        };
        btnView.SetBinding(ButtonView.TappedCommandProperty, new Binding(nameof(OnTap)));

        return btnView;
    }

    private static Button BuildButton()
    {
        var btn = new Button
        {
            StyleClass = { "ElevatedButton" }
        };
        btn.SetBinding(Button.TextProperty, nameof(TextLabel));
        btn.SetBinding(Button.CommandProperty, new Binding(nameof(OnTap)));

        return btn;
    }
}
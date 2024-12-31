using ControlsLibrary.Resources.Styles;
using UraniumUI.Material.Controls;

namespace ControlsLibrary;

public class InProgressExpandedElevatedButton : ButtonView
{
    public static readonly BindableProperty TextLabelProperty = BindableProperty.Create(
        nameof(TextLabel),
        typeof(string),
        typeof(InProgressExpandedElevatedButton)
    );

    public InProgressExpandedElevatedButton()
    {
        Resources.MergedDictionaries.Add(new Styles());
        var lbl = new Label();
        lbl.SetBinding(Label.TextProperty, new Binding { Source = this, Path = nameof(TextLabel) });

        Style = (Style)Resources["Elevation1"];

        Content = new Border
        {
            Stroke = Brush.Transparent,
            Content = new HorizontalStackLayout
            {
                Padding = 16,
                Children =
                {
                    lbl,
                    new ActivityIndicator { Scale = 0.5, IsRunning = true }
                }
            }
        };
    }

    public string TextLabel
    {
        get => (string)GetValue(TextLabelProperty);
        set => SetValue(TextLabelProperty, value);
    }
}

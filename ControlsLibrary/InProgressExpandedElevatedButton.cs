using UraniumUI.Material.Controls;

namespace ControlsLibrary;

public class InProgressExpandedElevatedButton : ButtonView
{
    public static readonly BindableProperty TextLabelProperty = BindableProperty.Create(nameof(TextLabel), typeof(string), typeof(InProgressExpandedElevatedButton));
    
    public InProgressExpandedElevatedButton()
    {
        var lbl = new Label();
        lbl.SetBinding(Label.TextProperty, nameof(TextLabel));
        
        Content = new ButtonView
        {
            StyleClass = { "ElevatedButton" },
            Content = new HorizontalStackLayout
            {
                Padding = 16,
                Children =
                {
                    lbl,
                    new ActivityIndicator
                    {
                        Scale = 0.5,
                        IsRunning = true
                    }
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
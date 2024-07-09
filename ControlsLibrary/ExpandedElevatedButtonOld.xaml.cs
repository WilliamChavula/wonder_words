using System.Windows.Input;

namespace ControlsLibrary;
public partial class ExpandedElevatedButtonOld : ContentView
{
    public static readonly BindableProperty TextLabelProperty = BindableProperty.Create(nameof(TextLabel), typeof(string), typeof(ExpandedElevatedButtonOld));
    public static readonly BindableProperty OnTapProperty = BindableProperty.Create(nameof(OnTap), typeof(ICommand), typeof(ExpandedElevatedButtonOld));
    public static readonly BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(string), typeof(ExpandedElevatedButtonOld));
    public static readonly BindableProperty InProgressProperty = BindableProperty.Create(nameof(InProgress), typeof(bool), typeof(ExpandedElevatedButtonOld), defaultValue: false);
    
    public ExpandedElevatedButtonOld()
    {
        InitializeComponent();
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

    public bool InProgress
    {
        get => (bool)GetValue(InProgressProperty); 
        set => SetValue(InProgressProperty, value);
    }
}
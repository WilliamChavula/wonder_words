using System.Windows.Input;
// ReSharper disable MemberCanBePrivate.Global

namespace ControlsLibrary;

public partial class InProgressTextButton : ContentView
{
    
    public static readonly BindableProperty ButtonTextProperty = BindableProperty.Create(nameof(ButtonText), typeof(string), typeof(InProgressTextButton), defaultValue: string.Empty);
    public static readonly BindableProperty OnTapCommandProperty = BindableProperty.Create(nameof(OnTapCommand), typeof(ICommand), typeof(InProgressTextButton), defaultValue: null);
    public static readonly BindableProperty TextFontSizeProperty = BindableProperty.Create(nameof(TextFontSize), typeof(double), typeof(InProgressTextButton), defaultValue: 14);
    public static readonly BindableProperty ButtonTextColorProperty = BindableProperty.Create(nameof(ButtonTextColor), typeof(Color), typeof(InProgressTextButton), defaultValue: Colors.Black);
    
    public InProgressTextButton()
    {
        InitializeComponent();
    }

    public string ButtonText
    {
        get => (string)GetValue(ButtonTextProperty);
        set => SetValue(ButtonTextProperty, value);
    }
    
    public ICommand? OnTapCommand
    {
        get => (ICommand) GetValue(OnTapCommandProperty);
        init => SetValue(OnTapCommandProperty, value);
    }

    public double TextFontSize
    {
        get => (double)GetValue(TextFontSizeProperty);
        set => SetValue(TextFontSizeProperty, value);
    }

    public Color ButtonTextColor
    {
        get => (Color)GetValue(ButtonTextColorProperty);
        set => SetValue(ButtonTextColorProperty, value);
    }
}
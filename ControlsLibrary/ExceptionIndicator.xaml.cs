using System.Reflection;
using System.Resources;
using System.Windows.Input;
using UraniumUI.Views;

namespace ControlsLibrary;
public partial class ExceptionIndicator : StatefulContentView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(ExceptionIndicator), propertyChanged: OnTitlePropertyChanged);
    public static readonly BindableProperty MessageProperty = BindableProperty.Create(nameof(Message), typeof(string), typeof(ExceptionIndicator), propertyChanged: OnMessagePropertyChanged);
    public static readonly BindableProperty OnTryAgainProperty = BindableProperty.Create(nameof(OnTryAgain), typeof(ICommand), typeof(ExceptionIndicator));
    
    public ExceptionIndicator()
    {
        InitializeComponent();

        var rm = new ResourceManager(@"ControlsLibrary.Resources.Resources", Assembly.GetExecutingAssembly());
        TitleLabel.Text = (Title ?? rm.GetString("exceptionIndicatorGenericTitle"))!;
        MessageLabel.Text = (Title ?? rm.GetString("exceptionIndicatorGenericMessage"))!;
    }

    public string? Title
    {
        get => (string)GetValue(TitleProperty); 
        set => SetValue(TitleProperty, value);
    }
    public string? Message { 
        get => (string)GetValue(MessageProperty); 
        set => SetValue(MessageProperty, value); 
    }

    public ICommand? OnTryAgain
    {
        get => (ICommand)GetValue(OnTryAgainProperty); 
        set => SetValue(OnTryAgainProperty, value);
    }
    
    private static void OnTitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (Label)bindable;
        control.Text = (string)newValue;
    }
    private static void OnMessagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (Label)bindable;
        control.Text = (string)newValue;
    }
}
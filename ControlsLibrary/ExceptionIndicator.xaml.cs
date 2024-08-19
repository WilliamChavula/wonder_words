using System.Windows.Input;
using ControlsLibrary.Resources.Styles;
using UraniumUI.Views;
// using L10n = ControlsLibrary.Resources.Resources;

namespace ControlsLibrary;

public partial class ExceptionIndicator : StatefulContentView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string),
        typeof(ExceptionIndicator));

    public static readonly BindableProperty MessageProperty = BindableProperty.Create(nameof(Message), typeof(string),
        typeof(ExceptionIndicator));

    public static readonly BindableProperty TryAgainProperty =
        BindableProperty.Create(nameof(TryAgain), typeof(ICommand), typeof(ExceptionIndicator));

    public ExceptionIndicator()
    {
        Resources.MergedDictionaries.Add(new Styles());

        InitializeComponent();

        // var rm = new ResourceManager(@"ControlsLibrary.Resources.Resources", Assembly.GetExecutingAssembly());
        TitleLabel.Text = "Oops!"; //L10n.exceptionIndicatorGenericTitle;
        MessageLabel.Text = "Oops!"; //L10n.exceptionIndicatorGenericMessage;
    }

    public string? Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string? Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public ICommand? TryAgain
    {
        get => (ICommand)GetValue(TryAgainProperty);
        set => SetValue(TryAgainProperty, value);
    }
}
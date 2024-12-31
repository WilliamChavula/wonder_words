using System.Windows.Input;
using ControlsLibrary.Resources.Styles;
using UraniumUI.Views;

namespace ControlsLibrary;

public partial class ChevronListTile : StatefulContentView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(ChevronListTile),
        string.Empty
    );
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command),
        typeof(ICommand),
        typeof(ChevronListTile)
    );

    public ChevronListTile()
    {
        Resources.MergedDictionaries.Add(new Styles());
        InitializeComponent();

        // SetBinding(PressedCommandProperty, new Binding { Source = this, Path = nameof(Command) });
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
}

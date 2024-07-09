using System.Windows.Input;
using UraniumUI.Views;

namespace ControlsLibrary;

public partial class ChevronListTile : StatefulContentView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(ChevronListTile), string.Empty);
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ChevronListTile));
    
    public ChevronListTile()
    {
        InitializeComponent();
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
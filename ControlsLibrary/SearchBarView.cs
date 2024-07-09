using System.Windows.Input;
using UraniumUI.Icons.MaterialSymbols;
using UraniumUI.Material.Controls;

namespace ControlsLibrary;

public class SearchBarView : ContentView
{
    public ICommand OnValueChanged
    {
        get => (ICommand)GetValue(OnValueChangedProperty);
        set => SetValue(OnValueChangedProperty, value);
    }

    private string SearchText
    {
        get => (string)GetValue(SearchTextProperty);
        set => SetValue(SearchTextProperty, value);
    }

    public static readonly BindableProperty OnValueChangedProperty = BindableProperty.Create(
        nameof(OnValueChanged),
        typeof(ICommand),
        typeof(SearchBarView)
    );

    public static readonly BindableProperty SearchTextProperty = BindableProperty.Create(
        nameof(SearchText),
        typeof(string),
        typeof(SearchBarView),
        defaultValue: string.Empty);

    public TextEditingController Controller { get; set; } = new TextEditingController();

    public SearchBarView()
    {
        var textField = new TextField
        {
            Title = ControlsLibrary.Resources.Resources.searchBarLabelText,
            Attachments =
            {
                new Image
                {
                    Source = new FontImageSource
                    {
                        Glyph = MaterialRounded.Search
                    }
                }
            },
        };
        textField.TextChanged += TextFieldOnTextChanged;

        Content = textField;
        Controller.Clear = textField.ClearValue;
    }

    private void TextFieldOnTextChanged(object? sender, TextChangedEventArgs e)
    {
        Controller.Text = e.NewTextValue;
    }

    public class TextEditingController
    {
        public string Text { get; set; } = string.Empty;
        public Action Clear { get; set; } = () => { };
    }
}
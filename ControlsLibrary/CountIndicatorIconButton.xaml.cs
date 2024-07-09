using System.Windows.Input;

namespace ControlsLibrary;

public partial class CountIndicatorIconButton : ContentView
{
    public static readonly BindableProperty CountProperty =
        BindableProperty.Create(nameof(Count), typeof(int), typeof(CountIndicatorIconButton));

    public static readonly BindableProperty IconProperty =
        BindableProperty.Create(nameof(Icon), typeof(string), typeof(CountIndicatorIconButton));

    public static readonly BindableProperty TooltipProperty =
        BindableProperty.Create(nameof(Tooltip), typeof(string), typeof(CountIndicatorIconButton));

    public static readonly BindableProperty OnTapProperty =
        BindableProperty.Create(nameof(OnTap), typeof(ICommand), typeof(CountIndicatorIconButton));

    public readonly BindableProperty IconColorProperty = BindableProperty.Create(nameof(IconColor), typeof(Color),
        typeof(CountIndicatorIconButton), propertyChanged:
        (bindable, _, newValue) =>
        {
            if (bindable is CountIndicatorIconButton btn)
            {
                btn.IconColor = (Color)newValue;
            }
        });

    public CountIndicatorIconButton()
    {
        InitializeComponent();
    }

    public int Count
    {
        get => (int)GetValue(CountProperty);
        set => SetValue(CountProperty, value);
    }

    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public string? Tooltip
    {
        get => (string)GetValue(TooltipProperty);
        set => SetValue(TooltipProperty, value);
    }

    public ICommand OnTap
    {
        get => (ICommand)GetValue(OnTapProperty);
        set => SetValue(OnTapProperty, value);
    }

    public Color? IconColor
    {
        get => (Color)GetValue(IconColorProperty);
        set => SetValue(IconColorProperty, value);
    }
}
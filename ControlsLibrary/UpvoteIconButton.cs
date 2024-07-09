using System.Windows.Input;
using UraniumUI.Icons.MaterialIcons;

namespace ControlsLibrary;

public class UpvoteIconButton : ContentView
{
    public int Count
    {
        get => (int)GetValue(CountProperty);
        set => SetValue(CountProperty, value);
    }

    public bool IsUpVoted
    {
        get => (bool)GetValue(IsUpVotedProperty);
        set => SetValue(IsUpVotedProperty, value);
    }

    public ICommand OnTap
    {
        get => (ICommand)GetValue(OnTapProperty);
        set => SetValue(OnTapProperty, value);
    }

    public static readonly BindableProperty CountProperty = BindableProperty.Create(
        nameof(Count),
        typeof(int),
        typeof(UpvoteIconButton)
    );

    public static readonly BindableProperty IsUpVotedProperty = BindableProperty.Create(
        nameof(IsUpVoted),
        typeof(bool),
        typeof(UpvoteIconButton),
        propertyChanged: (bindable, _, newValue) =>
        {
            if (bindable is UpvoteIconButton upvoteIconButton)
            {
                upvoteIconButton.IsUpVoted = (bool)newValue;
            }
        }
    );

    public static readonly BindableProperty OnTapProperty = BindableProperty.Create(
        nameof(OnTap),
        typeof(ICommand),
        typeof(UpvoteIconButton)
    );

    public UpvoteIconButton()
    {
        var btn = new CountIndicatorIconButton
        {
            OnTap = OnTap,
            Count = Count,
            Icon = MaterialSharp.Arrow_upward,
            Tooltip = ControlsLibrary.Resources.Resources.upvoteIconButtonTooltip
        };

        if (IsUpVoted)
        {
            btn.SetAppThemeColor(btn.IconColorProperty, Colors.Black, Colors.White);
        }
        else
        {
            btn.SetAppThemeColor(btn.IconColorProperty, Color.FromArgb("#8A000000"), Color.FromArgb("#8AFFFFFF"));
        }

        Content = btn;
    }
}
using System.Windows.Input;

namespace ControlsLibrary;

public partial class DownVoteIconButton : ContentView
{
    public static readonly BindableProperty CountProperty = BindableProperty.Create(
        nameof(Count),
        typeof(int),
        typeof(DownVoteIconButton)
    );
    public static readonly BindableProperty IsDownVotedProperty = BindableProperty.Create(
        nameof(IsDownVoted),
        typeof(bool),
        typeof(DownVoteIconButton)
    );
    public static readonly BindableProperty OnTapProperty = BindableProperty.Create(
        nameof(OnTap),
        typeof(ICommand),
        typeof(DownVoteIconButton)
    );

    public DownVoteIconButton()
    {
        InitializeComponent();

        _ = Resources.TryGetValue("VotedButtonColorDark", out var votedButtonColorDark);
        _ = Resources.TryGetValue("UnVotedButtonColorDark", out var unVotedButtonColorDark);

        // IconButtonView.IconColor = IsDownVoted
        //     ? (Color)votedButtonColorDark
        //     : (Color)unVotedButtonColorDark;
    }

    public int Count
    {
        get => (int)GetValue(CountProperty);
        set => SetValue(CountProperty, value);
    }
    public bool IsDownVoted
    {
        get => (bool)GetValue(IsDownVotedProperty);
        set => SetValue(IsDownVotedProperty, value);
    }
    public ICommand OnTap
    {
        get => (ICommand)GetValue(OnTapProperty);
        set => SetValue(OnTapProperty, value);
    }
}

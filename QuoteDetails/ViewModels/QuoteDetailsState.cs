using CommunityToolkit.Mvvm.ComponentModel;
using DomainModels;

namespace QuoteDetails.ViewModels;

public partial class QuoteDetailsViewModel
{
    public event EventHandler? OnStateChanged;

    [ObservableProperty]
    private bool _inProgress;

    [ObservableProperty]
    private Quote? _quote;

    [ObservableProperty]
    private Exception? _quoteUpdateError;

    [ObservableProperty]
    private bool _quoteDetailFailed;

    public void StateChangedEvent(EventArgs eventArgs)
    {
        OnStateChanged?.Invoke(this, eventArgs);
    }
}

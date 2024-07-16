using CommunityToolkit.Mvvm.ComponentModel;
using DomainModels;

namespace QuoteDetails.ViewModels;

public partial class QuoteDetailsState : ObservableObject
{
    public event EventHandler? OnStateChanged;
    
    [ObservableProperty] private bool _inProgress;
    [ObservableProperty] private Quote? _quote;
    [ObservableProperty] private dynamic? _quoteUpdateError;
    [ObservableProperty] private bool _quoteDetailFailed;

    public void StateChangedEvent(EventArgs eventArgs)
    {
        OnStateChanged?.Invoke(this, eventArgs);
    }
}
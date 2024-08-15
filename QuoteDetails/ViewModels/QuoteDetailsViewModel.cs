using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DomainModels.Delegates;
using DomainModels;
using Repository = QuoteRepository.QuoteRepository;

namespace QuoteDetails.ViewModels;

[QueryProperty(nameof(QuoteId), "QuoteId")]
public partial class QuoteDetailsViewModel : ObservableObject
{
    [ObservableProperty] private int _quoteId;
    private readonly Repository quoteRepository;
    private readonly AuthenticationErrorDelegate onAuthenticationError;

    public QuoteDetailsViewModel(Repository quoteRepository, AuthenticationErrorDelegate onAuthenticationError)
    {
        this.quoteRepository = quoteRepository;
        this.onAuthenticationError = onAuthenticationError;

        QuoteDetailsState = new QuoteDetailsState();
    }

    public QuoteDetailsState? QuoteDetailsState { get; private set; }

    private async Task FetchQuoteDetails()
    {
        try
        {
            var quote = await quoteRepository.GetQuoteDetails(QuoteId);
            QuoteDetailsState = new QuoteDetailsState { Quote = quote };
            QuoteDetailsState.StateChangedEvent(EventArgs.Empty);
        }
        catch
        {
            QuoteDetailsState = new QuoteDetailsState { QuoteDetailFailed = true };
            QuoteDetailsState.StateChangedEvent(EventArgs.Empty);
        }
    }

    private async Task ExecuteQuoteUpdateOperation(Func<Task<Quote>> updateQuote)
    {
        try
        {
            var quote = await updateQuote();
            QuoteDetailsState = new QuoteDetailsState { Quote = quote };
            QuoteDetailsState.StateChangedEvent(EventArgs.Empty);
        }
        catch (Exception e)
        {
            var lastState = QuoteDetailsState;

            if (lastState is { Quote: not null })
            {
                QuoteDetailsState = new QuoteDetailsState { Quote = lastState.Quote, QuoteUpdateError = e };
                QuoteDetailsState.StateChangedEvent(EventArgs.Empty);
            }
        }
    }

    [RelayCommand]
    private async Task ReFetch()
    {
        QuoteDetailsState = new QuoteDetailsState { InProgress = true };

        await FetchQuoteDetails();

        QuoteDetailsState = new QuoteDetailsState { InProgress = false };
    }

    [RelayCommand]
    private async Task UpVoteQuote()
    {
        await ExecuteQuoteUpdateOperation(() => quoteRepository.UpVoteQuote(QuoteId));
    }

    [RelayCommand]
    private async Task DownVoteQuote()
    {
        await ExecuteQuoteUpdateOperation(() => quoteRepository.DownVoteQuote(QuoteId));
    }

    [RelayCommand]
    private async Task UnVoteQuote()
    {
        await ExecuteQuoteUpdateOperation(() => quoteRepository.UnVoteQuote(QuoteId));
    }

    [RelayCommand]
    private async Task UnFavoriteQuote()
    {
        await ExecuteQuoteUpdateOperation(() => quoteRepository.UnFavoriteQuote(QuoteId));
    }

    [RelayCommand]
    private async Task OnAuthenticationError() => await onAuthenticationError();

    // Todo: Add Logic to pass back Quote onNavigateBack
}
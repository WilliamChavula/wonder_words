using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DomainModels;
using DomainModels.Delegates;
using Repository = QuoteRepository.QuoteRepository;

namespace QuoteDetails.ViewModels;

public partial class QuoteDetailsViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    private int _quoteId;
    private readonly Repository _quoteRepository;
    private readonly AuthenticationErrorDelegate _onAuthenticationError;

    public QuoteDetailsViewModel(
        Repository quoteRepository,
        AuthenticationErrorDelegate onAuthenticationError
    )
    {
        _quoteRepository = quoteRepository;
        _onAuthenticationError = onAuthenticationError;
    }

    private async Task FetchQuoteDetails()
    {
        try
        {
            var quote = await _quoteRepository.GetQuoteDetails(QuoteId);
            Quote = quote;
            StateChangedEvent(EventArgs.Empty);
        }
        catch
        {
            QuoteDetailFailed = true;
            StateChangedEvent(EventArgs.Empty);
        }
    }

    private async Task ExecuteQuoteUpdateOperation(Func<Task<Quote>> updateQuote)
    {
        try
        {
            var quote = await updateQuote();
            Quote = quote;
            StateChangedEvent(EventArgs.Empty);
        }
        catch (Exception e)
        {
            // var lastState = QuoteDetailsState;
            QuoteUpdateError = e;
            StateChangedEvent(EventArgs.Empty);

            // if (lastState is { Quote: not null })
            // {
            //     QuoteDetailsState = new QuoteDetailsState
            //     {
            //         Quote = lastState.Quote,
            //         QuoteUpdateError = e
            //     };
            //     QuoteDetailsState.StateChangedEvent(EventArgs.Empty);
            // }
        }
    }

    [RelayCommand]
    private async Task ReFetch()
    {
        InProgress = true;

        await FetchQuoteDetails();

        InProgress = false;
    }

    [RelayCommand]
    private async Task UpVoteQuote()
    {
        await ExecuteQuoteUpdateOperation(() => _quoteRepository.UpVoteQuote(QuoteId));
    }

    [RelayCommand]
    private async Task DownVoteQuote()
    {
        await ExecuteQuoteUpdateOperation(() => _quoteRepository.DownVoteQuote(QuoteId));
    }

    [RelayCommand]
    private async Task UnVoteQuote()
    {
        await ExecuteQuoteUpdateOperation(() => _quoteRepository.UnVoteQuote(QuoteId));
    }

    [RelayCommand]
    private async Task UnFavoriteQuote()
    {
        await ExecuteQuoteUpdateOperation(() => _quoteRepository.UnFavoriteQuote(QuoteId));
    }

    [RelayCommand]
    private async Task OnAuthenticationError() => await _onAuthenticationError();

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        _ = int.TryParse((string)query["quoteId"], out var param);
        QuoteId = param;

        await FetchQuoteDetails();
    }

    // Todo: Add Logic to pass back Quote onNavigateBack
}

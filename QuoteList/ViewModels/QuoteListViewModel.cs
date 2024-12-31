using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Linq;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DomainModels;
using DomainModels.Delegates;
using QuoteRepository;
using QuoteRepo = QuoteRepository.QuoteRepository;
using UserRepo = UserRepository.UserRepository;

namespace QuoteList.ViewModels;

public partial class QuoteListViewModel : ObservableObject
{
    [ObservableProperty] private IList<Quote> _itemList = new ObservableCollection<Quote>();
    [ObservableProperty] private int? _nextPage;
    [ObservableProperty] private Exception? _error;
    [ObservableProperty] private QuoteListFilter _filter;
    [ObservableProperty] private Exception? _refreshError;
    [ObservableProperty] private Exception? _favoriteToggleError;
    [ObservableProperty] private string? _searchTerm;
    [ObservableProperty] private Tag? _tag;
    [ObservableProperty] private bool _isRefreshing;

    private string? _authenticatedUsername;
    private readonly QuoteRepo _quoteRepository;
    private readonly UserRepo _userRepository;
    private readonly QuoteSelectedDelegate _onQuoteSelected;
    private readonly AuthenticationErrorDelegate _onAuthenticationError;

    public event EventHandler? EncounteredRefreshError;
    public event EventHandler? EncounteredFavoriteToggleError;

    /// <inheritdoc/>
    public QuoteListViewModel(
        QuoteRepo quoteRepository,
        UserRepo userRepository,
        QuoteSelectedDelegate onQuoteSelectedFunc,
        AuthenticationErrorDelegate onAuthenticationError
    )
    {
        _quoteRepository = quoteRepository;
        _userRepository = userRepository;
        _onQuoteSelected = onQuoteSelectedFunc;
        _onAuthenticationError = onAuthenticationError;

        OnQuoteListUsernameObtained();

        Observable
            .FromEventPattern<PropertyChangedEventArgs>(this, nameof(PropertyChanged))
            .Where(e => e.EventArgs.PropertyName == nameof(SearchTerm))
            .Select(_ => SearchTerm)
            .Throttle(TimeSpan.FromSeconds(1))
            .DistinctUntilChanged()
            .Subscribe(QuoteListSearchTermChangedCommand.Execute);
    }

    private async Task FetchData(
        int page,
        QuoteListPageFetchPolicy fetchPolicy,
        bool isRefreshing = false
    )
    {
        var currentlyAppliedFilter = Filter;
        var isFilteringByFavorites = currentlyAppliedFilter is QuoteListFilter.QuoteListFilterByFavorites;
        var isUserSignedIn = _authenticatedUsername != null;

        if (isFilteringByFavorites && !isUserSignedIn)
        {
            ItemList = [];
            NextPage = 1;
            Error = null;
            Filter = currentlyAppliedFilter;
        }
        else
        {
            var pagesStream = _quoteRepository.GetQuoteListPage(
                page,
                tag: currentlyAppliedFilter is QuoteListFilter.QuoteListFilterByTag ? Tag : null,
                searchTerm: currentlyAppliedFilter is QuoteListFilter.QuoteListFilterBySearchTerm
                    ? SearchTerm!
                    : string.Empty,
                fetchPolicy: fetchPolicy,
                favoredByUsername: currentlyAppliedFilter is QuoteListFilter.QuoteListFilterByFavorites
                    ? _authenticatedUsername
                    : null
            );

            try
            {
                await foreach (var (isLastPage, newItemList, _) in pagesStream)
                {
                    var oldItemList = ItemList;
                    var completeItemList = isRefreshing || page == 1
                        ? newItemList
                        : oldItemList.Concat(newItemList);

                    int? nextPage = isLastPage ? null : page + 1;

                    NextPage = nextPage;
                    ItemList = completeItemList.ToObservableCollection();
                    Filter = currentlyAppliedFilter;
                    IsRefreshing = isRefreshing;
                }
            }
            catch (Exception e) when (e is EmptySearchResultException)
            {
                ItemList = [];
                Filter = currentlyAppliedFilter;
            }
            catch (Exception e)
            {
                if (isRefreshing)
                {
                    RefreshError = e;
                    OnEncounteredRefreshError();
                }
                else
                {
                    Error = e;
                }
            }
        }
    }

    private async void OnQuoteListUsernameObtained()
    {
        await foreach (var user in _userRepository.GetUser())
        {
            _authenticatedUsername = user?.Username;
        }

        await FetchData(1, QuoteListPageFetchPolicy.CacheAndNetwork);
    }

    [RelayCommand]
    private async Task OnAuthenticationError() => await _onAuthenticationError();

    [RelayCommand]
    private async Task OnQuoteSelected(Quote quote)
    {
        var id = quote.Id.ToString();
        await _onQuoteSelected(quote.Id.ToString());
    }

    [RelayCommand]
    private async Task QuoteListItemFavoriteToggled(Quote item)
    {
        try
        {
            var quoteListItemFavorited = (bool)item.IsFavorite!;

            var updatedQuote = await (quoteListItemFavorited
                ? _quoteRepository.UnFavoriteQuote(item.Id)
                : _quoteRepository.FavoriteQuote(item.Id));

            if (Filter is not QuoteListFilter.QuoteListFilterByFavorites)
            {
                // var quoteIndex = ItemList.IndexOf(item);
                // ItemList.Remove(item);
                // ItemList.Insert(quoteIndex, updatedQuote);
                ItemList = ItemList
                    .Select(quote => quote.Id == updatedQuote.Id ? updatedQuote : quote)
                    .ToObservableCollection();
            }
            else
            {
                ItemList.Remove(item);
                await FetchData(1, QuoteListPageFetchPolicy.NetworkOnly);
            }
        }
        catch (Exception e)
        {
            FavoriteToggleError = e;
            OnEncounteredFavoriteToggleError();
        }
    }

    [RelayCommand]
    private async Task QuoteListFailedFetchRetried()
    {
        Error = null;
        await FetchData(1, QuoteListPageFetchPolicy.CacheAndNetwork);
    }

    [RelayCommand]
    private async Task QuoteListTagChanged(Tag? tag)
    {
        Tag = tag;
        await FetchData(1, QuoteListPageFetchPolicy.CachePreferably);
    }

    [RelayCommand]
    private async Task QuoteListSearchTermChanged(string searchTerm)
    {
        // SearchTerm = searchTerm;
        await FetchData(1, QuoteListPageFetchPolicy.CachePreferably);
    }

    [RelayCommand]
    private async Task QuoteListRefreshed()
    {
        IsRefreshing = true;
        await FetchData(1, QuoteListPageFetchPolicy.NetworkOnly, true);
        IsRefreshing = false;
    }

    [RelayCommand]
    private async Task QuoteListNextPageRequested(int pageNumber)
    {
        Error = null;
        await FetchData(pageNumber, QuoteListPageFetchPolicy.NetworkPreferably);
    }

    [RelayCommand]
    private async Task QuoteListFilterByFavoritesToggled()
    {
        var isFilteringByFavorites = Filter is QuoteListFilter.QuoteListFilterByFavorites;

        if (isFilteringByFavorites)
            Filter = QuoteListFilter.QuoteListFilterByFavorites;

        await FetchData(1,
            isFilteringByFavorites
                ? QuoteListPageFetchPolicy.CacheAndNetwork
                : QuoteListPageFetchPolicy.CachePreferably);
    }


    ///<summary>
    /// <see cref="QuoteListItemUpdated" />: Used when the user taps a quote and modifies it on
    /// that quote’s details screen — favoriting it, unfavoriting, upvoting, etc. You
    /// need this event so you can reflect that change on the home screen as well
    /// </summary>
    [RelayCommand]
    private void QuoteListItemUpdated(Quote updatedQuote)
    {
        ItemList = ItemList
            .Select(quote => quote.Id == updatedQuote.Id ? updatedQuote : quote)
            .ToObservableCollection();
    }

    private void OnEncounteredRefreshError()
    {
        EncounteredRefreshError?.Invoke(this, EventArgs.Empty);
    }

    private void OnEncounteredFavoriteToggleError()
    {
        EncounteredFavoriteToggleError?.Invoke(this, EventArgs.Empty);
    }
}

public enum QuoteListFilter
{
    QuoteListFilterByTag,
    QuoteListFilterBySearchTerm,
    QuoteListFilterByFavorites
}
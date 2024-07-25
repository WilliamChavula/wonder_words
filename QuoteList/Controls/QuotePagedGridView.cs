using System.Windows.Input;
using ControlsLibrary;
using QuoteList.ViewModels;
using UraniumUI.Views;

namespace QuoteList.Controls;

public class QuotePagedGridView : ContentView
{
    public static readonly BindableProperty OnQuoteSelectedProperty = BindableProperty.Create(
        nameof(OnQuoteSelected),
        typeof(ICommand),
        typeof(QuotePagedGridView)
    );

    public ICommand OnQuoteSelected
    {
        get => (ICommand)GetValue(OnQuoteSelectedProperty);
        set => SetValue(OnQuoteSelectedProperty, value);
    }

    public QuotePagedGridView()
    {
        Padding = new Thickness((double)Resources["MediumLargeSpacing"], default);

        var quoteCard = new QuoteCard
        {
            Top = new Image
            {
                WidthRequest = 46,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                Source = ImageSource.FromFile("opening-quote.svg")
            },
            Bottom = new Image
            {
                WidthRequest = 46,
                Source = "closing-quote.svg"
            },
        };

        var dataTemplate = new DataTemplate(() => quoteCard);
        var emptyView = new ExceptionIndicator(); // Todo: Swap with a DataTemplateSelector
        emptyView.SetBinding(
            ExceptionIndicator.OnTryAgainProperty,
            "BindingContext.QuoteListFailedFetchRetriedCommand"
        );

        var collectionView = new CollectionView
        {
            ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical)
            {
                HorizontalItemSpacing = (double)Resources["MediumLargeSpacing"],
                VerticalItemSpacing = (double)Resources["MediumLargeSpacing"]
            },
            ItemTemplate = dataTemplate,
            EmptyView = emptyView
        };
        collectionView.SetBinding(ItemsView.ItemsSourceProperty, new Binding
        {
            Source = new RelativeBindingSource(
                RelativeBindingSourceMode.FindAncestorBindingContext,
                typeof(QuoteListViewModel)
            ),
            Path = "ItemList"
        });
        collectionView.SetBinding(SelectableItemsView.SelectionChangedCommandProperty, nameof(OnQuoteSelected));

        quoteCard.SetBinding(QuoteCard.StatementProperty, "Body");
        quoteCard.SetBinding(QuoteCard.AuthorProperty, "Author");
        quoteCard.SetBinding(QuoteCard.IsFavoriteProperty, "IsFavorite");
        quoteCard.SetBinding(
            QuoteCard.OnFavoriteProperty,
            new Binding("BindingContext.QuoteListItemFavoriteToggledCommand", source: collectionView)
        );
        quoteCard.SetBinding(StatefulContentView.CommandParameterProperty, ".");

        Content = collectionView;
    }
}
using System.Windows.Input;
using ControlsLibrary;
using ControlsLibrary.Resources.Styles;
using QuoteList.ViewModels;
using UraniumUI.Views;

namespace QuoteList.Controls;

public class QuotePagedGridView : ContentView
{
    public static readonly BindableProperty QuoteSelectedCommandProperty = BindableProperty.Create(
        nameof(QuoteSelectedCommand),
        typeof(ICommand),
        typeof(QuotePagedGridView)
    );

    public ICommand QuoteSelectedCommand
    {
        get => (ICommand)GetValue(QuoteSelectedCommandProperty);
        set => SetValue(QuoteSelectedCommandProperty, value);
    }

    public QuotePagedGridView()
    {
        Resources.MergedDictionaries.Add(new Styles());
        Padding = new Thickness((double)Resources["MediumLargeSpacing"], default);

        var quoteCard = new QuoteCard
        {
            Top = new Image
            {
                WidthRequest = 46,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                Source = ImageSource.FromFile("opening_quote.svg")
            },
            Bottom = new Image
            {
                WidthRequest = 46,
                Source = "closing_quote.svg"
            },
        };
        var dataTemplate = new DataTemplate(() => quoteCard);
        // var emptyView = new ExceptionIndicator(); // Todo: Swap with a DataTemplateSelector
        // emptyView.SetBinding(
        //     ExceptionIndicator.OnTryAgainProperty,
        //     "BindingContext.QuoteListFailedFetchRetriedCommand"
        // );

        var collectionView = new CollectionView
        {
            ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical)
            {
                HorizontalItemSpacing = (double)Resources["MediumLargeSpacing"],
                VerticalItemSpacing = (double)Resources["MediumLargeSpacing"]
            },
            ItemTemplate = dataTemplate,
            EmptyView = new Label
            {
                Text = "No Results"
            }
        };
        collectionView.SetBinding(ItemsView.ItemsSourceProperty, new Binding
        {
            Source = new RelativeBindingSource(
                RelativeBindingSourceMode.FindAncestorBindingContext,
                typeof(QuoteListViewModel)
            ),
            Path = "ItemList"
        });
        collectionView.SetBinding(SelectableItemsView.SelectionChangedCommandProperty, nameof(QuoteSelectedCommand));

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
using System.Windows.Input;
using CommunityToolkit.Maui.Converters;
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

        // var emptyView = new ExceptionIndicator(); // Todo: Swap with a DataTemplateSelector
        // emptyView.SetBinding(
        //     ExceptionIndicator.TryAgainProperty,
        //     "BindingContext.QuoteListFailedFetchRetriedCommand"
        // );

        var collectionView = new CollectionView
        {
            RemainingItemsThreshold = 4,
            ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical)
            {
                HorizontalItemSpacing = (double)Resources["MediumLargeSpacing"],
                VerticalItemSpacing = (double)Resources["MediumLargeSpacing"]
            },
            ItemTemplate = new DataTemplate(() =>
            {
                var quoteCard = new QuoteCard
                {
                    Top = new Image
                    {
                        WidthRequest = 24,
                        HeightRequest = 24,
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Center,
                        Source = ImageSource.FromFile("opening_quote.png")
                    },
                    Bottom = new Image
                    {
                        WidthRequest = 24,
                        HeightRequest = 24,
                        HorizontalOptions = LayoutOptions.End,
                        VerticalOptions = LayoutOptions.Center,
                        Source = "closing_quote.png"
                    },
                };

                quoteCard.SetBinding(QuoteCard.StatementProperty, "Body");
                quoteCard.SetBinding(QuoteCard.AuthorProperty, "Author");
                quoteCard.SetBinding(
                    QuoteCard.IsFavoriteProperty,
                    "IsFavorite",
                    converter: new IsNotNullConverter()
                );
                quoteCard.SetBinding(
                    QuoteCard.FavoriteProperty,
                    new Binding
                    {
                        Path = "QuoteListItemFavoriteToggledCommand",
                        Source = new RelativeBindingSource(
                            RelativeBindingSourceMode.FindAncestorBindingContext,
                            typeof(QuoteListViewModel)
                        )
                    }
                );
                quoteCard.SetBinding(QuoteCard.FavoriteCommandParameterProperty, ".");

                quoteCard.SetBinding(
                    QuoteCard.TapProperty,
                    new Binding
                    {
                        Source = new RelativeBindingSource(
                            RelativeBindingSourceMode.FindAncestorBindingContext,
                            typeof(QuoteListViewModel)
                        ),
                        Path = "QuoteSelectedCommand",
                    }
                );

                quoteCard.SetBinding(QuoteCard.SelectedQuoteCommandParameterProperty, ".");

                return quoteCard;
            }),
            EmptyView = new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "No Results",
                TextColor = Colors.LightGray
            }
        };

        collectionView.SetBinding(
            ItemsView.ItemsSourceProperty,
            new Binding
            {
                Source = new RelativeBindingSource(
                    RelativeBindingSourceMode.FindAncestorBindingContext,
                    typeof(QuoteListViewModel)
                ),
                Path = "ItemList"
            }
        );

        collectionView.SetBinding(
            ItemsView.RemainingItemsThresholdReachedCommandProperty,
            new Binding
            {
                Source = new RelativeBindingSource(
                    RelativeBindingSourceMode.FindAncestorBindingContext,
                    typeof(QuoteListViewModel)
                ),
                Path = "QuoteListNextPageRequestedCommand",
            }
        );
        collectionView.SetBinding(
            ItemsView.RemainingItemsThresholdReachedCommandParameterProperty,
            new Binding
            {
                Source = new RelativeBindingSource(
                    RelativeBindingSourceMode.FindAncestorBindingContext,
                    typeof(QuoteListViewModel)
                ),
                Path = "NextPage",
            }
        );

        Content = collectionView;
    }

    /*
     *  new Label
                  {
                      HorizontalTextAlignment = TextAlignment.Center,
                      Text = "No Results",
                      TextColor = Colors.LightGray
                  }
     */
}

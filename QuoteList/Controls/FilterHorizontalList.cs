using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Markup;
using ControlsLibrary;
using ControlsLibrary.Resources.Styles;
using DomainModels;
using QuoteList.Converters;
using QuoteList.Extensions;
using QuoteList.ViewModels;

namespace QuoteList.Controls;

public class FilterHorizontalList : ContentView
{
    public FilterHorizontalList()
    {
        Resources.MergedDictionaries.Add(new Styles());
        var favoritesChip = new FavoritesChip();
        favoritesChip.SetBinding(FavoritesChip.IsFilteringByFavoritesProperty, new Binding
        {
            Source = new RelativeBindingSource(
                RelativeBindingSourceMode.FindAncestorBindingContext,
                typeof(QuoteListViewModel)
            ),
            Path = "Filter",
            Converter = new FilterByFavoritesToBoolConverter()
        });

        var tags = Enum.GetValues(typeof(Tag))
            .Cast<Tag>()
            .Select(tag => new { TagName = tag })
            .ToObservableCollection();

        var collectionView = new CollectionView
        {
            Margin = new Thickness(16, 12),
            HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
            ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Horizontal)
            {
                ItemSpacing = 20
            },
            HeightRequest = 40
        };

        collectionView.SetBinding(ItemsView.ItemsSourceProperty, new Binding
        {
            Source = tags,
        });

        collectionView.ItemTemplate = new DataTemplate(() =>
        {
            var tagName = new TagChip();
            tagName.SetBinding(
                TagChip.TagNameProperty,
                "TagName",
                converter: new TagToLocalizedString()
            );

            return tagName;
        });

        Content = new ScrollView
        {
            Orientation = ScrollOrientation.Horizontal,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
            Content = new HorizontalStackLayout
            {
                Children =
                {
                    new Border
                    {
                        Margin = new Thickness { Left = 16, Right = 0, Top = 12, Bottom = 12 },
                        Stroke = Colors.Transparent,
                        Content = favoritesChip
                    },

                    collectionView
                }
            }
        };
    }
}
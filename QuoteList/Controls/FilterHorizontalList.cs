using CommunityToolkit.Maui.Markup;
using DomainModels;
using QuoteList.Converters;
using QuoteList.ViewModels;

namespace QuoteList.Controls;

public class FilterHorizontalList : ContentView
{
    public FilterHorizontalList()
    {
        var favoritesChip = new FavoritesChip();
        favoritesChip.SetBinding(FavoritesChip.IsFilteringByFavoritesProperty, new Binding
        {
            Source = new RelativeBindingSource(RelativeBindingSourceMode.FindAncestorBindingContext,
                typeof(QuoteListViewModel)),
            Path = "Filter",
            Converter = new FilterByFavoritesToBoolConverter()
        });

        var chips = Enum.GetValues(typeof(Tag))
            .Cast<Tag>()
            .Select(tag => new TagChip { TagName = tag }
                .Bind(TagChip.SelectedTagProperty, static (QuoteListViewModel viewModel) => viewModel.Tag));

        var layout = new HorizontalStackLayout();
        layout.Children.Add(favoritesChip);

        foreach (var tagChip in chips)
        {
            layout.Children.Add(tagChip);
        }

        Content = new ScrollView
        {
            Orientation = ScrollOrientation.Horizontal,
            Padding = new Thickness
            {
                Top = (double)Resources["MediumLargeSpacing"],
                Bottom = (double)Resources["MediumLargeSpacing"],
            },
            Content = layout
        };
    }
}
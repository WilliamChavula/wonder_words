using CommunityToolkit.Maui.Views;
using ControlsLibrary;
using ControlsLibrary.Icons;
using QuoteList.ViewModels;
using L10n = QuoteList.Resources.Resources;

namespace QuoteList.Controls;

public class FavoritesChip : ContentView
{
    public static readonly BindableProperty IsFilteringByFavoritesProperty = BindableProperty.Create(
        nameof(IsFilteringByFavorites),
        typeof(bool),
        typeof(FavoritesChip),
        propertyChanged: IsFilteringPropertyChanged
    );

    private static void IsFilteringPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var cls = (FavoritesChip)bindable;
        cls._isFiltering = (bool)newValue;

    }

    public bool IsFilteringByFavorites
    {
        get => (bool)GetValue(IsFilteringByFavoritesProperty);
        set => SetValue(IsFilteringByFavoritesProperty, value);
    }

    private bool _isFiltering;

    public FavoritesChip()
    {
        var choiceChip = new RoundedChoiceChip
        {
            LabelText = L10n.favoritesTagLabel,
            Avatar = new AvatarView
            {
                HeightRequest = 24,
                WidthRequest = 24,
                ImageSource = new FontImageSource
                {
                    Glyph = _isFiltering ? MaterialOutlineIcons.Favorite : MaterialOutlineIcons.FavoriteBorder,
                    Color = _isFiltering ? Colors.White : Colors.Black,
                    FontFamily = "MaterialIconsRegular",
                    Size = 14
                }
            }
        };
        choiceChip.SetBinding(RoundedChoiceChip.IsSelectedProperty, new Binding
        {
            Source = this,
            Path = nameof(IsFilteringByFavorites)
        });
        choiceChip.SetBinding(RoundedChoiceChip.SelectCommandProperty,
            new Binding
            {
                Source = new RelativeBindingSource(
                    RelativeBindingSourceMode.FindAncestorBindingContext,
                    typeof(QuoteListViewModel)
                ),
                Path = "QuoteListFilterByFavoritesToggledCommand"
            });

        Content = choiceChip;
    }
}
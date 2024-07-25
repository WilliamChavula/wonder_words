using CommunityToolkit.Maui.Views;
using UraniumUI.Icons.MaterialSymbols;
using ControlsLibrary;
using QuoteList.ViewModels;
using L10n = QuoteList.Resources.Resources;

namespace QuoteList.Controls;

public class FavoritesChip : ContentView
{
    public static readonly BindableProperty IsFilteringByFavoritesProperty =
        BindableProperty.Create(nameof(IsFilteringByFavorites), typeof(bool), typeof(FavoritesChip));

    public bool IsFilteringByFavorites
    {
        get => (bool)GetValue(IsFilteringByFavoritesProperty);
        set => SetValue(IsFilteringByFavoritesProperty, value);
    }

    public FavoritesChip()
    {
        var choiceChip = new RoundedChoiceChip
        {
            LabelText = L10n.favoritesTagLabel,
            Avatar = new AvatarView
            {
                ImageSource = new FontImageSource
                {
                    Glyph = IsFilteringByFavorites ? MaterialRounded.Favorite : MaterialOutlined.Favorite,
                    Color = IsFilteringByFavorites ? Colors.White : Colors.Black
                },
            },
            IsSelected = IsFilteringByFavorites,
        };
        choiceChip.SetBinding(RoundedChoiceChip.OnSelectedProperty,
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
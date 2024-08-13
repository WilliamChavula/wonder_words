using System.Windows.Input;
using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Maui.Markup;
using ControlsLibrary.Resources.Styles;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;
using UraniumUI.Icons.MaterialSymbols;
using UraniumUI.Material.Controls;
using UraniumUI.Views;

namespace ControlsLibrary;

public class QuoteCard : StatefulContentView
{
    public static readonly BindableProperty StatementProperty =
        BindableProperty.Create(nameof(Statement), typeof(string), typeof(QuoteCard));

    public static readonly BindableProperty AuthorProperty =
        BindableProperty.Create(nameof(Author), typeof(string), typeof(QuoteCard));

    public static readonly BindableProperty IsFavoriteProperty =
        BindableProperty.Create(nameof(IsFavorite), typeof(bool), typeof(QuoteCard));

    public static readonly BindableProperty TopProperty =
        BindableProperty.Create(nameof(Top), typeof(string), typeof(QuoteCard));

    public static readonly BindableProperty BottomProperty =
        BindableProperty.Create(nameof(Bottom), typeof(string), typeof(QuoteCard));

    public static readonly BindableProperty OnTapProperty =
        BindableProperty.Create(nameof(OnTap), typeof(ICommand), typeof(QuoteCard));

    public static readonly BindableProperty OnFavoriteProperty =
        BindableProperty.Create(nameof(OnFavorite), typeof(ICommand), typeof(QuoteCard));


    public string Statement
    {
        get => (string)GetValue(StatementProperty);
        set => SetValue(StatementProperty, value);
    }

    public string? Author
    {
        get => (string)GetValue(AuthorProperty);
        set => SetValue(AuthorProperty, value);
    }

    public bool IsFavorite
    {
        get => (bool)GetValue(IsFavoriteProperty);
        set => SetValue(IsFavoriteProperty, value);
    }

    public View? Top
    {
        get => (View)GetValue(TopProperty);
        set => SetValue(TopProperty, value);
    }

    public View? Bottom
    {
        get => (View)GetValue(BottomProperty);
        set => SetValue(BottomProperty, value);
    }

    public ICommand? OnTap
    {
        get => (ICommand)GetValue(OnTapProperty);
        set => SetValue(OnTapProperty, value);
    }

    public ICommand? OnFavorite
    {
        get => (ICommand)GetValue(OnFavoriteProperty);
        set => SetValue(OnFavoriteProperty, value);
    }

    public QuoteCard()
    {
        
        Resources.MergedDictionaries.Add(new Styles());

        _ = Resources.TryGetValue("MediumSpacing", out var mediumSp);
        _ = Resources.TryGetValue("XLargeSpacing", out var xLarge);
        _ = Resources.TryGetValue("Large", out var fontLarge);

        // StyleClass.Add("Elevation1");
        Margin = new Thickness(0);

        Content = new Border
        {
            StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(12)
            },
            Content = new VerticalStackLayout
            {
                HorizontalOptions = LayoutOptions.End,
                Children =
                {
                    new FlexLayout
                    {
                        Direction = FlexDirection.Row,
                        Children =
                        {
                            new Border
                            {
                                Padding = new Thickness { Left = (double)mediumSp },
                                Content = Top
                            }.Bind(IsVisibleProperty, nameof(Top), converter: new IsNotNullConverter()),

                            new BoxView().Grow(1f),
                            new ButtonView
                            {
                                Content = new Image
                                {
                                    Source = new FontImageSource
                                    {
                                        Glyph = IsFavorite ? MaterialRounded.Favorite : MaterialOutlined.Favorite
                                    }
                                }
                            }.Bind(ButtonView.PressedCommandProperty, nameof(OnFavorite))
                        }
                    },

                    new Label
                    {
                        Text = Statement,
                        Padding = new Thickness
                        {
                            Left = (double)xLarge,
                            Right = (double)xLarge
                        },
                        FontSize = (double)fontLarge
                    },

                    new Border
                    {
                        Padding = new Thickness { Right = (double)mediumSp },
                        Content = Bottom
                    }.Bind(IsVisibleProperty, nameof(Bottom), converter: new IsNotNullConverter()),

                    new Label
                    {
                        Text = Author!,
                        Padding = new Thickness
                        {
                            Bottom = (double)mediumSp,
                            Right = (double)mediumSp
                        }
                    }.Bind(IsVisibleProperty, nameof(Author), converter: new IsNotNullConverter())
                }
            }
        }.Bind(StatefulContentView.TappedCommandProperty, nameof(OnTap));
    }
}
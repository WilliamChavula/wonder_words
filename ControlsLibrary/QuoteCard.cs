using System.Windows.Input;
using CommunityToolkit.Maui.Converters;
using ControlsLibrary.Resources.Styles;
using Microsoft.Maui.Controls.Shapes;
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
        BindableProperty.Create(nameof(Top), typeof(View), typeof(QuoteCard));

    public static readonly BindableProperty BottomProperty =
        BindableProperty.Create(nameof(Bottom), typeof(View), typeof(QuoteCard));

    public static readonly BindableProperty TapProperty =
        BindableProperty.Create(nameof(Tap), typeof(ICommand), typeof(QuoteCard));

    public static readonly BindableProperty FavoriteProperty =
        BindableProperty.Create(nameof(Favorite), typeof(ICommand), typeof(QuoteCard));


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

    public ICommand? Tap
    {
        get => (ICommand)GetValue(TapProperty);
        set => SetValue(TapProperty, value);
    }

    public ICommand? Favorite
    {
        get => (ICommand)GetValue(FavoriteProperty);
        set => SetValue(FavoriteProperty, value);
    }

    public QuoteCard()
    {
        Resources.MergedDictionaries.Add(new Styles());

        _ = Resources.TryGetValue("MediumSpacing", out var mediumSp);
        _ = Resources.TryGetValue("XLargeSpacing", out var xLarge);
        // _ = Resources.TryGetValue("Large", out var fontLarge);

        Margin = new Thickness(0);
        Padding = new Thickness(8);
        

        var gridView = new Grid
        {
            // Style = (Style)Resources["Elevation3"],
            Padding = new Thickness(12),
            ColumnDefinitions =
            [
                new ColumnDefinition(GridLength.Auto),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Auto)
            ]
        };

        var topBorder = new Border
        {
            Stroke = Brush.Transparent,
            Padding = new Thickness { Left = (double)mediumSp }
        };
        topBorder.SetBinding(Border.ContentProperty, new Binding
        {
            Source = this,
            Path = nameof(Top)
        });
        topBorder.SetBinding(IsVisibleProperty, new Binding
        {
            Source = this,
            Path = nameof(Top),
            Converter = new IsNotNullConverter()
        });

        gridView.Add(topBorder, 0);
        gridView.Add(new BoxView
        {
            BackgroundColor = Colors.Transparent
        }, 1);

        var btn = new ButtonView
        {
            Content = new Image
            {
                Source = new FontImageSource
                {
                    Glyph = IsFavorite ? MaterialRounded.Favorite : MaterialOutlined.Favorite,
                    FontFamily = "MaterialIconsRegular",
                    Size = 12
                }
            }
        };
        btn.SetBinding(ButtonView.PressedCommandProperty, new Binding
        {
            Source = this,
            Path = nameof(Favorite)
        });

        gridView.Add(btn, 2);

        var quoteBody = new Label
        {
            Padding = new Thickness
            {
                Left = (double)xLarge,
                Right = (double)xLarge
            },
            FontSize = 16, // (double)fontLarge,
            // MaxLines = 5
        };
        quoteBody.SetBinding(Label.TextProperty, new Binding
        {
            Source = this,
            Path = nameof(Statement)
        });

        var bottomBorder = new Border
        {
            Padding = new Thickness { Right = (double)mediumSp },
            Stroke = Brush.Transparent
        };
        bottomBorder.SetBinding(Border.ContentProperty, new Binding
        {
            Source = this,
            Path = nameof(Bottom)
        });
        
        bottomBorder.SetBinding(IsVisibleProperty, new Binding
        {
            Source = this,
            Path = nameof(Bottom),
            Converter = new IsNotNullConverter()
        });

        var authorLabel = new Label
        {
            Padding = new Thickness
            {
                Bottom = (double)mediumSp,
                Right = (double)mediumSp
            },
            FontSize = 14,
            HorizontalTextAlignment = TextAlignment.End
        };
        
        authorLabel.SetBinding(Label.TextProperty, new Binding
        {
            Source = this,
            Path = nameof(Author)
        });
        authorLabel.SetBinding(IsVisibleProperty, new Binding
        {
            Source = this,
            Path = nameof(Author),
            Converter = new IsNotNullConverter()
        });
        
        var container = new Border
        {
            Stroke = Brush.Transparent,
            StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(12)
            },
            Content = new VerticalStackLayout
            {
                HorizontalOptions = LayoutOptions.End,
                Children =
                {
                    gridView,

                    quoteBody,

                    bottomBorder,

                    authorLabel
                }
            }
        };
        
        container.SetBinding(TappedCommandProperty, new Binding
        {
            Source = this,
            Path = nameof(Tap)
        });

        Content = new Border
        {
            Content = container,
            Shadow = new Shadow
            {
                Brush = Brush.DarkGray,
                Offset = new Point(4, 4),
                Opacity = 0.4f
            },
            StrokeShape = new RoundRectangle
            {
                CornerRadius = 12
            }
        };
    }
}
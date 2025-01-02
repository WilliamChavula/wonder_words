using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using ControlsLibrary;
using ControlsLibrary.Icons;
using ControlsLibrary.Resources.Styles;
using DomainModels;
using DomainModels.Delegates;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using QuoteDetails.Converters;
using QuoteDetails.ViewModels;
using MauiScrollView = Microsoft.Maui.Controls.ScrollView;

namespace QuoteDetails.Views;

public class QuoteDetailsPage : ContentPage
{
    public QuoteDetailsPage(QuoteDetailsViewModel viewModel, CancelTapDelegate goBackFunc)
    {
        On<iOS>().SetUseSafeArea(true);
        Resources.MergedDictionaries.Add(new Styles());
        Shell.SetNavBarIsVisible(this, false);

        BindingContext = viewModel;

#pragma warning disable CA1416 // Validate platform compatibility
        Behaviors.Add(
            new StatusBarBehavior
            {
                StatusBarColor = Colors.White,
                StatusBarStyle = StatusBarStyle.DarkContent
            }
        );
#pragma warning restore CA1416 // Validate platform compatibility

        viewModel.OnStateChanged += StateChangeListener;

        # region First Toolbar Item
        var fntSource = new FontImageSource
        {
            FontFamily = "MaterialIconsRegular",
            Size = 48,
            Color = Colors.Black
        };

        fntSource.SetBinding(
            FontImageSource.GlyphProperty,
            new Binding
            {
                Source = this,
                Path = "BindingContext.Quote.IsFavorite",
                Converter = new BooleanToIconConverter
                {
                    FirstIcon = MaterialOutlineIcons.Favorite,
                    SecondIcon = MaterialOutlineIcons.FavoriteBorder
                }
            }
        );

        var favoriteImgBtn = new ImageButton
        {
            //Padding = new Thickness(8),
            HeightRequest = 24,
            WidthRequest = 24,
            Source = fntSource,
            BackgroundColor = Colors.Transparent
        };

        var commandConverter = new BooleanToCommandConverter
        {
            FirstCommandOption = viewModel.UnVoteQuoteCommand,
            SecondCommandOption = viewModel.UpVoteQuoteCommand
        };

        favoriteImgBtn.SetBinding(
            ImageButton.CommandProperty,
            new Binding
            {
                Source = this,
                Path = "BindingContext.Quote.IsFavorite",
                Converter = commandConverter
            }
        );

        #endregion

        #region upVoted Button

        var upVoteImgBtn = new CountIndicatorIconButton { Icon = MaterialOutlineIcons.ArrowUpward };

        var IsUpVotedCommandConverter = new BooleanToCommandConverter
        {
            FirstCommandOption = viewModel.UnVoteQuoteCommand,
            SecondCommandOption = viewModel.UpVoteQuoteCommand
        };
        upVoteImgBtn.SetBinding(
            CountIndicatorIconButton.OnTapProperty,
            new Binding
            {
                Source = this,
                Path = "BindingContext.Quote.IsUpVoted",
                Converter = IsUpVotedCommandConverter
            }
        );

        upVoteImgBtn.SetBinding(
            CountIndicatorIconButton.IconColorProperty,
            new Binding
            {
                Source = this,
                Path = "BindingContext.Quote.IsUpVoted",
                Converter = new BooleanToColorConverter
                {
                    FirstColor = (Color)Resources["VotedButtonColorLight"],
                    SecondColor = (Color)Resources["UnVotedButtonColorLight"]
                }
            }
        );

        upVoteImgBtn.SetBinding(
            CountIndicatorIconButton.CountProperty,
            new Binding { Source = this, Path = "BindingContext.Quote.UpVotesCount", }
        );
        #endregion

        #region downVoted Button
        var downVoteImgBtn = new CountIndicatorIconButton
        {
            Icon = MaterialOutlineIcons.ArrowDownward
        };

        var IsDownVotedCommandConverter = new BooleanToCommandConverter
        {
            FirstCommandOption = viewModel.UnVoteQuoteCommand,
            SecondCommandOption = viewModel.DownVoteQuoteCommand
        };

        downVoteImgBtn.SetBinding(
            CountIndicatorIconButton.OnTapProperty,
            new Binding
            {
                Source = this,
                Path = "BindingContext.Quote.IsDownVoted",
                Converter = IsDownVotedCommandConverter
            }
        );

        downVoteImgBtn.SetBinding(
            CountIndicatorIconButton.IconColorProperty,
            new Binding
            {
                Source = this,
                Path = "BindingContext.Quote.IsDownVoted",
                Converter = new BooleanToColorConverter
                {
                    FirstColor = (Color)Resources["VotedButtonColorLight"],
                    SecondColor = (Color)Resources["UnVotedButtonColorLight"]
                }
            }
        );

        downVoteImgBtn.SetBinding(
            CountIndicatorIconButton.CountProperty,
            new Binding { Source = this, Path = "BindingContext.Quote.DownVotesCount", }
        );
        #endregion

        #region App Bar

        var goBackImgBtn = new ImageButton
        {
            // Padding = new Thickness(4),
            Margin = new Thickness(0),
            HeightRequest = 28,
            WidthRequest = 28,
            Source = new FontImageSource
            {
                Glyph = MaterialOutlineIcons.ChevronLeft,
                FontFamily = "MaterialIconsRegular",
                Size = 36,
                Color = Colors.Black
            },
            BackgroundColor = Colors.Transparent,
            Command = new Command(() => goBackFunc())
        };

        var shareBtn = new ImageButton
        {
            // Padding = new Thickness(8),
            Margin = new Thickness(0),
            HeightRequest = 22,
            WidthRequest = 22,
            Source = new FontImageSource
            {
                Glyph = MaterialRoundIcons.Share,
                FontFamily = "MaterialIconsRegular",
                Size = 28,
                Color = Colors.Black
            },
            BackgroundColor = Colors.Transparent
        };

        var actionItems = new Grid
        {
            VerticalOptions = LayoutOptions.Center,
            ColumnDefinitions =
            [
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star),
            ],
            Shadow = (Shadow)Resources["ShadowElevation0"]
        };
        actionItems.Add(goBackImgBtn, column: 0, row: 0);
        actionItems.Add(favoriteImgBtn, column: 1, row: 0);
        actionItems.Add(upVoteImgBtn, column: 2, row: 0);
        actionItems.Add(downVoteImgBtn, column: 3, row: 0);
        actionItems.Add(shareBtn, column: 4, row: 0);

        #endregion

        #region Page Content

        var quoteLayout = new Grid
        {
            VerticalOptions = LayoutOptions.Center,
            ColumnDefinitions =
            [
                new ColumnDefinition(GridLength.Auto),
                new ColumnDefinition(),
                new ColumnDefinition(GridLength.Auto),
            ],
            RowDefinitions =
            [
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Star),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
            ]
        };

        var openingQuote = new Image
        {
            WidthRequest = 24,
            HeightRequest = 24,
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Center,
            Source = ImageSource.FromFile("opening_quote.png")
        };
        quoteLayout.Add(openingQuote, column: 0, row: 0);

        var closingQuote = new Image
        {
            WidthRequest = 24,
            HeightRequest = 24,
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Center,
            Source = "closing_quote.png"
        };
        quoteLayout.Add(closingQuote, column: 2, row: 2);

        var quote = new Label
        {
            TextColor = Colors.Black,
            FontSize = 22,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center
        };
        quote.SetBinding(
            Label.TextProperty,
            new Binding { Source = this, Path = "BindingContext.Quote.Body" }
        );
        Grid.SetRow(quote, 1);
        Grid.SetColumnSpan(quote, 3);

        quoteLayout.Add(quote);

        var author = new Label
        {
            TextColor = Colors.Black,
            FontSize = 18,
            HorizontalTextAlignment = TextAlignment.End,
            // VerticalTextAlignment = TextAlignment.Center
        };
        author.SetBinding(
            Label.TextProperty,
            new Binding { Source = this, Path = "BindingContext.Quote.Author" }
        );

        quoteLayout.Add(author, row: 3, column: 2);

        var contentContainer = new MauiScrollView
        {
            Padding = new Thickness((double)Resources["MediumLargeSpacing"]),
            Content = quoteLayout
        };

        var pageContent = new Grid
        {
            RowDefinitions =
            [
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Star),
                new RowDefinition(GridLength.Auto),
            ],
        };

        pageContent.Add(actionItems);
        pageContent.Add(contentContainer, column: 0, row: 1);

        Content = pageContent;

        #endregion
    }

    private static void StateChangeListener(object? sender, EventArgs e)
    {
        var quoteUpdateError = ((QuoteDetailsViewModel)sender!).QuoteUpdateError;

        switch (quoteUpdateError)
        {
            case null:
                return;
            case UserAuthenticationRequiredException:
                AuthenticationRequiredErrorSnackBar.MakeSnackBar();
                break;
            default:
                GenericErrorSnackBar.MakeSnackBar();
                break;
        }

        ((QuoteDetailsViewModel)sender!).QuoteUpdateError = null;
    }
}

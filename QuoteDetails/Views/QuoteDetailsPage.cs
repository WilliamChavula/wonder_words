using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using UraniumUI.Icons.MaterialSymbols;
using ControlsLibrary;
using ControlsLibrary.Resources.Styles;
using DomainModels;
using QuoteDetails.ViewModels;

namespace QuoteDetails.Views;

public class QuoteDetailsPage : ContentPage
{
    public QuoteDetailsPage(QuoteDetailsViewModel viewModel)
    {
        On<iOS>().SetUseSafeArea(true);
        Resources.MergedDictionaries.Add(new Styles());
        // var quote = viewModel.QuoteDetailsState?.Quote;
        BindingContext = viewModel;

        Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Colors.Black,
            StatusBarStyle = StatusBarStyle.LightContent
        });

        // var command = (bool)quote?.IsFavorite! ? viewModel.UnVoteQuoteCommand : viewModel.UpVoteQuoteCommand;

        // viewModel.QuoteDetailsState!.OnStateChanged += StateChangeListener;

        var fntSource = new FontImageSource();

        // Glyph = (bool)quote?.IsFavorite! ? MaterialRounded.Favorite : MaterialOutlined.Favorite
        // fntSource.SetBinding(FontImageSource.GlyphProperty, "");

        var toolbarItem = new ToolbarItem
        {
            IconImageSource = fntSource,

        };

        // Command = (bool)quote.IsFavorite! ? viewModel.UnVoteQuoteCommand : viewModel.UpVoteQuoteCommand

        // toolbarItem.SetBinding(ToolbarItem.CommandProperty, "");

        // ToolbarItems.Add(toolbarItem);

        var toolbarItem2 = new ToolbarItem
        {
            IconImageSource = new FontImageSource
            {
                Glyph = MaterialRounded.Arrow_downward,
                // Color = (bool)quote.IsDownVoted!
                //     ? (Color)Resources["votedButtonColorDark"]
                //     : (Color)Resources["unVotedButtonColorDark"]
            },
            // Command = (bool)quote.IsDownVoted! ? viewModel.UnVoteQuoteCommand : viewModel.DownVoteQuoteCommand,
        };

        // ToolbarItems.Add(toolbarItem2);

        Content = new VerticalStackLayout
        {
            Padding = new Thickness((double)Resources["MediumLargeSpacing"]),

            Children =
            {
                new ExceptionIndicator().Bind(
                        IsVisibleProperty,
                        getter: static (
                            QuoteDetailsViewModel viewModel) => viewModel.QuoteDetailsState is
                            { QuoteDetailFailed: true },
                        handlers:
                        [
                            (vm => vm, nameof(QuoteDetailsViewModel.QuoteDetailsState)),
                            (vm => vm.QuoteDetailsState,
                                nameof(QuoteDetailsViewModel.QuoteDetailsState.QuoteDetailFailed)),
                        ])
                    .Bind(
                        ExceptionIndicator.TryAgainProperty,
                        nameof(viewModel.ReFetchCommand)),

                new VerticalStackLayout
                {
                    HorizontalOptions = LayoutOptions.End,
                    Children =
                    {
                        new Image
                        {
                            WidthRequest = 46,
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Center,
                            Source = ImageSource.FromFile("opening_quote.svg")
                        },

                        new Label
                            {
                                Padding = new Thickness
                                {
                                    Left = (double)Resources["XxLargeSpacing"],
                                    Right = (double)Resources["XxLargeSpacing"]
                                },
                                HorizontalTextAlignment = TextAlignment.Center,
                                VerticalTextAlignment = TextAlignment.Center,
                                HorizontalOptions = LayoutOptions.Center,
                                VerticalOptions = LayoutOptions.Center,
                                FontSize = (double)Resources["XXl"]
                            }.Grow(1)
                            .Bind(Label.TextProperty,
                                static (QuoteDetailsViewModel vm) => vm.QuoteDetailsState!.Quote!.Body),

                        new Image
                        {
                            WidthRequest = 46,
                            Source = "closing_quote.svg"
                        },

                        new BoxView { HeightRequest = (double)Resources["MediumSpacing"] },

                        new Label { FontSize = (double)Resources["Large"] }
                            .Bind(Label.TextProperty,
                                nameof(viewModel.QuoteDetailsState.Quote.Author))
                            .Bind(IsVisibleProperty,
                                nameof(viewModel.QuoteDetailsState.Quote.Author),
                                converter: new IsNotNullConverter())
                    }
                }
            }
        };
    }

    private static void StateChangeListener(object? sender, EventArgs e)
    {
        var quoteUpdateError = ((QuoteDetailsState)sender!).QuoteUpdateError;

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
    }
}
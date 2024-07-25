using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using ControlsLibrary;
using ControlsLibrary.Resources.Styles;
using DomainModels;
using Microsoft.Maui.Layouts;
using QuoteList.Controls;
using QuoteList.ViewModels;
using FlexLayout = Microsoft.Maui.Controls.Compatibility.FlexLayout;
using L10n = QuoteList.Resources.Resources;
using SearchBar = Microsoft.Maui.Controls.SearchBar;

namespace QuoteList.Views;

public class QuoteListPage : ContentPage
{
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    public QuoteListPage(QuoteListViewModel viewModel)
    {
        On<iOS>().SetUseSafeArea(true);
        Resources.MergedDictionaries.Add(new Styles());

        Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Colors.Black,
            StatusBarStyle = StatusBarStyle.LightContent
        });

        BindingContext = viewModel;

        viewModel.EncounteredRefreshError += RefreshErrorStateListener;
        viewModel.EncounteredFavoriteToggleError += FavoriteToggleErrorStateListener;
        
        var searchBar = new SearchBar
        {
            Placeholder = "Search...",
            HorizontalOptions = LayoutOptions.Fill,
            VerticalTextAlignment = TextAlignment.Center
        };
        searchBar.SetBinding(SearchBar.TextProperty, new Binding("SearchTerm", BindingMode.TwoWay));

        var refreshView = new RefreshView
        {
            Content = new QuotePagedGridView()
        };
        refreshView.SetBinding(RefreshView.IsRefreshingProperty, new Binding
        {
            Source = new RelativeBindingSource(RelativeBindingSourceMode.FindAncestorBindingContext,
                typeof(QuoteListViewModel)),
            Path = "IsRefreshing",
        });
        refreshView.SetBinding(RefreshView.CommandProperty, new Binding
        {
            Source = new RelativeBindingSource(RelativeBindingSourceMode.FindAncestorBindingContext,
                typeof(QuoteListViewModel)),
            Path = "QuoteListRefreshedCommand",
        });
        refreshView.Grow(1);

        Content = new FlexLayout
        {
            Direction = FlexDirection.Column,
            Children =
            {
                new Border
                {
                    Padding = new Thickness
                    {
                        Left = (double)Resources["MediumLargeSpacing"],
                        Right = (double)Resources["MediumLargeSpacing"]
                    },
                    Content = searchBar
                },
                new FilterHorizontalList(),
                refreshView
            }
        };
    }

    private static void FavoriteToggleErrorStateListener(object? sender, EventArgs e)
    {
        var viewModel = (QuoteListViewModel)sender!;

        switch (viewModel.FavoriteToggleError)
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

    private static async void RefreshErrorStateListener(object? sender, EventArgs e)
    {
        var viewModel = (QuoteListViewModel)sender!;

        if (viewModel.RefreshError is null) return;

        var snackbarOptions = new SnackbarOptions
        {
            CornerRadius = new CornerRadius(10),
            Font = Microsoft.Maui.Font.SystemFontOfSize(14),
        };

        var text = L10n.quoteListRefreshErrorMessage;
        var duration = TimeSpan.FromSeconds(8);

        var snackbar = Snackbar.Make(text, duration: duration, visualOptions: snackbarOptions);
        await snackbar.Dismiss();
        await snackbar.Show();
    }
}
using ControlsLibrary;
using DomainModels;
using QuoteList.Converters;
using QuoteList.Extensions;
using QuoteList.ViewModels;
using UraniumUI.Views;

namespace QuoteList.Controls;

public class TagChip : ContentView
{
    public static readonly BindableProperty TagNameProperty = BindableProperty.Create(
        nameof(TagName),
        typeof(Tag),
        typeof(TagChip)
    );

    public static readonly BindableProperty SelectedTagProperty = BindableProperty.Create(
        nameof(SelectedTag),
        typeof(Tag),
        typeof(TagChip)
    );

    public Tag TagName
    {
        get => (Tag)GetValue(TagNameProperty);
        set => SetValue(TagNameProperty, value);
    }

    public Tag SelectedTag
    {
        get => (Tag)GetValue(SelectedTagProperty);
        set => SetValue(SelectedTagProperty, value);
    }

    public TagChip()
    {
        var isLastTag = ((Tag[])Enum.GetValues(typeof(Tag))).Last() == TagName;
        var isSelected = SelectedTag == TagName;

        var choiceChip = new RoundedChoiceChip
        {
            LabelText = TagName.ToLocalizedString()
        };
        choiceChip.SetBinding(RoundedChoiceChip.IsSelectedProperty, new Binding
        {
            Source = new RelativeBindingSource(RelativeBindingSourceMode.FindAncestorBindingContext,
                typeof(QuoteListViewModel)),
            Path = "Tag",
            Converter = new IsTagSelectedBoolConverter(),
            ConverterParameter = SelectedTag
        });
        choiceChip.SetBinding(RoundedChoiceChip.OnSelectedProperty, new Binding
        {
            Source = new RelativeBindingSource(
                RelativeBindingSourceMode.FindAncestorBindingContext,
                typeof(QuoteListViewModel)
            ),
            Path = "QuoteListTagChangedCommand",
        });
        choiceChip.SetBinding(StatefulContentView.CommandParameterProperty, isSelected ? nameof(TagName) : null);

        Padding = new Thickness
        {
            Right = isLastTag ? (double)Resources["MediumLargeSpacing"] : (double)Resources["XSmallSpacing"],
            Left = (double)Resources["MediumLargeSpacing"]
        };

        Content = choiceChip;
    }
}
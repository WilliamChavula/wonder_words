using ControlsLibrary;
using ControlsLibrary.Resources.Styles;
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
        typeof(string),
        typeof(TagChip),
        propertyChanged: TagNamePropertyChanged
    );

    private static readonly BindableProperty SelectedTagProperty = BindableProperty.Create(
        nameof(SelectedTag),
        typeof(Tag),
        typeof(TagChip),
        propertyChanged: SelectedTagPropertyChanged
    );

    private static void TagNamePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var cls = (TagChip)bindable;
        
        var last = ((Tag[])Enum.GetValues(typeof(Tag))).Last();

        cls._isLastTag = last.ToLocalizedString() == cls.TagName;
    }

    private static void SelectedTagPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var cls = (TagChip)bindable;

        cls._isSelected = cls.TagName == cls.SelectedTag.ToLocalizedString();
        cls._selectedTag = cls.SelectedTag;
    }

    public string TagName
    {
        get => (string)GetValue(TagNameProperty);
        set => SetValue(TagNameProperty, value);
    }

    public Tag SelectedTag
    {
        get => (Tag)GetValue(SelectedTagProperty);
        set => SetValue(SelectedTagProperty, value);
    }

    private bool _isLastTag;
    private bool _isSelected;
    private Tag _selectedTag;

    public TagChip()
    {
        Resources.MergedDictionaries.Add(new Styles());

        var choiceChip = new RoundedChoiceChip();
        choiceChip.SetBinding(RoundedChoiceChip.LabelTextProperty, new Binding
        {
            Source = this,
            Path = nameof(TagName)
        });
        choiceChip.SetBinding(RoundedChoiceChip.IsSelectedProperty, new Binding
        {
            Source = new RelativeBindingSource(RelativeBindingSourceMode.FindAncestorBindingContext,
                typeof(QuoteListViewModel)),
            Path = "Tag",
            Converter = new IsTagSelectedBoolConverter(),
            ConverterParameter = _selectedTag
        });
        choiceChip.SetBinding(RoundedChoiceChip.SelectCommandProperty, new Binding
        {
            Source = new RelativeBindingSource(
                RelativeBindingSourceMode.FindAncestorBindingContext,
                typeof(QuoteListViewModel)
            ),
            Path = "QuoteListTagChangedCommand",
        });
        choiceChip.SetBinding(StatefulContentView.CommandParameterProperty, new Binding
        {
            Source = this,
            Path = _isSelected ? nameof(TagName) : null
        });

        Padding = new Thickness
        {
            Right = _isLastTag ? (double)Resources["MediumLargeSpacing"] : (double)Resources["XSmallSpacing"]
        };

        Content = choiceChip;
    }
}
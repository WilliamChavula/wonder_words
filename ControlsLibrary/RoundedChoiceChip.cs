using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using ControlsLibrary.Resources.Styles;
using Microsoft.Maui.Controls.Shapes;
using UraniumUI.Views;

namespace ControlsLibrary;

public class RoundedChoiceChip : StatefulContentView
{
    public string LabelText
    {
        get => (string)GetValue(LabelTextProperty);
        set => SetValue(LabelTextProperty, value);
    }

    public AvatarView? Avatar
    {
        get => (AvatarView)GetValue(AvatarProperty);
        set => SetValue(AvatarProperty, value);
    }

    public ICommand? SelectCommand
    {
        get => (ICommand)GetValue(SelectCommandProperty);
        set => SetValue(SelectCommandProperty, value);
    }

    public Color? LabelColor
    {
        get => (Color)GetValue(LabelColorProperty);
        set => SetValue(LabelColorProperty, value);
    }

    public Color? SelectedLabelColor
    {
        get => (Color)GetValue(SelectedLabelColorProperty);
        set => SetValue(SelectedLabelColorProperty, value);
    }

    public Color? ChipBackgroundColor
    {
        get => (Color)GetValue(ChipBackgroundColorProperty);
        set => SetValue(ChipBackgroundColorProperty, value);
    }

    public Color? SelectedBackgroundColor
    {
        get => (Color)GetValue(SelectedBackgroundColorProperty);
        set => SetValue(SelectedBackgroundColorProperty, value);
    }

    public bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(
        nameof(LabelText),
        typeof(string),
        typeof(RoundedChoiceChip),
        propertyChanged: LabelTextPropertyChanged
    );

    private static void LabelTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var cls = (RoundedChoiceChip)bindable;
        cls.LabelText = (string)newValue;
    }

    public static readonly BindableProperty AvatarProperty = BindableProperty.Create(
        nameof(Avatar),
        typeof(AvatarView),
        typeof(RoundedChoiceChip)
    );

    public static readonly BindableProperty SelectCommandProperty = BindableProperty.Create(
        nameof(SelectCommand),
        typeof(ICommand),
        typeof(RoundedChoiceChip)
    );

    public static readonly BindableProperty LabelColorProperty = BindableProperty.Create(
        nameof(LabelColor),
        typeof(Color),
        typeof(RoundedChoiceChip)
    );

    public static readonly BindableProperty SelectedLabelColorProperty = BindableProperty.Create(
        nameof(SelectedLabelColor),
        typeof(Color),
        typeof(RoundedChoiceChip)
    );

    public static readonly BindableProperty ChipBackgroundColorProperty = BindableProperty.Create(
        nameof(ChipBackgroundColor),
        typeof(Color),
        typeof(RoundedChoiceChip)
    );

    public static readonly BindableProperty SelectedBackgroundColorProperty = BindableProperty.Create(
        nameof(SelectedBackgroundColor),
        typeof(Color),
        typeof(RoundedChoiceChip)
    );

    public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(
        nameof(IsSelected),
        typeof(bool),
        typeof(RoundedChoiceChip),
        propertyChanged: (bindable, _, newValue) =>
        {
            if (bindable is RoundedChoiceChip chip)
            {
                chip.IsSelected = (bool)newValue;
            }
        }
    );

    public RoundedChoiceChip()
    {
        Resources.MergedDictionaries.Add(new Styles());

        var label = new Label
        {
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            FontSize = 14
        };

        if (IsSelected)
            label.SetAppThemeColor(Label.TextColorProperty, 
                SelectedLabelColor ?? (Color)Resources["Background"], 
                SelectedLabelColor ?? (Color)Resources["BackgroundDark"]);
        else
            label.SetAppThemeColor(Label.TextColorProperty, 
                LabelColor ?? (Color)Resources["OnBackground"], 
                LabelColor ?? (Color)Resources["OnBackgroundDark"]);
        
        label.SetBinding(Label.TextProperty, new Binding
        {
            Source = this,
            Path = nameof(LabelText)
        });

        var imgView = new Border { Stroke = Colors.Transparent };
        imgView.SetBinding(Border.ContentProperty, new Binding
        {
            Source = this,
            Path = nameof(Avatar)
        });

        var content =  new Border
        {
            Padding = new Thickness { Left = 4, Right = 8 },
            HorizontalOptions = LayoutOptions.Center,
            StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(16)
            },
            Content = new HorizontalStackLayout
            {
                Spacing = 5,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    imgView,
                    label
                }
            }
        };
        content.SetBinding(BackgroundColorProperty, new Binding
        {
            Source = this,
            Path = IsSelected ? nameof(SelectedBackgroundColor) : nameof(ChipBackgroundColor)
        });
        
        Content = content;

        // TappedCommand = SelectCommand;
        this.SetBinding(TappedCommandProperty, new Binding
        {
            Source = this,
            Path = nameof(SelectCommand)
        });
        Tapped += (_, _) => { IsSelected = !IsSelected; };
    }
}
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls.Shapes;
using UraniumUI.Resources;
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

    public ICommand? OnSelected
    {
        get => (ICommand)GetValue(OnSelectedProperty);
        set => SetValue(OnSelectedProperty, value);
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
        typeof(RoundedChoiceChip)
    );

    public static readonly BindableProperty AvatarProperty = BindableProperty.Create(
        nameof(Avatar),
        typeof(AvatarView),
        typeof(RoundedChoiceChip)
    );

    public static readonly BindableProperty OnSelectedProperty = BindableProperty.Create(
        nameof(OnSelected),
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
        Content = new Border
        {
            BackgroundColor = IsSelected ? SelectedBackgroundColor : ChipBackgroundColor,
            StrokeShape = new RoundRectangle
            {
                CornerRadius = 20,
            },
            Content = new HorizontalStackLayout
            {
                Spacing = 5,
                Children =
                {
                    Avatar,
                    new Label
                    {
                        Text = LabelText,
                        TextColor = IsSelected ? SelectedLabelColor ?? AssignColor() : LabelColor ?? AssignColor()
                    }
                }
            }
        };

        TappedCommand = OnSelected;
        Tapped += (_, _) => { IsSelected = !IsSelected; };
    }

    private static Color AssignColor()
    {
        if (Application.Current != null && Application.Current.RequestedTheme == AppTheme.Dark)
        {
            return ColorResource.GetColor("PrimaryDark", Colors.White);
        }

        return ColorResource.GetColor("Primary", Colors.Black);
    }
}
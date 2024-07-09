using System.Windows.Input;
using CommunityToolkit.Maui.Markup;
using UraniumUI.Icons.MaterialSymbols;
using UraniumUI.Material.Controls;

namespace ControlsLibrary;

public class ShareIconButton : ContentView
{
    public ICommand OnTap
    {
        get => (ICommand)GetValue(OnTapProperty);
        set => SetValue(OnTapProperty, value);
    }

    public static readonly BindableProperty OnTapProperty = BindableProperty.Create(
        nameof(OnTap),
        typeof(ICommand),
        typeof(ShareIconButton)
    );

    public ShareIconButton()
    {
        var btn = new ButtonView
        {
            Content = new Image
            {
                Source = new FontImageSource
                {
                    Glyph = MaterialRounded.Share
                }
            }
        }.Bind(ButtonView.TappedCommandProperty, nameof(OnTap));

        Content = btn;
        ToolTipProperties.SetText(btn, ControlsLibrary.Resources.Resources.shareIconButtonTooltip);
    }
}
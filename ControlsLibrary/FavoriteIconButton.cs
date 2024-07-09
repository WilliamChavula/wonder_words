using System.Reflection;
using System.Resources;
using System.Windows.Input;
using CommunityToolkit.Maui.Markup;
using UraniumUI.Material.Controls;
using UraniumUI.Icons.MaterialSymbols;

namespace ControlsLibrary;

public class FavoriteIconButton : ButtonView
{
    public static readonly BindableProperty IsFavoriteProperty = BindableProperty.Create(nameof(IsFavorite), typeof(bool), typeof(FavoriteIconButton));
    public static readonly BindableProperty OnTapProperty = BindableProperty.Create(nameof(OnTap), typeof(ICommand), typeof(FavoriteIconButton));

    
    public FavoriteIconButton()
    {
        var res = new ResourceManager(@"ControlsLibrary.Resources.Resources", Assembly.GetExecutingAssembly());
        var btn = new ButtonView
        {
            Content = new Image
            {
                Source = new FontImageSource
                {
                    Glyph = IsFavorite ? MaterialRounded.Favorite : MaterialOutlined.Favorite
                }
            }
            
        }.Bind(PressedCommandProperty, nameof(OnTap));
        
        ToolTipProperties.SetText(btn, res.GetString("favoriteIconButtonTooltip")!);

        Content = btn;
    }

    public bool IsFavorite
    {
        get => (bool)GetValue(IsFavoriteProperty); 
        set => SetValue(IsFavoriteProperty, value);
    }

    public ICommand OnTap
    {
        get => (ICommand)GetValue(OnTapProperty); 
        set => SetValue(OnTapProperty, value);
    }
}
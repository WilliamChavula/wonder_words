using ControlsLibrary.Resources.Styles;

namespace Maui.Wonder.Words;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Resources.MergedDictionaries.Add(new Styles());
    }
}

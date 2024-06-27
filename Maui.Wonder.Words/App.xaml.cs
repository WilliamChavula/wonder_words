using Microsoft.Maui.Controls;

namespace Maui.Wonder.Words;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}
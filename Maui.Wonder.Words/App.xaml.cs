using QuoteDetails.Views;
using SignIn.Views;
using SignUp.Views;

namespace Maui.Wonder.Words;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();

        Routing.RegisterRoute(nameof(SignInPage), typeof(SignInPage));
        Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
        Routing.RegisterRoute("quoteDetails", typeof(QuoteDetailsPage));
    }
}

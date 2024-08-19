using QuoteDetails.Views;
using QuoteList.Views;
using SignIn.Views;
using SignUp.Views;

namespace Maui.Wonder.Words;
public partial class App : Application
{
    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        MainPage = new QuoteListShellV2(serviceProvider);
        
        Routing.RegisterRoute(nameof(SignInPage), typeof(SignInPage));
        Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
        Routing.RegisterRoute("quoteDetails", typeof(QuoteDetailsPage));
    }
}
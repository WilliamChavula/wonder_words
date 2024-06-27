namespace ForgotMyPassword.Control;

public partial class ForgotMyPasswordDialog : ContentPage
{
    public ForgotMyPasswordDialog(ForgotMyPasswordViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
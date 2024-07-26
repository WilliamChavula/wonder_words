using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using ControlsLibrary;
using InputKit.Shared.Validations;
using SignIn.Converters;
using SignIn.ViewModels;
using UraniumUI.Icons.MaterialSymbols;
using UraniumUI.Material.Controls;
using L10n = SignIn.Resources.Resources;

namespace SignIn.Controls;

internal class SignInForm : ContentView
{
    internal SignInForm(SignInViewModel viewModel)
    {
        BindingContext = viewModel;


        var hasSubmissionError = viewModel.SubmissionStatus
            is SubmissionStatus.InvalidCredentialsError
            or SubmissionStatus.GenericError;

        if (hasSubmissionError)
        {
            if (viewModel.SubmissionStatus is SubmissionStatus.InvalidCredentialsError)
            {
                var snackbarOptions = new SnackbarOptions
                {
                    CornerRadius = new CornerRadius(10),
                    Font = Microsoft.Maui.Font.SystemFontOfSize(14),
                };

                var text = L10n.invalidCredentialsErrorMessage;
                var duration = TimeSpan.FromSeconds(8);

                var snackbar = Snackbar.Make(text, duration: duration, visualOptions: snackbarOptions);
                snackbar.Dismiss();
                snackbar.Show();
            }
            else
            {
                GenericErrorSnackBar.MakeSnackBar();
            }
        }

        #region PasswordField

        var passwordValidationText = new RequiredValidation();
        passwordValidationText.SetBinding(
            RequiredValidation.MessageProperty,
            nameof(viewModel.Password.Error),
            converter: new PasswordValidationTextConverter()
        );

        var passwordField = new TextField
        {
            IsPassword = true,
            Icon = MaterialRounded.Password,
            Title = L10n.passwordTextFieldLabel,
            Validations =
            {
                passwordValidationText
            }
        };

        passwordField.SetBinding(TextField.IsEnabledProperty,
            nameof(viewModel.SubmissionStatus),
            converter: new SubmissionInProgressInvertedConverter());
        var passwordTextChanged = new EventToCommandBehavior
        {
            EventName = "TextChanged",
        };
        passwordTextChanged.SetBinding(EventToCommandBehavior.CommandProperty,
            nameof(viewModel.PasswordChangedCommand));
        passwordField.SetBinding(EventToCommandBehavior.CommandParameterProperty, nameof(passwordField.Text));

        passwordField.Behaviors.Add(passwordTextChanged);

        var editingCompleted = new EventToCommandBehavior
        {
            EventName = "Completed",
        };
        editingCompleted.SetBinding(EventToCommandBehavior.CommandProperty,
            nameof(viewModel.SubmitCommand));

        passwordField.Behaviors.Add(editingCompleted);

        #endregion


        #region EmailField

        var emailValidationText = new RequiredValidation();
        emailValidationText.SetBinding(RequiredValidation.MessageProperty, nameof(viewModel.Email.Error),
            converter: new EmailValidationTextConverter());

        var emailField = new TextField
        {
            ReturnType = ReturnType.Next,
            ReturnCommand = new Command(() => passwordField.Focus()),
            IsTextPredictionEnabled = false,
            Keyboard = Keyboard.Email,
            Icon = MaterialRounded.Alternate_email,
            Title = L10n.emailTextFieldLabel,
            Validations =
            {
                emailValidationText
            }
        };
        emailField.Focus();

        var emailTextChanged = new EventToCommandBehavior
        {
            EventName = "TextChanged",
        };
        emailTextChanged.SetBinding(EventToCommandBehavior.CommandProperty, nameof(viewModel.EmailChangedCommand));
        emailTextChanged.SetBinding(EventToCommandBehavior.CommandParameterProperty, nameof(emailField.Text));
        emailField.Behaviors.Add(emailTextChanged);

        emailField.SetBinding(TextField.IsEnabledProperty, nameof(viewModel.SubmissionStatus),
            converter: new SubmissionInProgressInvertedConverter());

        #endregion

        #region Buttons

        var forgotPasswordBtn = new Button
        {
            StyleClass = { "TextButton" },
            Text = L10n.forgotMyPasswordButtonLabel
        };
        forgotPasswordBtn.SetBinding(Button.CommandProperty, nameof(viewModel.ForgotPasswordTapCommand));
        forgotPasswordBtn.SetBinding(Button.IsEnabledProperty, nameof(viewModel.SubmissionStatus),
            converter: new SubmissionInProgressInvertedConverter());

        var signUpBtn = new Button
        {
            StyleClass = { "TextButton" },
            Text = L10n.signUpButtonLabel
        };
        signUpBtn.SetBinding(Button.CommandProperty, nameof(viewModel.SignUpTapCommand));
        signUpBtn.SetBinding(Button.IsEnabledProperty, nameof(viewModel.SubmissionStatus),
            converter: new SubmissionInProgressInvertedConverter());

        var signInInProgress = new InProgressExpandedElevatedButton
        {
            TextLabel = L10n.signUpButtonLabel
        };
        signInInProgress.SetBinding(
            ButtonView.IsVisibleProperty,
            nameof(viewModel.SubmissionStatus),
            converter: new SubmissionInProgressConverter()
        );

        var signInBtn = new ExpandedElevatedButton
        {
            TextLabel = L10n.signInButtonLabel,
            Icon = MaterialRounded.Login
        };
        signInBtn.SetBinding(ExpandedElevatedButton.OnTapProperty, nameof(viewModel.SubmitCommand));
        
        signInBtn.SetBinding(
            ExpandedElevatedButton.IsVisibleProperty,
            nameof(viewModel.SubmissionStatus),
            converter: new SubmissionInProgressInvertedConverter()
        );

        #endregion


        Content = new VerticalStackLayout
        {
            Children =
            {
                emailField,
                new BoxView { HeightRequest = (double)Resources["LargeSpacing"] },
                passwordField,
                forgotPasswordBtn,
                new BoxView { HeightRequest = (double)Resources["SmallSpacing"] },

                signInBtn,
                signInInProgress,

                new BoxView { HeightRequest = (double)Resources["XxLargeSpacing"] },
                new Label { Text = L10n.signUpOpeningText },
                signUpBtn
            }
        };
    }
}
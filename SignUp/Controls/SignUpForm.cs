using System.ComponentModel;
using CommunityToolkit.Maui.Behaviors;
using UraniumUI.Material.Controls;
using UraniumUI.Views;
using ControlsLibrary;
using FormFields.Inputs;
using InputKit.Shared.Validations;
using SignUp.Converters;
using SignUp.ViewModels;
using UraniumUI.Icons.MaterialSymbols;
using L10n = SignUp.Resources.Resources;

namespace SignUp.Controls;

internal class SignUpForm : StatefulContentView
{
    internal SignUpForm(SignUpViewModel viewModel)
    {
        #region Variables

        var spacer = new BoxView { HeightRequest = (double)Resources["MediumLargeSpacing"] };
        var tripleLargeSpacer = new BoxView { HeightRequest = (double)Resources["XxxLargeSpacing"] };

        #endregion

        #region EmailTextField

        var emailValidationText = new RequiredValidation();
        emailValidationText.SetBinding(RequiredValidation.MessageProperty, nameof(viewModel.Email.Error),
            converter: new ValidationTextConverter<EmailValidationError>());

        var emailEntry = new TextField
        {
            Keyboard = Keyboard.Email,
            ReturnType = ReturnType.Next,
            IsTextPredictionEnabled = false,
            Icon = MaterialRounded.Alternate_email,
            Title = L10n.emailTextFieldLabel,
            Validations = { emailValidationText }
        };
        emailEntry.SetBinding(IsEnabledProperty, nameof(viewModel.SubmissionStatus),
            converter: new SubmissionInProgressInvertedConverter());

        var emailEntryTextChangedBehavior = new EventToCommandBehavior
        {
            EventName = "TextChanged"
        };
        emailEntryTextChangedBehavior.SetBinding(EventToCommandBehavior.CommandProperty,
            nameof(viewModel.EmailChangedCommand));
        emailEntry.Behaviors.Add(emailEntryTextChangedBehavior);

        var emailEntryFocusedBehavior = new EventToCommandBehavior { EventName = nameof(Unfocused) };
        emailEntryFocusedBehavior.SetBinding(EventToCommandBehavior.CommandProperty,
            nameof(viewModel.EmailUnfocusedCommand));
        emailEntry.Behaviors.Add(emailEntryFocusedBehavior);

        #endregion

        #region UsernameTextField

        var usernameEntryValidation = new RequiredValidation();
        usernameEntryValidation.SetBinding(RequiredValidation.MessageProperty, nameof(viewModel.Username.Error),
            converter: new ValidationTextConverter<UsernameValidationError>());

        var usernameEntry = new TextField
        {
            Validations = { usernameEntryValidation },
            IsTextPredictionEnabled = false,
            ReturnType = ReturnType.Next,
            Icon = MaterialRounded.Person,
            Title = L10n.usernameTextFieldLabel
        };
        usernameEntry.SetBinding(IsEnabledProperty, nameof(viewModel.SubmissionStatus),
            converter: new SubmissionInProgressInvertedConverter());

        var usernameTextChangedBehavior = new EventToCommandBehavior
        {
            EventName = "TextChanged"
        };
        usernameTextChangedBehavior.SetBinding(EventToCommandBehavior.CommandProperty,
            nameof(viewModel.UsernameChangedCommand));
        usernameEntry.Behaviors.Add(usernameTextChangedBehavior);


        var usernameEntryBehavior = new EventToCommandBehavior { EventName = nameof(Unfocused) };
        usernameEntryBehavior.SetBinding(EventToCommandBehavior.CommandProperty,
            nameof(viewModel.UsernameUnfocusedCommand));
        usernameEntry.Behaviors.Add(usernameEntryBehavior);

        #endregion

        #region PasswordTextField

        var passwordEntryValidation = new RequiredValidation();
        passwordEntryValidation.SetBinding(RequiredValidation.MessageProperty, nameof(viewModel.Password.Error),
            converter: new ValidationTextConverter<PasswordValidationError>());

        var passwordEntry = new TextField
        {
            Validations = { passwordEntryValidation },
            ReturnType = ReturnType.Next,
            IsPassword = true,
            Icon = MaterialRounded.Password,
            Title = L10n.passwordTextFieldLabel
        };

        passwordEntry.SetBinding(IsEnabledProperty, nameof(viewModel.SubmissionStatus),
            converter: new SubmissionInProgressInvertedConverter());

        var passwordTextChanged = new EventToCommandBehavior
        {
            EventName = "TextChanged"
        };
        passwordTextChanged.SetBinding(EventToCommandBehavior.CommandProperty,
            nameof(viewModel.PasswordChangedCommand));
        passwordEntry.Behaviors.Add(passwordTextChanged);

        var passwordEntryBehavior = new EventToCommandBehavior { EventName = nameof(Unfocused) };
        passwordEntryBehavior.SetBinding(EventToCommandBehavior.CommandProperty,
            nameof(viewModel.PasswordUnfocusedCommand));
        passwordEntry.Behaviors.Add(passwordEntryBehavior);

        #endregion

        #region PasswordConfirmationTextField

        var passwordConfirmationEntryValidation = new RequiredValidation();
        passwordConfirmationEntryValidation.SetBinding(RequiredValidation.MessageProperty,
            nameof(viewModel.PasswordConfirmation.Error),
            converter: new ValidationTextConverter<PasswordConfirmationValidationError>());

        var passwordConfirmationEntry = new TextField
        {
            Validations = { passwordConfirmationEntryValidation },
            IsPassword = true,
            Icon = MaterialRounded.Password,
            Title = L10n.passwordConfirmationTextFieldLabel
        };

        passwordConfirmationEntry.SetBinding(IsEnabledProperty, nameof(viewModel.SubmissionStatus),
            converter: new SubmissionInProgressInvertedConverter());
        passwordConfirmationEntry.SetBinding(TextField.ReturnCommandProperty, nameof(viewModel.SubmitCommand));

        var passwordConfirmationTextChanged = new EventToCommandBehavior
        {
            EventName = "TextChanged"
        };
        passwordConfirmationTextChanged.SetBinding(EventToCommandBehavior.CommandProperty,
            nameof(viewModel.PasswordConfirmationChangedCommand));
        passwordConfirmationEntry.Behaviors.Add(passwordConfirmationTextChanged);

        var passwordConfirmationEntryBehavior = new EventToCommandBehavior { EventName = nameof(Unfocused) };
        passwordConfirmationEntryBehavior.SetBinding(EventToCommandBehavior.CommandProperty,
            nameof(viewModel.PasswordConfirmationUnfocusedCommand));
        passwordConfirmationEntry.Behaviors.Add(passwordConfirmationEntryBehavior);

        var formEditingComplete = new EventToCommandBehavior
        {
            EventName = "Complete"
        };
        formEditingComplete.SetBinding(EventToCommandBehavior.CommandProperty, nameof(viewModel.SubmitCommand));

        #endregion

        #region Buttons

        var submitInProgressBtn = new InProgressExpandedElevatedButton
        {
            TextLabel = L10n.signUpButtonLabel
        };
        submitInProgressBtn.SetBinding(IsVisibleProperty, nameof(viewModel.SubmissionStatus),
            converter: new SubmissionInProgressConverter());

        var submitBtn = new ExpandedElevatedButton
        {
            TextLabel = L10n.signUpButtonLabel,
            Icon = MaterialRounded.Login
        };
        submitBtn.SetBinding(ExpandedElevatedButton.OnTapProperty, nameof(viewModel.SubmitCommand));
        submitBtn.SetBinding(IsVisibleProperty, nameof(viewModel.SubmissionStatus),
            converter: new SubmissionInProgressInvertedConverter());

        #endregion

        #region Events

        viewModel.PropertyChanged += ViewModelOnPropertyChanged;
        emailEntry.ReturnCommand = new Command(() => usernameEntry.Focus());
        usernameEntry.ReturnCommand = new Command(() => passwordEntry.Focus());
        passwordEntry.ReturnCommand = new Command(() => passwordConfirmationEntry.Focus());

        #endregion

        Content = new VerticalStackLayout
        {
            Children =
            {
                emailEntry,
                spacer,
                usernameEntry,
                spacer,
                passwordEntry,
                spacer,
                passwordConfirmationEntry,
                tripleLargeSpacer,
                submitInProgressBtn,
                submitBtn
            }
        };
    }

    private static void ViewModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        ArgumentNullException.ThrowIfNull(sender);
        if (e.PropertyName != "SubmissionStatus") return;

        var viewModel = (SignUpViewModel)sender;
        var submissionStatus = viewModel.SubmissionStatus;

        if (submissionStatus == SubmissionStatus.Success)
            viewModel.SignUpSuccessCommand.Execute(null);

        if (submissionStatus == SubmissionStatus.Error)
            GenericErrorSnackBar.MakeSnackBar();
    }
}
using System.ComponentModel;
using CommunityToolkit.Maui.Behaviors;
using ControlsLibrary;
using ControlsLibrary.Resources.Styles;
using FormFields.Inputs;
using SignUp.Converters;
using SignUp.ViewModels;
using UraniumUI.Icons.MaterialSymbols;
using UraniumUI.Material.Controls;
using L10n = SignUp.Resources.Resources;

namespace SignUp.Controls;

internal class SignUpForm : ContentView
{
    internal SignUpForm(SignUpViewModel viewModel)
    {
        Resources.MergedDictionaries.Add(new Styles());
        BindingContext = viewModel;

        #region EmailTextField

        var emailValidationText = new Label { Style = (Style)Resources["textFieldError"] };
        emailValidationText.SetBinding(
            Label.TextProperty,
            new Binding
            {
                Source = this,
                Path = "BindingContext.EmailError",
                Converter = new ValidationTextConverter<EmailValidationError>()
            }
        );

        var emailEntry = new TextField
        {
            Margin = new Thickness { Bottom = (double)Resources["XSmallSpacing"] },
            Keyboard = Keyboard.Email,
            ReturnType = ReturnType.Next,
            IsTextPredictionEnabled = false,
            Attachments =
            {
                new Image
                {
                    WidthRequest = 20,
                    HeightRequest = 20,
                    Margin = new Thickness { Right = 8 },
                    Source = new FontImageSource
                    {
                        Glyph = MaterialRounded.Alternate_email,
                        FontFamily = "MaterialRound",
                        Color = Colors.Gray,
                        Size = 20
                    }
                }
            },
            Title = L10n.emailTextFieldLabel
        };
        emailEntry.SetBinding(
            IsEnabledProperty,
            new Binding
            {
                Source = this,
                Path = "BindingContext.SubmissionStatus",
                Converter = new SubmissionInProgressInvertedConverter()
            }
        );

        var emailEntryTextChangedBehavior = new EventToCommandBehavior
        {
            EventName = "TextChanged"
        };
        emailEntryTextChangedBehavior.SetBinding(
            EventToCommandBehavior.CommandProperty,
            new Binding { Source = this, Path = "BindingContext.EmailChangedCommand" }
        );

        emailEntryTextChangedBehavior.SetBinding(
            EventToCommandBehavior.CommandParameterProperty,
            new Binding { Source = emailEntry, Path = "Text" }
        );

        emailEntry.Behaviors.Add(emailEntryTextChangedBehavior);

        var emailEntryFocusedBehavior = new EventToCommandBehavior
        {
            EventName = nameof(Unfocused)
        };
        emailEntryFocusedBehavior.SetBinding(
            EventToCommandBehavior.CommandProperty,
            new Binding { Source = this, Path = "BindingContext.EmailUnfocusedCommand" }
        );
        emailEntry.Behaviors.Add(emailEntryFocusedBehavior);

        #endregion

        #region UsernameTextField

        var usernameEntryValidation = new Label { Style = (Style)Resources["textFieldError"] };
        usernameEntryValidation.SetBinding(
            Label.TextProperty,
            new Binding
            {
                Source = this,
                Path = "BindingContext.UsernameError",
                Converter = new ValidationTextConverter<UsernameValidationError>()
            }
        );

        var usernameEntry = new TextField
        {
            Margin = new Thickness { Bottom = (double)Resources["XSmallSpacing"] },
            IsTextPredictionEnabled = false,
            ReturnType = ReturnType.Next,
            Attachments =
            {
                new Image
                {
                    WidthRequest = 20,
                    HeightRequest = 20,
                    Margin = new Thickness { Right = 8 },
                    Source = new FontImageSource
                    {
                        Glyph = MaterialRounded.Person,
                        FontFamily = "MaterialRound",
                        Color = Colors.Gray,
                        Size = 20
                    }
                }
            },
            Title = L10n.usernameTextFieldLabel
        };
        usernameEntry.SetBinding(
            IsEnabledProperty,
            new Binding
            {
                Source = this,
                Path = "BindingContext.SubmissionStatus",
                Converter = new SubmissionInProgressInvertedConverter()
            }
        );

        var usernameTextChangedBehavior = new EventToCommandBehavior { EventName = "TextChanged" };
        usernameTextChangedBehavior.SetBinding(
            EventToCommandBehavior.CommandProperty,
            new Binding { Source = this, Path = "BindingContext.UsernameChangedCommand" }
        );

        usernameTextChangedBehavior.SetBinding(
            EventToCommandBehavior.CommandParameterProperty,
            new Binding { Source = usernameEntry, Path = "Text" }
        );
        usernameEntry.Behaviors.Add(usernameTextChangedBehavior);

        var usernameEntryBehavior = new EventToCommandBehavior { EventName = nameof(Unfocused) };
        usernameEntryBehavior.SetBinding(
            EventToCommandBehavior.CommandProperty,
            new Binding { Source = this, Path = "BindingContext.UsernameUnfocusedCommand" }
        );
        usernameEntry.Behaviors.Add(usernameEntryBehavior);

        #endregion

        #region PasswordTextField

        var passwordEntryValidation = new Label { Style = (Style)Resources["textFieldError"] };
        passwordEntryValidation.SetBinding(
            Label.TextProperty,
            new Binding
            {
                Source = this,
                Path = "BindingContext.PasswordError",
                Converter = new ValidationTextConverter<PasswordValidationError>()
            }
        );

        var passwordEntry = new TextField
        {
            Margin = new Thickness { Bottom = (double)Resources["XSmallSpacing"] },
            ReturnType = ReturnType.Next,
            IsPassword = true,
            Attachments =
            {
                new Image
                {
                    WidthRequest = 20,
                    HeightRequest = 20,
                    Margin = new Thickness { Right = 8 },
                    Source = new FontImageSource
                    {
                        Glyph = MaterialRounded.Password,
                        FontFamily = "MaterialRound",
                        Color = Colors.Gray,
                        Size = 20
                    }
                }
            },
            Title = L10n.passwordTextFieldLabel
        };

        passwordEntry.SetBinding(
            IsEnabledProperty,
            new Binding
            {
                Source = this,
                Path = "BindingContext.SubmissionStatus",
                Converter = new SubmissionInProgressInvertedConverter()
            }
        );

        var passwordTextChanged = new EventToCommandBehavior { EventName = "TextChanged" };
        passwordTextChanged.SetBinding(
            EventToCommandBehavior.CommandProperty,
            new Binding { Source = this, Path = "BindingContext.PasswordChangedCommand" }
        );
        passwordTextChanged.SetBinding(
            EventToCommandBehavior.CommandParameterProperty,
            new Binding { Source = passwordEntry, Path = "Text" }
        );
        passwordEntry.Behaviors.Add(passwordTextChanged);

        var passwordEntryBehavior = new EventToCommandBehavior { EventName = nameof(Unfocused) };
        passwordEntryBehavior.SetBinding(
            EventToCommandBehavior.CommandProperty,
            new Binding { Source = this, Path = "BindingContext.PasswordUnfocusedCommand" }
        );
        passwordEntry.Behaviors.Add(passwordEntryBehavior);

        #endregion

        #region PasswordConfirmationTextField

        var passwordConfirmationEntryValidation = new Label
        {
            Style = (Style)Resources["textFieldError"]
        };
        passwordConfirmationEntryValidation.SetBinding(
            Label.TextProperty,
            new Binding
            {
                Source = this,
                Converter = new ValidationTextConverter<PasswordConfirmationValidationError>(),
                Path = "BindingContext.PasswordConfirmationError"
            }
        );

        var passwordConfirmationEntry = new TextField
        {
            Margin = new Thickness { Bottom = (double)Resources["XSmallSpacing"] },
            IsPassword = true,
            Attachments =
            {
                new Image
                {
                    WidthRequest = 20,
                    HeightRequest = 20,
                    Margin = new Thickness { Right = 8 },
                    Source = new FontImageSource
                    {
                        Glyph = MaterialRounded.Password,
                        FontFamily = "MaterialRound",
                        Color = Colors.Gray,
                        Size = 20
                    }
                }
            },
            Title = L10n.passwordConfirmationTextFieldLabel
        };

        passwordConfirmationEntry.SetBinding(
            IsEnabledProperty,
            new Binding
            {
                Source = this,
                Path = "BindingContext.SubmissionStatus",
                Converter = new SubmissionInProgressInvertedConverter()
            }
        );
        passwordConfirmationEntry.SetBinding(
            TextField.ReturnCommandProperty,
            new Binding { Source = this, Path = "BindingContext.SubmitCommand" }
        );

        var passwordConfirmationTextChanged = new EventToCommandBehavior
        {
            EventName = "TextChanged"
        };
        passwordConfirmationTextChanged.SetBinding(
            EventToCommandBehavior.CommandProperty,
            new Binding
            {
                Source = this,
                Path = "BindingContext.PasswordConfirmationChangedCommand"
            }
        );
        passwordConfirmationTextChanged.SetBinding(
            EventToCommandBehavior.CommandParameterProperty,
            new Binding { Source = passwordConfirmationEntry, Path = "Text" }
        );
        passwordConfirmationEntry.Behaviors.Add(passwordConfirmationTextChanged);

        var passwordConfirmationEntryBehavior = new EventToCommandBehavior
        {
            EventName = nameof(Unfocused)
        };
        passwordConfirmationEntryBehavior.SetBinding(
            EventToCommandBehavior.CommandProperty,
            new Binding
            {
                Source = this,
                Path = "BindingContext.PasswordConfirmationUnfocusedCommand"
            }
        );
        passwordConfirmationEntry.Behaviors.Add(passwordConfirmationEntryBehavior);

        var formEditingComplete = new EventToCommandBehavior { EventName = "Completed" };
        formEditingComplete.SetBinding(
            EventToCommandBehavior.CommandProperty,
            new Binding { Source = this, Path = "BindingContext.SubmitCommand" }
        );

        #endregion

        #region Buttons

        var submitInProgressBtn = new InProgressExpandedElevatedButton
        {
            TextLabel = L10n.signUpButtonLabel
        };
        submitInProgressBtn.SetBinding(
            IsVisibleProperty,
            new Binding
            {
                Source = this,
                Path = "BindingContext.SubmissionStatus",
                Converter = new SubmissionInProgressConverter()
            }
        );

        var submitBtn = new ExpandedElevatedButton
        {
            TextLabel = L10n.signUpButtonLabel,
            Icon = MaterialRounded.Login
        };
        submitBtn.SetBinding(
            ExpandedElevatedButton.OnTapProperty,
            new Binding { Source = this, Path = "BindingContext.SubmitCommand" }
        );
        submitBtn.SetBinding(
            IsVisibleProperty,
            new Binding
            {
                Source = this,
                Path = "BindingContext.SubmissionStatus",
                Converter = new SubmissionInProgressInvertedConverter()
            }
        );

        #endregion

        #region Events

        viewModel.PropertyChanged += ViewModelOnPropertyChanged;
        // emailEntry.ReturnCommand = new Command(() => usernameEntry.Focus());
        // usernameEntry.ReturnCommand = new Command(() => passwordEntry.Focus());
        // passwordEntry.ReturnCommand = new Command(() => passwordConfirmationEntry.Focus());

        #endregion

        var gridLayout = new Grid
        {
            ColumnDefinitions = [new ColumnDefinition(GridLength.Star)],
            RowDefinitions =
            [
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
            ]
        };
        gridLayout.Add(emailEntry);
        gridLayout.Add(emailValidationText, row: 1);
        gridLayout.Add(new BoxView { Style = (Style)Resources["MediumSpacer"] }, row: 2);
        gridLayout.Add(usernameEntry, row: 3);
        gridLayout.Add(usernameEntryValidation, row: 4);
        gridLayout.Add(new BoxView { Style = (Style)Resources["MediumSpacer"] }, row: 5);
        gridLayout.Add(passwordEntry, row: 6);
        gridLayout.Add(passwordEntryValidation, row: 7);
        gridLayout.Add(new BoxView { Style = (Style)Resources["MediumSpacer"] }, row: 8);
        gridLayout.Add(passwordConfirmationEntry, row: 9);
        gridLayout.Add(passwordConfirmationEntryValidation, row: 10);
        gridLayout.Add(new BoxView { Style = (Style)Resources["ExxxtraSpacer"] }, row: 11);
        gridLayout.Add(submitInProgressBtn, row: 12);
        gridLayout.Add(submitBtn, row: 13);

        Content = gridLayout;
    }

    private static void ViewModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        ArgumentNullException.ThrowIfNull(sender);
        if (e.PropertyName != "SubmissionStatus")
            return;

        var viewModel = (SignUpViewModel)sender;
        var submissionStatus = viewModel.SubmissionStatus;

        if (submissionStatus == SubmissionStatus.Success)
            viewModel.SignUpSuccessCommand.Execute(null);

        if (submissionStatus == SubmissionStatus.Error)
            GenericErrorSnackBar.MakeSnackBar();
    }
}

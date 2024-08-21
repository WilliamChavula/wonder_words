using System.ComponentModel;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using ControlsLibrary;
using ControlsLibrary.Resources.Styles;
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
        Resources.MergedDictionaries.Add(new Styles());
        BindingContext = viewModel;

        viewModel.PropertyChanged += OnViewModelPropertyChanged;

        #region PasswordField

        var passwordField = new TextField
        {
            IsPassword = true,
            BorderColor = (Color)Resources["Gray600"],
            Title = L10n.passwordTextFieldLabel,
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
            }
        };

        var passwordError = new Label
        {
            TextColor = (Color)Resources["OnErrorDark"],
            FontSize = 14
        };
        passwordError.SetBinding(
            Label.TextProperty,
            new Binding
            {
                Source = this,
                Path = "BindingContext.PasswordError",
                Converter = new PasswordValidationTextConverter()
            }
        );

        passwordField.SetBinding(
            TextField.IsEnabledProperty,
            new Binding
            {
                Converter = new SubmissionInProgressInvertedConverter(),
                Path = "BindingContext.SubmissionStatus",
                Source = this
            }
        );

        var passwordTextChanged = new EventToCommandBehavior { EventName = "TextChanged", };
        passwordTextChanged.SetBinding(
            EventToCommandBehavior.CommandProperty,
            new Binding { Source = this, Path = "BindingContext.PasswordChangedCommand" }
        );
        passwordTextChanged.SetBinding(
            EventToCommandBehavior.CommandParameterProperty,
            new Binding { Source = passwordField, Path = "Text" }
        );

        passwordField.Behaviors.Add(passwordTextChanged);

        var passwordUnfocused = new EventToCommandBehavior { EventName = "Unfocused" };

        passwordUnfocused.SetBinding(
            EventToCommandBehavior.CommandProperty,
            new Binding { Source = this, Path = "BindingContext.PasswordUnfocusedCommand" }
        );
        passwordField.Behaviors.Add(passwordUnfocused);

        var editingCompleted = new EventToCommandBehavior { EventName = "Completed", };
        editingCompleted.SetBinding(
            EventToCommandBehavior.CommandProperty,
            new Binding { Source = this, Path = "BindingContext.SubmitCommand" }
        );

        passwordField.Behaviors.Add(editingCompleted);

        #endregion


        #region EmailField

        var emailField = new TextField
        {
            BorderColor = (Color)Resources["Gray600"],
            ReturnType = ReturnType.Next,
            ReturnCommand = new Command(() => passwordField.Focus()),
            IsTextPredictionEnabled = false,
            Keyboard = Keyboard.Email,
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
            Title = L10n.emailTextFieldLabel,
        };
        emailField.Focus();

        var emailFieldError = new Label
        {
            TextColor = (Color)Resources["OnErrorDark"],
            FontSize = 14
        };
        emailFieldError.SetBinding(
            Label.TextProperty,
            new Binding
            {
                Source = this,
                Path = "BindingContext.EmailError",
                Converter = new EmailValidationTextConverter()
            }
        );

        var emailTextChanged = new EventToCommandBehavior
        {
            EventName = "TextChanged",
            CommandParameter = emailField.Text
        };
        emailTextChanged.SetBinding(
            EventToCommandBehavior.CommandProperty,
            new Binding { Source = this, Path = "BindingContext.EmailChangedCommand" }
        );
        emailTextChanged.SetBinding(
            EventToCommandBehavior.CommandParameterProperty,
            new Binding { Source = emailField, Path = "Text" }
        );

        emailField.Behaviors.Add(emailTextChanged);

        var emailUnfocused = new EventToCommandBehavior { EventName = "Unfocused" };

        emailUnfocused.SetBinding(
            EventToCommandBehavior.CommandProperty,
            new Binding { Source = this, Path = "BindingContext.EmailUnfocusedCommand" }
        );
        emailField.Behaviors.Add(emailUnfocused);

        emailField.SetBinding(
            TextField.IsEnabledProperty,
            new Binding
            {
                Converter = new SubmissionInProgressInvertedConverter(),
                Path = "BindingContext.SubmissionStatus",
                Source = this
            }
        );

        #endregion

        #region Buttons

        var forgotPasswordBtn = new Button
        {
            Style = (Style)Resources["textButton"],
            Text = L10n.forgotMyPasswordButtonLabel,
            TextColor = (Color)Resources["Gray900"]
        };
        forgotPasswordBtn.SetBinding(
            Button.CommandProperty,
            new Binding { Source = this, Path = "BindingContext.ForgotPasswordTapCommand" }
        );
        forgotPasswordBtn.SetBinding(
            Button.IsEnabledProperty,
            new Binding
            {
                Source = this,
                Path = "BindingContext.SubmissionStatus",
                Converter = new SubmissionInProgressInvertedConverter()
            }
        );

        var signUpBtn = new Button
        {
            Style = (Style)Resources["textButton"],
            TextColor = (Color)Resources["Gray900"],
            Text = L10n.signUpButtonLabel
        };

        signUpBtn.SetBinding(
            Button.CommandProperty,
            new Binding { Source = this, Path = "BindingContext.SignUpTapCommand" }
        );
        signUpBtn.SetBinding(
            IsEnabledProperty,
            new Binding
            {
                Source = this,
                Path = "BindingContext.SubmissionStatus",
                Converter = new SubmissionInProgressInvertedConverter()
            }
        );

        var signInInProgress = new InProgressExpandedElevatedButton
        {
            TextLabel = L10n.signUpButtonLabel
        };
        signInInProgress.SetBinding(
            IsVisibleProperty,
            new Binding
            {
                Source = this,
                Path = "BindingContext.SubmissionStatus",
                Converter = new SubmissionInProgressConverter()
            }
        );

        var signInBtn = new ExpandedElevatedButton
        {
            TextLabel = L10n.signInButtonLabel,
            Icon = MaterialRounded.Login
        };

        signInBtn.SetBinding(
            ExpandedElevatedButton.OnTapProperty,
            new Binding { Source = this, Path = "BindingContext.SubmitCommand" }
        );

        signInBtn.SetBinding(
            IsVisibleProperty,
            new Binding
            {
                Source = this,
                Path = "BindingContext.SubmissionStatus",
                Converter = new SubmissionInProgressInvertedConverter()
            }
        );

        #endregion

        // 12
        var formControl = new Grid
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
            ]
        };

        formControl.Add(emailField, row: 0);
        formControl.Add(emailFieldError, row: 1);

        formControl.Add(
            new BoxView
            {
                HeightRequest = (double)Resources["LargeSpacing"],
                BackgroundColor = Colors.Transparent
            },
            row: 2
        );
        formControl.Add(passwordField, row: 3);
        formControl.Add(passwordError, row: 4);
        formControl.Add(forgotPasswordBtn, row: 5);
        formControl.Add(
            new BoxView
            {
                HeightRequest = (double)Resources["SmallSpacing"],
                BackgroundColor = Colors.Transparent
            },
            row: 6
        );

        formControl.Add(signInBtn, row: 7);
        formControl.Add(signInInProgress, row: 8);
        formControl.Add(
            new BoxView
            {
                HeightRequest = (double)Resources["XxLargeSpacing"],
                BackgroundColor = Colors.Transparent
            },
            row: 9
        );

        formControl.Add(
            new Label
            {
                Text = L10n.signUpOpeningText,
                TextColor = (Color)Resources["Gray500"],
                HorizontalTextAlignment = TextAlignment.Center
            },
            row: 10
        );
        formControl.Add(signUpBtn, row: 11);

        Content = formControl;
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        ArgumentNullException.ThrowIfNull(sender);

        var vm = (SignInViewModel)sender;

        if (e.PropertyName != nameof(vm.SubmissionStatus))
            return;

        var status = vm.SubmissionStatus;

        switch (status)
        {
            case SubmissionStatus.InvalidCredentialsError:
                var snackbarOptions = new SnackbarOptions
                {
                    CornerRadius = new CornerRadius(10),
                    Font = Microsoft.Maui.Font.SystemFontOfSize(14),
                };

                var text = L10n.invalidCredentialsErrorMessage;
                var duration = TimeSpan.FromSeconds(8);

                var snackbar = Snackbar.Make(
                    text,
                    duration: duration,
                    visualOptions: snackbarOptions
                );
                snackbar.Dismiss();
                snackbar.Show();
                break;
            case SubmissionStatus.GenericError:
                GenericErrorSnackBar.MakeSnackBar();
                break;
            default:
                return;
        }
    }
}

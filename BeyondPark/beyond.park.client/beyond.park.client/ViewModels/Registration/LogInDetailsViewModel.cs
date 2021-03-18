using beyond.park.client.Extensions;
using beyond.park.client.Factories.Validation;
using beyond.park.client.Models.Arguments.Registration;
using beyond.park.client.Models.Registration;
using beyond.park.client.Models.Rest.Identity;
using beyond.park.client.Models.Validations;
using beyond.park.client.Models.Validations.ValidationRules;
using beyond.park.client.Services.Identity;
using beyond.park.client.ViewModels.Base;
using beyond.park.client.ViewModels.Popups;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace beyond.park.client.ViewModels.Registration {
    public sealed class LogInDetailsViewModel : ContentPageBaseViewModel {

        private readonly IIdentityService _identityService;

        bool _isChecked;
        public bool IsChecked {
            get => _isChecked;
            set => SetProperty(ref _isChecked, value);
        }

        ValidatableObject<string> _name;
        public ValidatableObject<string> Name {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        ValidatableObject<string> _email;
        public ValidatableObject<string> Email {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        ValidatableObject<string> _password;
        public ValidatableObject<string> Password {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        AccountCreationPopupViewModel _accountCreationPopupViewModel;
        public AccountCreationPopupViewModel AccountCreationPopupViewModel {
            get => _accountCreationPopupViewModel;
            set {
                _accountCreationPopupViewModel?.Dispose();
                SetProperty(ref _accountCreationPopupViewModel, value);
            }
        }

        public ICommand CloseCommand => new Command(async () => await NavigationService.FirstInitilizeAsync<WelcomeViewModel>());

        public ICommand ContinueCommand => new Command(async () => await OnContinue());

        public ICommand CheckedCommand => new Command(() => OnCheckedCommand());

        public ICommand ValidateNameCommand => new Command(() => ValidateName());

        public ICommand ValidateEmailCommand => new Command(() => ValidateEmail());

        public ICommand ValidatePasswordCommand => new Command(() => ValidatePassword());

        /// <summary>
        ///     ctor().
        /// </summary>
        public LogInDetailsViewModel(IValidationObjectFactory validationObjectFactory, IIdentityService identityService) {
            _identityService = identityService;

            Name = validationObjectFactory.GetValidatableObject<string>();
            Email = validationObjectFactory.GetValidatableObject<string>();
            Password = validationObjectFactory.GetValidatableObject<string>();

            AccountCreationPopupViewModel = DependencyLocator.Resolve<AccountCreationPopupViewModel>();
            AccountCreationPopupViewModel.InitializeAsync(this);

#if DEBUG
            Name.Value = "Tod";
            Email.Value = "tod@mailinator.com";
            Password.Value = "Zxc12345";
            IsChecked = true;
#endif

            AddValidations();
        }

        public override Task InitializeAsync(object navigationData) {
            AccountCreationPopupViewModel?.InitializeAsync(navigationData);



            return base.InitializeAsync(navigationData);
        }

        private void OnCheckedCommand() {
            IsChecked = !_isChecked;
        }

        private async Task OnContinue() {
            if (Validate()) {
                if (IsChecked) {
                    IsBusy = true;

                    try {
                        RegisterDataContract registerDataContract = new RegisterDataContract {
                            Username = Email.Value,
                            Password = Password.Value
                        };

                        //var result = await _identityService.RegisterAsync(registerDataContract);
                        //if (result != null) {
                        //    ConfirmationArgs confirmationArgs = new ConfirmationArgs { Callback = ConfirmationCallback };
                        //    AccountCreationPopupViewModel.ShowPopupCommand.Execute(null);
                        //    await AccountCreationPopupViewModel.InitializeAsync(confirmationArgs);
                        //}

                        ConfirmationArgs confirmationArgs = new ConfirmationArgs { Callback = ConfirmationCallback };
                        AccountCreationPopupViewModel.ShowPopupCommand.Execute(null);
                        await AccountCreationPopupViewModel.InitializeAsync(confirmationArgs);
                    } catch (Exception ex) {
                        SetExceptionWithPopup(ex).IgnoreAwait();
                        Crashes.TrackError(ex);
                        Debugger.Break();
                    } finally {
                        IsBusy = false;
                    }
                } else {
                    await DialogService.ShowAlertAsync("Please confirm privacy policy!", "Alert", "Ok");
                }
            }
        }

        private async void ConfirmationCallback(Confirmation confirmation) {
            if (confirmation != null && !string.IsNullOrEmpty(confirmation.Code)) {
                AccountCreationPopupViewModel.ClosePopupCommand.Execute(null);
                await NavigationService.NavigateToAsync<VehicleDetailsViewModel>();
            } else {
                return;
            }
        }

        private bool Validate() {
            bool isValidName = ValidateName();
            bool isValidEmail = ValidateEmail();
            bool isValidPassword = ValidatePassword();

            return isValidName && isValidEmail && isValidPassword;
        }

        private bool ValidateName() => _name.Validate();

        private bool ValidateEmail() => _email.Validate();

        private bool ValidatePassword() => _password.Validate();

        private void AddValidations() {
            _name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "A name is required." });
            _email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "A email is required." });
            _email.Validations.Add(new EmailRule<string> { ValidationMessage = "A email is invalid." });
            _password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "A password is required." });
            _password.Validations.Add(new PasswordRule<string> { ValidationMessage = "Minimum 6 symbols and 3 digits required." });
        }
    }
}

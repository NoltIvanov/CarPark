using beyond.park.client.Extensions;
using beyond.park.client.Factories.Validation;
using beyond.park.client.Models.Arguments.Registration;
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
    public sealed class SignInViewModel : ContentPageBaseViewModel {

        private readonly IIdentityService _identityService;

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

        WelcomeBackPopupViewModel _welcomeBackPopupViewModel;
        public WelcomeBackPopupViewModel WelcomeBackPopupViewModel {
            get => _welcomeBackPopupViewModel;
            set {
                _welcomeBackPopupViewModel?.Dispose();
                SetProperty(ref _welcomeBackPopupViewModel, value);
            }
        }

        public ICommand CloseCommand => new Command(async () => await NavigationService.FirstInitilizeAsync<WelcomeViewModel>());

        public ICommand ContinueCommand => new Command(async () => await OnContinue());

        public ICommand RegisterCommand => new Command(async () => await OnRegister());

        /// <summary>
        ///     ctor().
        /// </summary>
        public SignInViewModel(IValidationObjectFactory validationObjectFactory, IIdentityService identityService) {
            _identityService = identityService;

            Email = validationObjectFactory.GetValidatableObject<string>();
            Password = validationObjectFactory.GetValidatableObject<string>();

            WelcomeBackPopupViewModel = DependencyLocator.Resolve<WelcomeBackPopupViewModel>();
            WelcomeBackPopupViewModel.InitializeAsync(this);

            AddValidations();
        }

        public override Task InitializeAsync(object navigationData) {
            WelcomeBackPopupViewModel?.InitializeAsync(navigationData);

            return base.InitializeAsync(navigationData);
        }

        private async Task OnContinue() {
            if (Validate()) {
                try {
                    SignInDataContract signInDataContract = new SignInDataContract() {
                        Username = Email.Value,
                        Password = Password.Value
                    };
                    //var res = await _identityService.SignInAsync(signInDataContract);
                    //if (res != null) {
                    //    BaseSingleton<GlobalSetting>.Instance.UserProfile.IsAuth = true;
                    //    BaseSingleton<GlobalSetting>.Instance.UserProfile.SaveChanges();

                    //    WelcomeBackArgs welcomeBackArgs = new WelcomeBackArgs {
                    //        Name = "Arnold",
                    //        Callback = AcceptCallback
                    //    };

                    //    WelcomeBackPopupViewModel.ShowPopupCommand.Execute(null);
                    //    await WelcomeBackPopupViewModel.InitializeAsync(welcomeBackArgs);
                    //}
                    BaseSingleton<GlobalSetting>.Instance.UserProfile.IsAuth = true;
                    BaseSingleton<GlobalSetting>.Instance.UserProfile.SaveChanges();

                    WelcomeBackArgs welcomeBackArgs = new WelcomeBackArgs {
                        Name = "Roland",
                        Callback = AcceptCallback
                    };

                    WelcomeBackPopupViewModel.ShowPopupCommand.Execute(null);
                    await WelcomeBackPopupViewModel.InitializeAsync(welcomeBackArgs);
                } catch (Exception ex) {
                    SetExceptionWithPopup(ex).IgnoreAwait();
                    Crashes.TrackError(ex);
                    Debugger.Break();
                } finally {
                    IsBusy = false;
                }
            }
        }

        private bool Validate() {
            bool isValidEmail = ValidateEmail();
            bool isValidPassword = ValidatePassword();

            return isValidEmail && isValidPassword;
        }

        private bool ValidateEmail() => _email.Validate();

        private bool ValidatePassword() => _password.Validate();

        private async Task OnRegister() {
            await NavigationService.NavigateToAsync<LogInDetailsViewModel>();
        }

        private async void AcceptCallback() {
            await NavigationService.RemoveBackStackAsync();
            await NavigationService.FirstInitilizeAsync<MainViewModel>();
        }

        private void AddValidations() {
            _email.Validations.Add(new EmailRule<string> { ValidationMessage = "A email is invalid." });
        }
    }
}

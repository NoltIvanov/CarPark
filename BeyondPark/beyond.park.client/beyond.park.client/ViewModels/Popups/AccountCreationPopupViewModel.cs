using beyond.park.client.Extensions;
using beyond.park.client.Models.Arguments.Registration;
using beyond.park.client.Models.Registration;
using beyond.park.client.Services.OpenUrl;
using beyond.park.client.Services.Platform.Contracts;
using beyond.park.client.ViewModels.Base;
using beyond.park.client.Views.Popups;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace beyond.park.client.ViewModels.Popups {
    public sealed class AccountCreationPopupViewModel : PopupBaseViewModel {

        string _firstDigit;
        public string FirstDigit {
            get => _firstDigit;
            set => SetProperty(ref _firstDigit, value);
        }

        string _secondDigit;
        public string SecondDigit {
            get => _secondDigit;
            set => SetProperty(ref _secondDigit, value);
        }

        string _thirdDigit;
        public string ThirdDigit {
            get => _thirdDigit;
            set => SetProperty(ref _thirdDigit, value);
        }

        string _fourthDigit;
        public string FourthDigit {
            get => _fourthDigit;
            set => SetProperty(ref _fourthDigit, value);
        }

        public Action<Confirmation> Callback { get; internal set; }

        public override Type RelativeViewType => typeof(AccountCreationPopupView);

        public ICommand ConfirmCodeCommand => new Command(async () => await OnConfirmCode());

        public ICommand OpenEmailCommand => new Command(() => OnOpenEmail());

        public AccountCreationPopupViewModel() {

        }

        public override Task InitializeAsync(object navigationData) {
            if (navigationData is ConfirmationArgs args) {
                Callback = args.Callback;
            }

            return base.InitializeAsync(navigationData);
        }

        private void OnOpenEmail() {
            try {               
                DependencyService.Get<IEmailService>().OpenInbox();
            } catch (Exception ex) {
                SetExceptionWithPopup(ex).IgnoreAwait();
                Crashes.TrackError(ex);
                Debugger.Break();
            }
        }

        private async Task OnConfirmCode() {
            IsBusy = true;

            await Task.Delay(2000);
           
            Callback?.Invoke(new Confirmation { Code = ComposeCode() });

            Clear();

            IsBusy = false;
        }

        private string ComposeCode() {
            string result = string.Empty;

            result += FirstDigit;
            result += SecondDigit;
            result += ThirdDigit;
            result += FourthDigit;

            return result;
        }

        private void Clear() {
            FirstDigit = string.Empty;
            SecondDigit = string.Empty;
            ThirdDigit = string.Empty;
            FourthDigit = string.Empty;
        }
    }
}

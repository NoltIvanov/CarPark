using beyond.park.client.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace beyond.park.client.ViewModels.Registration {
    public sealed class VehicleDetailsViewModel : ContentPageBaseViewModel {

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

        string _fifthDigit;
        public string FifthDigit {
            get => _fifthDigit;
            set => SetProperty(ref _fifthDigit, value);
        }

        string _sixthDigit;
        public string SixthDigit {
            get => _sixthDigit;
            set => SetProperty(ref _sixthDigit, value);
        }

        public ICommand ContinueCommand => new Command(async () => await OnContinue());

        public ICommand CloseCommand => new Command(async () => await NavigationService.FirstInitilizeAsync<WelcomeViewModel>());

        /// <summary>
        ///     ctor().
        /// </summary>
        public VehicleDetailsViewModel() {

        }

        private async Task OnContinue() {
            await NavigationService.NavigateToAsync<PaymentDetailsViewModel>();
        }
    }
}

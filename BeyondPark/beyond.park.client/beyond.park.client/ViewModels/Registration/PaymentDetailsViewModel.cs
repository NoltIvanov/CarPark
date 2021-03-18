using beyond.park.client.Factories.Validation;
using beyond.park.client.Models.Validations;
using beyond.park.client.ViewModels.Base;
using Plugin.PayCards;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace beyond.park.client.ViewModels.Registration {
    public sealed class PaymentDetailsViewModel : ContentPageBaseViewModel {

        ValidatableObject<string> _holderName;
        public ValidatableObject<string> HolderName {
            get => _holderName;
            set => SetProperty(ref _holderName, value);
        }

        ValidatableObject<string> _cardNumber;
        public ValidatableObject<string> CardNumber {
            get => _cardNumber;
            set => SetProperty(ref _cardNumber, value);
        }

        ValidatableObject<string> _expirationDate;
        public ValidatableObject<string> ExpirationDate {
            get => _expirationDate;
            set => SetProperty(ref _expirationDate, value);
        }

        bool _isManually;
        public bool IsManually { 
            get => _isManually;
            set => SetProperty(ref _isManually, value);
        }

        public ICommand CloseCommand => new Command(async () => await NavigationService.FirstInitilizeAsync<WelcomeViewModel>());

        public ICommand ContinueCommand => new Command(async () => await OnContinue());

        public ICommand ManuallyCommand => new Command(() => OnManually());

        public ICommand ScanCommand => new Command(() => OnScan());

        /// <summary>
        ///     ctor().
        /// </summary>
        public PaymentDetailsViewModel(IValidationObjectFactory validationObjectFactory) {
            HolderName = validationObjectFactory.GetValidatableObject<string>();
            CardNumber = validationObjectFactory.GetValidatableObject<string>();
            ExpirationDate = validationObjectFactory.GetValidatableObject<string>();
        }

        private async void OnScan() {
            var cardInfo = await CrossPayCards.Current.ScanAsync();

            if (cardInfo != null) {
                OnManually();

                HolderName.Value = cardInfo.HolderName;                
                FillInputStepByStep(cardInfo.CardNumber);
                ExpirationDate.Value = cardInfo.ExpirationDate;

                Debug.WriteLine($"Result: {cardInfo.HolderName}\n{cardInfo.CardNumber}\n{cardInfo.ExpirationDate}");
            }
        }

        private async Task OnContinue() {
            BaseSingleton<GlobalSetting>.Instance.UserProfile.IsAuth = true;
            BaseSingleton<GlobalSetting>.Instance.UserProfile.SaveChanges();
           
            await NavigationService.NavigateToAsync<AccountCreatedViewModel>();
        }

        // This method using only for credit card mask input.
        private void FillInputStepByStep(string holderName) {
            for (int i = 0; i < holderName.Length; i++) {
                CardNumber.Value += holderName[i].ToString();
            }
        }

        private void OnManually() {
            IsManually = !IsManually;
        }
    }
}

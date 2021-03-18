using beyond.park.client.ViewModels.Base;
using beyond.park.client.Views.Popups;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace beyond.park.client.ViewModels.Popups {
    public sealed class NotSignUpPopupViewModel : PopupBaseViewModel {

        bool _mainContentShown = true;
        public bool MainContentShown {
            get => _mainContentShown;
            set => SetProperty(ref _mainContentShown, value);
        }

        bool _secondaryContentShown;
        public bool SecondaryContentShown {
            get => _secondaryContentShown;
            set => SetProperty(ref _secondaryContentShown, value);
        }

        public override Type RelativeViewType => typeof(NotSignUpPopupView);

        public ICommand NextCommand => new Command(() => OnNext());

        /// <summary>
        ///     ctor().
        /// </summary>
        public NotSignUpPopupViewModel() {

        }

        public override Task InitializeAsync(object navigationData) {

            SetDefaultState();

            return base.InitializeAsync(navigationData);
        }

        private void SetDefaultState() {
            MainContentShown = true;
            SecondaryContentShown = false;
        }

        private void OnNext() {
            SecondaryContentShown = true;
            MainContentShown = false;
        }
    }
}

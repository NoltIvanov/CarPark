using beyond.park.client.Extensions;
using beyond.park.client.Models.Arguments.Registration;
using beyond.park.client.ViewModels.Base;
using beyond.park.client.Views.Popups;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace beyond.park.client.ViewModels.Popups {
    public sealed class WelcomeBackPopupViewModel : PopupBaseViewModel {

        public override Type RelativeViewType => typeof(WelcomeBackPopupView);

        string _name;
        public string Name {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        bool _stateChanged;
        public bool StateChanged {
            get => _stateChanged;
            set => SetProperty(ref _stateChanged, value);
        }

        public Action Callback { get; internal set; }

        public ICommand AcceptCommand => new Command(() => OnAccept());

        /// <summary>
        ///     ctor().
        /// </summary>
        public WelcomeBackPopupViewModel() {

        }

        public override Task InitializeAsync(object navigationData) {
            if (navigationData is WelcomeBackArgs args) {
                Callback = args.Callback;
                Name = args.Name;

                StartAnimate().IgnoreAwait();
            }

            return base.InitializeAsync(navigationData);
        }

        private async Task StartAnimate() {
            await Task.Delay(1000);
            StateChanged = true;
            await Task.Delay(1000);

            OnAccept();
        }

        private void OnAccept() {
            Callback?.Invoke();
        }
    }
}

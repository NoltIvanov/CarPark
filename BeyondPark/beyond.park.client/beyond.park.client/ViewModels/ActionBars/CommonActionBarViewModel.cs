using beyond.park.client.Models.EventMessages;
using beyond.park.client.ViewModels.Base;
using PubSub;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace beyond.park.client.ViewModels.ActionBars {
    public sealed class CommonActionBarViewModel : ActionBarBaseViewModel {

        bool _isBackButtonAvailable;
        public bool IsBackButtonAvailable {
            get { return _isBackButtonAvailable; }
            private set { SetProperty(ref _isBackButtonAvailable, value); }
        }

        public ICommand OpemMenuCommand => new Command(() => OnOpemMenu());

        private void OnOpemMenu() {
            if (BaseSingleton<GlobalSetting>.Instance.UserProfile.IsAuth) {
                Hub.Default.Publish(new MenuMessage());
            } else {
                Hub.Default.Publish(new NotSignUpMessage());
            }
        }

        public ICommand HelpCommand => new Command(() => Hub.Default.Publish<HelpSystemMessage>(new HelpSystemMessage()));

        public override Task InitializeAsync(object navigationData) {

            IsBackButtonAvailable = NavigationService.IsBackButtonAvailable;

            return base.InitializeAsync(navigationData);
        }
    }
}

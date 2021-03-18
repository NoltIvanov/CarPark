using beyond.park.client.ViewModels.Base;
using beyond.park.client.ViewModels.Registration;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace beyond.park.client.ViewModels {
    public sealed  class WelcomeViewModel : ContentPageBaseViewModel {
      
        public ICommand SignInCommand => new Command(async () => await OnSignIn());

        public ICommand OpenGuideCommand => new Command(async () => await OnOpenGuide());

        public ICommand UnregisterInCommand => new Command(async () => await OnUnregisterIn());
        
        /// <summary>
        ///     ctor().
        /// </summary>
        public WelcomeViewModel() {
        }

        private async Task OnOpenGuide() {
            await NavigationService.RemoveBackStackAsync();
            await NavigationService.FirstInitilizeAsync<GuideViewModel>();
        }

        private async Task OnSignIn() {
            await NavigationService.RemoveBackStackAsync();
            await NavigationService.FirstInitilizeAsync<SignInViewModel>();
        }

        private async Task OnUnregisterIn() {
            await NavigationService.RemoveBackStackAsync();
            await NavigationService.FirstInitilizeAsync<MainViewModel>();
        }
    }
}

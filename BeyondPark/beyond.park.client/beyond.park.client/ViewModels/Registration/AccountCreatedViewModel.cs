using beyond.park.client.ViewModels.Base;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace beyond.park.client.ViewModels.Registration {
    public sealed class AccountCreatedViewModel : ContentPageBaseViewModel {

        public ICommand ContinueCommand => new Command(async () => await OnContinue());

        public ICommand CloseCommand => new Command(async () => await NavigationService.FirstInitilizeAsync<WelcomeViewModel>());

        /// <summary>
        ///     ctor().
        /// </summary>
        public AccountCreatedViewModel() {

        }

        private async Task OnContinue() {      
            await NavigationService.RemoveBackStackAsync();
            await NavigationService.FirstInitilizeAsync<MainViewModel>();
        }
    }
}

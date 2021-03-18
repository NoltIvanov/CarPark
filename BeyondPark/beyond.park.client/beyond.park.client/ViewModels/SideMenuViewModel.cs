using beyond.park.client.Models.Identities;
using beyond.park.client.Services.Identity;
using beyond.park.client.ViewModels.Base;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace beyond.park.client.ViewModels {
    public sealed class SideMenuViewModel : ViewModelBase {

        private readonly IIdentityService _identityService;

        //public ICommand UserManagementCommand => new Command(async () => {
        //    await NavigationService.RemoveBackStackAsync();
        //    await NavigationService.FirstInitilizeAsync<UserManagementViewModel>();
        //});

        string _supportTitle = "Support & Feedback";
        public string SupportTitle {
            get => _supportTitle;
            set => SetProperty(ref _supportTitle, value);
        }

        string _privacyTitle = "Privacy & Safety";
        public string PrivacyTitle {
            get => _privacyTitle;
            set => SetProperty(ref _privacyTitle, value);
        }

        UserProfile _userProfile;
        public UserProfile UserProfile {
            get { return _userProfile; }
            set => SetProperty(ref _userProfile, value);
        }

        //public ICommand LogOutCommand => new Command(async () => await _identityService.LogOutAsync());

        public ICommand LogOutCommand => new Command(async () => await OnLogOut());

        public SideMenuViewModel(IIdentityService identityService) {
            _identityService = identityService;

            UserProfile = BaseSingleton<GlobalSetting>.Instance.UserProfile;
        }

        private async Task OnLogOut() {
            try {
                BaseSingleton<GlobalSetting>.Instance.UserProfile.IsAuth = false;
                BaseSingleton<GlobalSetting>.Instance.UserProfile.SaveChanges();

                await NavigationService.RemoveBackStackAsync();
                await NavigationService.FirstInitilizeAsync<WelcomeViewModel>();

            } catch (Exception ex) {
                Debug.WriteLine($"ERROR: {ex.Message}");
                Debugger.Break();

                Crashes.TrackError(ex);
            }
        }
    }
}

using beyond.park.client.Common;
using beyond.park.client.Services.Navigation;
using beyond.park.client.ViewModels.Base;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace beyond.park.client {
    public partial class App : Application {
        public App() {
            InitializeComponent();

            InitApp();
        }

        protected override async void OnStart() {        
            await InitNavigation();
        }

        protected override void OnSleep() {
            DependencyLocator.Resolve<INavigationService>().UnsubscribeAfterSleepApp();
        }

        protected override void OnResume() {
            DependencyLocator.Resolve<INavigationService>().InitAfterResumeApp();
        }

        private void InitApp() {
            ConfigureAppCenter();

            DependencyLocator.RegisterDependencies();
        }

        private Task InitNavigation() {
            INavigationService navigationService = DependencyLocator.Resolve<INavigationService>();
            return navigationService.InitializeAsync();
        }

        private static void ConfigureAppCenter() {
            AppCenter.Start($"ios={ApplicationConsts.APP_CENTER_IOS_KEY}" + $"android={ApplicationConsts.APP_CENTER_ANDROID_KEY}",
                              typeof(Analytics), typeof(Crashes));
        }
    }
}

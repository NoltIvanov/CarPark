using beyond.park.client.Common;
using beyond.park.client.Helpers;
using beyond.park.client.Models.Identities;
using beyond.park.client.Models.LifeCycle;
using beyond.park.client.ViewModels;
using beyond.park.client.ViewModels.Base;
using beyond.park.client.Views.Base;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace beyond.park.client.Services.Navigation {
    public class NavigationService : INavigationService {

        public bool IsBackButtonAvailable {
            get {
                if (Application.Current.MainPage is CustomNavigationView mainPage) {
                    Page rootPage = mainPage.RootPage;
                    if (rootPage != null) {
                        return true;
                    }
                }
                return false;
            }
        }

        public ViewModelBase PreviousPageViewModel {
            get {
                try {
                    var mainPage = Application.Current.MainPage as CustomNavigationView;
                    var viewModel = mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2].BindingContext;
                    return viewModel as ViewModelBase;
                } catch (Exception ex) {
                    Crashes.TrackError(ex);
                    Debugger.Break();
                    return null;
                }
            }
        }

        public ViewModelBase LastPageViewModel {
            get {
                if (Application.Current.MainPage is CustomNavigationView) {
                    return ((CustomNavigationView)Application.Current.MainPage).Navigation.NavigationStack.LastOrDefault().BindingContext as ViewModelBase;
                }

                return null;
            }
        }

        public IReadOnlyCollection<ViewModelBase> CurrentViewModelsNavigationStack {
            get {
                CustomNavigationView customNavigationView = Application.Current.MainPage as CustomNavigationView;

                IReadOnlyCollection<ViewModelBase> readOnlyResult =
                    customNavigationView.Navigation.NavigationStack
                    .Select<Page, ViewModelBase>(p => (ViewModelBase)p.BindingContext)
                    .ToList<ViewModelBase>()
                    .AsReadOnly();

                return readOnlyResult;
            }
        }

        public Task InitializeAsync(object parameter = default(object)) {

            string serializedUserProfile = Preferences.Get(ApplicationConsts.USER_PROFILE, string.Empty);
            UserProfile userProfile = JsonSerializerHelper.Deserialize<UserProfile>(serializedUserProfile);

            ClearResources();

            if (userProfile != null && userProfile.IsAuth) {
                return FirstInitilizeAsync<MainViewModel>(parameter);
            } else {
                return FirstInitilizeAsync<WelcomeViewModel>(parameter);
            }
        }

        private static void ClearResources() {
            if (Application.Current.MainPage is CustomNavigationView navigationPage) {
                List<Page> stack = navigationPage.Navigation.NavigationStack.ToList();
                stack.ForEach(p => {
                    if (p.BindingContext is ViewModelBase viewModel) {
                        viewModel.Dispose();
                    }
                });
            }
        }

        public async Task FirstInitilizeAsync<TViewModel>(object parameter = default(object)) where TViewModel : ViewModelBase {
            Page page = CreatePage(typeof(TViewModel), null);
            Application.Current.MainPage = new CustomNavigationView(page);
            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase =>
             InternalNavigateToAsync(typeof(TViewModel), null);

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase =>
             InternalNavigateToAsync(typeof(TViewModel), parameter);
     
        private async Task InternalNavigateToAsync(Type viewModelType, object parameter) {
            if (Application.Current.MainPage is CustomNavigationView navigationPage) {
                List<Page> stack = navigationPage.Navigation.NavigationStack.ToList();
                Page targetPage = stack.FirstOrDefault(p => {
                    return p.BindingContext.GetType().FullName == viewModelType.FullName;
                });

                if (targetPage != null) {
                    int targetPageIndex = stack.IndexOf(targetPage);
                    int stepsToForBackStack = 0;

                    for (int i = 0; i < stack.Count; i++) {
                        if (i > targetPageIndex) {
                            stepsToForBackStack++;
                        }
                    }

                    if (stepsToForBackStack > 1) {
                        stepsToForBackStack--;

                        for (int i = 1; i <= stepsToForBackStack; i++) {
                            await RemoveLastFromBackStackAsync();
                        }

                        await GoBackAsync();
                    } else if (stepsToForBackStack == 1) {
                        await GoBackAsync();
                    }

                    await ((ViewModelBase)targetPage.BindingContext).InitializeAsync(parameter);
                } else {
                    Page page = CreatePage(viewModelType, parameter);
                    await navigationPage.PushAsync(page, true);
                    await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
                }
            } else {
                Page page = CreatePage(viewModelType, parameter);
                Application.Current.MainPage = new CustomNavigationView(page);
                await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
            }
        }

        public Task RemoveLastFromBackStackAsync() {
            if (Application.Current.MainPage is CustomNavigationView mainPage) {
                Page pageToRemove = mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2];
                DisposeResources(pageToRemove);               
                mainPage.Navigation.RemovePage(pageToRemove);
            }

            return Task.FromResult(true);
        }

        public Task RemoveBackStackAsync() {
            if (Application.Current.MainPage is CustomNavigationView mainPage) {
                for (int i = 0; i < mainPage.Navigation.NavigationStack.Count - 1; i++) {
                    var page = mainPage.Navigation.NavigationStack[i];
                    DisposeResources(page);
                    mainPage.Navigation.RemovePage(page);
                }

                int rootIndex = 0;
                if (mainPage.Navigation.NavigationStack.Count - 1 == rootIndex) {
                    var rootPage = mainPage.Navigation.NavigationStack[rootIndex];                  
                    DisposeResources(rootPage);
                }
            }

            return Task.FromResult(true);
        }

        private void DisposeResources(Page page) {
            DisposeBindingContext(page);
            DisposePageSource(page);
        }

        public Task RemoveIntermediatePagesAsync() {
            if (Application.Current.MainPage is CustomNavigationView mainPage && mainPage.Navigation.NavigationStack.Count >= 3) {
                List<Page> pagesToRemove = new List<Page>();

                for (int i = 1; i <= mainPage.Navigation.NavigationStack.Count - 2; i++) {
                    pagesToRemove.Add(mainPage.Navigation.NavigationStack[i]);
                }

                pagesToRemove.ForEach(page => {
                    DisposeResources(page);                   
                    mainPage.Navigation.RemovePage(page);
                });
            }

            return Task.FromResult(true);
        }

        public async Task GoBackAsync() {
            try {
                var mainPage = Application.Current.MainPage as CustomNavigationView;
                DisposeResources(mainPage.CurrentPage);               

                await mainPage.PopAsync();
            } catch (Exception ex) {
                Crashes.TrackError(ex);
                Debugger.Break();
                throw;
            }
        }

        public void InitAfterResumeApp() {
            try {
                CustomNavigationView customNavigationView = Application.Current.MainPage as CustomNavigationView;
                customNavigationView.Navigation.NavigationStack
                .Select(p => (ViewModelBase)p.BindingContext)
                .ToList().ForEach(viewModel => viewModel.InitializeAsync(null));
            } catch (Exception ex) {
                Crashes.TrackError(ex);
                Debugger.Break();
            }
        }

        public void UnsubscribeAfterSleepApp() {
            try {
                CustomNavigationView customNavigationView = Application.Current.MainPage as CustomNavigationView;

                SleepAppArg sleepAppArg = new SleepAppArg();

                customNavigationView.Navigation.NavigationStack
                .Select<Page, ViewModelBase>(p => (ViewModelBase)p.BindingContext)
                .ToList<ViewModelBase>().ForEach(viewModel => viewModel.InitializeAsync(sleepAppArg));
            } catch (Exception ex) {
                Crashes.TrackError(ex);
                Debugger.Break();
            }
        }

        public async Task CloseModalAsync() {
            var mainPage = Application.Current.MainPage as CustomNavigationView;

            DisposeResources(mainPage.CurrentPage);           

            await mainPage.Navigation.PopModalAsync();
        }

        private void DisposePageSource(Page page) {
            if (page is IDisposable disposable) {
                disposable.Dispose();
            }
        }

        private void DisposeBindingContext(Page targetPage) {
            if (targetPage?.BindingContext is ViewModelBase viewModel) {
                viewModel.Dispose();
            }
        }

        private Page CreatePage(Type viewModelType, object parameter) {
            try {
                Type pageType = GetPageTypeForViewModel(viewModelType);
                if (pageType == null) {
                    throw new Exception($"Cannot locate page type for {viewModelType}");
                }

                return Activator.CreateInstance(pageType) as Page;
            } catch (Exception ex) {
                Crashes.TrackError(ex);
                Debugger.Break();
                throw;
            }
        }

        private Type GetPageTypeForViewModel(Type viewModelType) {
            var viewName = viewModelType.FullName.Replace("Model", string.Empty);
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
            var viewType = Type.GetType(viewAssemblyName);
            return viewType;
        }
    }
}

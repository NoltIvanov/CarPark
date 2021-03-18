using beyond.park.client.Helpers;
using beyond.park.client.Models.Exceptions;
using beyond.park.client.Models.Identities;
using beyond.park.client.Models.LifeCycle;
using beyond.park.client.Services.Dialog;
using beyond.park.client.Services.Navigation;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace beyond.park.client.ViewModels.Base {
    public abstract class ViewModelBase : ExtendedBindableObject {

        private CancellationTokenSource _refreshCancellationTokenSource = new CancellationTokenSource();

        protected readonly IDialogService DialogService;

        protected readonly INavigationService NavigationService;

        string _serverError;
        public string ServerError {
            get { return _serverError; }
            set { SetProperty(ref _serverError, value); }
        }

        //public ResourceLoader ResourceLoader { get; private set; }

        ICommand _refreshCommand;
        public ICommand RefreshCommand {
            get => _refreshCommand;
            protected set => SetProperty(ref _refreshCommand, value);
        }

        /// <summary>
        ///     ctor().
        /// </summary>
        public ViewModelBase() {
            DialogService = DependencyLocator.Resolve<IDialogService>();
            NavigationService = DependencyLocator.Resolve<INavigationService>();

            BackCommand = new Command(async (x) => {
                await NavigationService.PreviousPageViewModel?.InitializeAsync(x);
                await NavigationService.GoBackAsync();
            });

            CloseModalCommand = new Command(async () => {
                await NavigationService.LastPageViewModel?.InitializeAsync(null);
                await NavigationService.CloseModalAsync();
            });

            //ResourceLoader = new ResourceLoader();
            ResolveStringResources();
        }

        public ICommand BackCommand { get; protected set; }

        public ICommand CloseModalCommand { get; protected set; }

        public ICommand CleanServerErrorCommand => new Command(() => ServerError = string.Empty);

        public bool IsSubscribedOnAppEvents { get; private set; }

        bool _isBusy;
        public bool IsBusy {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public virtual Task InitializeAsync(object navigationData) {
            if (!IsSubscribedOnAppEvents) {
                OnSubscribeOnAppEvents();
            }

            if (navigationData is SleepAppArg) {
                OnUnsubscribeFromAppEvents();
            }

            return Task.FromResult(false);
        }

        public virtual void Dispose() {
            //ResourceLoader?.Dispose();
            ResetCancellationTokenSource(ref _refreshCancellationTokenSource);
            OnUnsubscribeFromAppEvents();
        }

        protected virtual void ResolveStringResources() { }

        protected void ResetCancellationTokenSource(ref CancellationTokenSource cancellationTokenSource) {
            cancellationTokenSource.Cancel();
            cancellationTokenSource = new CancellationTokenSource();
        }

        protected virtual void OnSubscribeOnAppEvents() {
            IsSubscribedOnAppEvents = true;
        }

        protected virtual void OnUnsubscribeFromAppEvents() {
            IsSubscribedOnAppEvents = false;
        }

        protected async Task SetExceptionWithPopup(Exception ex) {
            if (ex is HttpRequestExceptionEx apiException) {
                if (apiException.HttpCode == System.Net.HttpStatusCode.BadRequest) {

                    try {
                        var errorResponse = JsonSerializerHelper.Deserialize<HttpExceptionResponse<object>>(apiException.Message);
                        //ServerError = errorResponse.Message;
                        await DialogService.ShowAlertAsync(errorResponse.Message, "Error", "Ok");
                    } catch (JsonSerializationException jsonEx) {
                        await DialogService.ShowAlertAsync(jsonEx.Message, "Error", "Ok");
                    } catch (Exception e) {
                        await DialogService.ShowAlertAsync(e.Message, "Error", "Ok");
                    }
                }
            } else {
                await DialogService.ShowAlertAsync(ex.Message, "Error", "Ok");
            }
        }

        protected async void TryToRefreshAccessToken() {
            //ResetCancellationTokenSource(ref _refreshCancellationTokenSource);
            //CancellationTokenSource cancellationTokenSource = _refreshCancellationTokenSource;

            //IIdentityService identityService = null;
            //try {
            //    identityService = DependencyLocator.Resolve<IIdentityService>();
            //    await identityService.RefreshTokenAsync(cancellationTokenSource.Token);
            //} catch (Exception ex) {
            //    Crashes.TrackError(ex);
            //    Debugger.Break();
            //    await identityService?.LogOutAsync();
            //}
        }
    }
}

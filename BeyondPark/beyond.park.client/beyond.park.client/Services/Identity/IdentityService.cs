using beyond.park.client.Models.Exceptions;
using beyond.park.client.Models.Rest;
using beyond.park.client.Models.Rest.Enums;
using beyond.park.client.Models.Rest.Identity;
using beyond.park.client.Services.Navigation;
using beyond.park.client.Services.RequestProvider;
using beyond.park.client.ViewModels.Base;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace beyond.park.client.Services.Identity {
    public sealed class IdentityService : IIdentityService {

        private readonly IRequestProvider _requestProvider;

        private readonly INavigationService _navigationService;

        /// <summary>
        ///     ctor().
        /// </summary>
        public IdentityService(IRequestProvider requestProvider, INavigationService navigationService) {
            _requestProvider = requestProvider;
            _navigationService = navigationService;
        }

        public async Task<BaseResult<bool>> RegisterAsync(RegisterDataContract registerDataContract, CancellationToken cancellationToken = default) =>
            await Task.Run(async () => {
                BaseResult<bool> registerResult = null;

                string url = BaseSingleton<GlobalSetting>.Instance.RestEndpoints.IdentityEndpoints.RegisterEndPoint;

                try {
                    registerResult = await _requestProvider.PostAsync<BaseResult<bool>, RegisterDataContract>(url, registerDataContract);

                    if (registerResult != null) {
                        //await SetupTokens(registerResult);
                    }
                } catch (ConnectivityException ex) {
                    throw ex;
                } catch (HttpRequestExceptionEx ex) {
                    throw ex;
                } catch (Exception ex) {
                    Debug.WriteLine($"ERROR:{ex.Message}");
                    Crashes.TrackError(ex);
                    Debugger.Break();
                }
                return registerResult;
            }, cancellationToken);

        public async Task<BaseResult<object>> EmailConfirmationAsync(ConfirmationDataContract confirmationDataContract, CancellationToken cancellationToken = default) =>
            await Task.Run(async () => {
                BaseResult<object> confirmationResult = null;

                string url = BaseSingleton<GlobalSetting>.Instance.RestEndpoints.IdentityEndpoints.EmailConfirmationEndPoint;

                try {
                    confirmationResult = await _requestProvider.PostAsync<BaseResult<object>, ConfirmationDataContract>(url, confirmationDataContract);

                    if (confirmationResult != null && confirmationResult.StatusCode == BeyondParkStatusCodes.Success) {
                        return confirmationResult;
                    }
                } catch (ConnectivityException ex) {
                    throw ex;
                } catch (HttpRequestExceptionEx ex) {
                    throw ex;
                } catch (Exception ex) {
                    Debug.WriteLine($"ERROR:{ex.Message}");
                    Crashes.TrackError(ex);
                    Debugger.Break();
                }
                return confirmationResult;
            }, cancellationToken);

        public async Task<BaseResult<SignInBody>> SignInAsync(SignInDataContract signInDataContract, CancellationToken cancellationToken = default) =>
            await Task.Run(async () => {
                BaseResult<SignInBody> signInResult = null;

                string url = BaseSingleton<GlobalSetting>.Instance.RestEndpoints.IdentityEndpoints.SignInEndPoint;

                try {
                    signInResult = await _requestProvider.PostAsync<BaseResult<SignInBody>, SignInDataContract>(url, signInDataContract);

                    if (signInResult != null && signInResult.StatusCode == BeyondParkStatusCodes.Success) {
                        return signInResult;
                    }
                } catch (ConnectivityException ex) {
                    throw ex;
                } catch (HttpRequestExceptionEx ex) {
                    throw ex;
                } catch (Exception ex) {
                    Debug.WriteLine($"ERROR:{ex.Message}");
                    Crashes.TrackError(ex);
                    Debugger.Break();
                }
                return signInResult;
            }, cancellationToken);
    }
}

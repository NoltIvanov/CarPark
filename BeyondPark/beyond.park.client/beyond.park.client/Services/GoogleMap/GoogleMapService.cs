using beyond.park.client.Models.Exceptions;
using beyond.park.client.Models.Rest;
using beyond.park.client.Models.Rest.GoogleMap;
using beyond.park.client.Services.RequestProvider;
using beyond.park.client.ViewModels.Base;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace beyond.park.client.Services.GoogleMap {
    public sealed class GoogleMapService : IGoogleMapService {

        private readonly IRequestProvider _requestProvider;

        /// <summary>
        ///     ctor().
        /// </summary>
        /// <param name="requestProvider"></param>
        public GoogleMapService(IRequestProvider requestProvider) {
            _requestProvider = requestProvider;
        }

        public async Task<object> GetDirections(string originLatitude, string originLongitude, string destinationLatitude, string destinationLongitude, CancellationToken cancellationToken = default(CancellationToken)) =>
             await Task.Run(async () => {
                 BaseResult<object> registerResult = null;

                 string url = string.Format(BaseSingleton<GlobalSetting>.Instance.RestEndpoints.GoogleMapsEndpoints.GetDirectionsEndPoint, originLatitude, originLongitude, destinationLatitude, destinationLongitude);

                 try {
                     var result = await _requestProvider.GetAsync<GoogleDirection>(url);

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
    }
}

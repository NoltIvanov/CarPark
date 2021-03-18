using beyond.park.client.Common;
using beyond.park.client.Helpers;
using beyond.park.client.Models.Exceptions;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace beyond.park.client.Services.RequestProvider {
    public class RequestProvider : IRequestProvider {

        private readonly HttpClient _client;

        /// <summary>
        ///     ctor().
        /// </summary>
        public RequestProvider() {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Reset access token.
        /// </summary>
        public void ClientTokenReset() {
            if (_client != null)
                _client.DefaultRequestHeaders.Authorization = null;
        }

        /// <summary>
        /// GET.
        /// </summary>
        public async Task<TResult> GetAsync<TResult>(string uri, string accessToken = "", CancellationToken cancellationToken = default(CancellationToken)) =>
              await Task.Run(async () => {
                  TResult result = default(TResult);
                  CheckInternetConnection();
                  SetAccesToken(accessToken);

                  HttpResponseMessage response = await _client.GetAsync(uri, cancellationToken);

                  await HandleResponse(response);

                  string serialized = await response.Content.ReadAsStringAsync();
                  result = await DeserializeResponse<TResult>(serialized);

                  return result;
              }, cancellationToken);

        /// <summary>
        /// POST.
        /// </summary>
        public async Task<TResult> PostAsync<TResult, TBodyContent>(string uri, TBodyContent bodyContent, string accessToken = "") =>
            await Task.Run(async () => {
                HttpContent content = null;
                CheckInternetConnection();
                SetAccesToken(accessToken);

                if (bodyContent != null) {
                    string jObject = JsonConvert.SerializeObject(bodyContent);

                    content = new StringContent(jObject);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }

                HttpResponseMessage response = await _client.PostAsync(uri, content);

                await HandleResponse(response);

                string serialized = await response.Content.ReadAsStringAsync();

                TResult result = await DeserializeResponse<TResult>(serialized);

                return result;
            });

        /// <summary>
        /// PUT.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task<TResult> PutAsync<TResult, TBodyContent>(string url, TBodyContent bodyContent, string accessToken = "") =>
            await Task.Run(async () => {
                HttpContent content = null;
                TResult result = default(TResult);

                CheckInternetConnection();
                SetAccesToken(accessToken);

                if (bodyContent != null) {
                    content = new StringContent(JsonConvert.SerializeObject(bodyContent));
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                }
                HttpResponseMessage response = await _client.PutAsync(url, content);

                await HandleResponse(response);

                string serialized = await response.Content.ReadAsStringAsync();

                result = await DeserializeResponse<TResult>(serialized);

                return result;
            });

        private static async Task<TResult> DeserializeResponse<TResult>(string serialized) =>
           await Task.Run(() => {
               if (serialized != null) {
                   return JsonSerializerHelper.Deserialize<TResult>(serialized);
               }
               return default(TResult);
           });

        private void CheckInternetConnection() {
            if (!CrossConnectivity.Current.IsConnected)
                throw new ConnectivityException(ApplicationConsts.ERROR_INTERNET_CONNECTION);
        }

        private void SetAccesToken(string accessToken) {
            if (_client.DefaultRequestHeaders.Authorization == null) {
                if (!string.IsNullOrEmpty(accessToken)) {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }
            } else {
                if (!(string.IsNullOrEmpty(accessToken)) && _client.DefaultRequestHeaders.Authorization.Parameter != accessToken) {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }
            }
        }

        private async Task HandleResponse(HttpResponseMessage response) {
            if (!response.IsSuccessStatusCode) {
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized) {

                    ClientTokenReset();

                    throw new ServiceAuthenticationException(content);
                }

                throw new HttpRequestExceptionEx(response.StatusCode, content);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace beyond.park.client.Services.RequestProvider {
    public interface IRequestProvider {
        Task<TResult> GetAsync<TResult>(string uri, string accessToken = "", CancellationToken cancellationToken = default(CancellationToken));

        Task<TResult> PostAsync<TResult, TBodyContent>(string url, TBodyContent bodyContent, string accessToken = "");

        Task<TResult> PutAsync<TResult, TBodyContent>(string url, TBodyContent bodyContent, string accessToken = "");

    
    }
}

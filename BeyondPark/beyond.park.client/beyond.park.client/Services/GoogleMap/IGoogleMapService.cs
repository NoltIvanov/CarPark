using beyond.park.client.Models.Rest.GoogleMap;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace beyond.park.client.Services.GoogleMap {
    public interface IGoogleMapService {
        Task<object> GetDirections(string originLatitude, string originLongitude, string destinationLatitude, string destinationLongitude, CancellationToken cancellationToken = default(CancellationToken));
    }
}

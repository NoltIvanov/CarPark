using System;
using System.Collections.Generic;
using System.Text;

namespace beyond.park.client.Models.Rest.EndPoints {
    public sealed class GoogleMapsEndpoints : BaseEndpoints {

        private const string SERVER_API_KEY = "AIzaSyCDKBahuZrEAQng47zKeypj3WR0_1KXhwo";

        private const string GET_DIRECTIONS_API_KEY = "api/directions/json?origin={0},{1}&destination={2},{3}&key=";

        //private const string GET_DIRECTIONS_API_KEY = "api/directions/json?mode=driving&transit_routing_preference=less_driving&origin={0},{1}&destination={2},{3}&key=";
    
        public string GetDirectionsEndPoint { get; private set; }

        /// <summary>
        ///     ctor().
        /// </summary>
        /// <param name="defaultEndPoint"></param>
        public GoogleMapsEndpoints(string defaultEndPoint) : base(defaultEndPoint) { }

        public override void UpdateEndpoint(string baseEndpoint) {
            GetDirectionsEndPoint = $"{baseEndpoint}/{GET_DIRECTIONS_API_KEY}{SERVER_API_KEY}";
        }
    }
}

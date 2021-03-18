using Newtonsoft.Json;

namespace beyond.park.client.Models.Rest.GoogleMap {
    public sealed class GoogleDirection {
        [JsonProperty("geocoded_waypoints")]
        public GeocodedWaypoint[] GeocodedWaypoints { get; set; }

        [JsonProperty("routes")]
        public Route[] Routes { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}

using Newtonsoft.Json;

namespace beyond.park.client.Models.Rest.GoogleMap {
    public sealed class Northeast {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }
    }
}

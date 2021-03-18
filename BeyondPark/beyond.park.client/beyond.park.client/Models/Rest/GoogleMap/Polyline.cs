using Newtonsoft.Json;

namespace beyond.park.client.Models.Rest.GoogleMap {
    public sealed class Polyline {
        [JsonProperty("points")]
        public string Points { get; set; }
    }
}

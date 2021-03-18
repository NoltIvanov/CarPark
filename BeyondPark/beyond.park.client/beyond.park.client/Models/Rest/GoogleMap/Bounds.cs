using Newtonsoft.Json;

namespace beyond.park.client.Models.Rest.GoogleMap {
    public sealed class Bounds {
        [JsonProperty("northeast")]
        public Northeast Northeast { get; set; }

        [JsonProperty("southwest")]
        public Northeast Southwest { get; set; }
    }
}

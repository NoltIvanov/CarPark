using Newtonsoft.Json;

namespace beyond.park.client.Models.Rest.GoogleMap {
    public sealed class Distance {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("value")]
        public long Value { get; set; }
    }
}

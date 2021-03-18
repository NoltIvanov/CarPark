using Newtonsoft.Json;

namespace beyond.park.client.Models.Rest.GoogleMap {
    public sealed class Leg {
        [JsonProperty("distance")]
        public Distance Distance { get; set; }

        [JsonProperty("duration")]
        public Distance Duration { get; set; }

        [JsonProperty("end_address")]
        public string EndAddress { get; set; }

        [JsonProperty("end_location")]
        public Northeast EndLocation { get; set; }

        [JsonProperty("start_address")]
        public string StartAddress { get; set; }

        [JsonProperty("start_location")]
        public Northeast StartLocation { get; set; }

        [JsonProperty("steps")]
        public Step[] Steps { get; set; }

        [JsonProperty("traffic_speed_entry")]
        public object[] TrafficSpeedEntry { get; set; }

        [JsonProperty("via_waypoint")]
        public object[] ViaWaypoint { get; set; }
    }
}

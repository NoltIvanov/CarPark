using Newtonsoft.Json;

namespace beyond.park.client.Models.Rest.GoogleMap {
    public sealed class Route {
        [JsonProperty("bounds")]
        public Bounds Bounds { get; set; }

        [JsonProperty("copyrights")]
        public string Copyrights { get; set; }

        [JsonProperty("legs")]
        public Leg[] Legs { get; set; }

        [JsonProperty("overview_polyline")]
        public Polyline OverviewPolyline { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("warnings")]
        public object[] Warnings { get; set; }

        [JsonProperty("waypoint_order")]
        public object[] WaypointOrder { get; set; }
    }
}

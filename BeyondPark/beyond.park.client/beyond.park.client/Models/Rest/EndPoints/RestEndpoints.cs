namespace beyond.park.client.Models.Rest.EndPoints {
    public sealed class RestEndpoints {

        //public const string DEFAULT_ENDPOINT = ""; // Release

        public const string DEFAULT_ENDPOINT = "https://bpuserdev.azurewebsites.net"; // Development

        //public const string DEFAULT_ENDPOINT = ""; // Local

        public const string GOOGLE_MAP_ENDPOINT = "https://maps.googleapis.com/maps";

        public IdentityEndpoints IdentityEndpoints { get; set; } = new IdentityEndpoints(DEFAULT_ENDPOINT);

        public GoogleMapsEndpoints GoogleMapsEndpoints { get; set; } = new GoogleMapsEndpoints(GOOGLE_MAP_ENDPOINT);
    }
}

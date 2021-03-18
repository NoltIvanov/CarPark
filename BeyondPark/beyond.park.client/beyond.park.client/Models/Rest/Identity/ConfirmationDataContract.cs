using Newtonsoft.Json;

namespace beyond.park.client.Models.Rest.Identity {
    public sealed class ConfirmationDataContract {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}

using Newtonsoft.Json;

namespace beyond.park.client.Models.Rest.Identity {
    public sealed class RegisterDataContract {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}

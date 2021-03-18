using Newtonsoft.Json;

namespace beyond.park.client.Models.Rest.Identity {
    public sealed class SignInBody {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("identityToken")]
        public object IdentityToken { get; set; }

        [JsonProperty("tokenType")]
        public string TokenType { get; set; }

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonProperty("expiresIn")]
        public long ExpiresIn { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("photo")]
        public object Photo { get; set; }
    }
}

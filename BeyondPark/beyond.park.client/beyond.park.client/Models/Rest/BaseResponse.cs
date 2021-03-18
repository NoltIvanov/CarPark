using beyond.park.client.Models.Rest.Enums;
using Newtonsoft.Json;

namespace beyond.park.client.Models.Rest {
    public abstract class BaseResponse<T> {
        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }

        [JsonProperty("StatusCode")]
        public BeyondParkStatusCodes StatusCode { get; set; }
    }
}

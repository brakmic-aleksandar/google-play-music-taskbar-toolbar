using Newtonsoft.Json;

namespace GooglePlayMusicDesktop.Communication.DataTransferObject
{
    class ControllerResponse
    {
        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }

        [JsonProperty("requestID")]
        public long RequestId { get; set; }
    }

    class ControllerResponse<T> : ControllerResponse
    {
        [JsonProperty("value")]
        public new T Value { get; set; }
    }
}

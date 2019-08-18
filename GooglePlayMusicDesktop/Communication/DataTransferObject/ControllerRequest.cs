using System.Collections.Generic;
using Newtonsoft.Json;

namespace GooglePlayMusicDesktop.Communication.DataTransferObject
{
    class ControllerRequest
    {
        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("arguments")]
        public List<string> Arguments { get; set; } = new List<string>();

        [JsonProperty("requestID")]
        public int Id { get; set; }

        public bool HasResponse;
    }
}

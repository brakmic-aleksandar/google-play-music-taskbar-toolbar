using Newtonsoft.Json;

namespace GooglePlayMusicDesktop.Communication.DataTransferObject
{
    public class TimeDto
    {
        [JsonProperty("current")]
        public int Current { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }
}

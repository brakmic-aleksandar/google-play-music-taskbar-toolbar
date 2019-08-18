using Newtonsoft.Json;

namespace GooglePlayMusicDesktop.Communication.DataTransferObject
{
    public class PlaylistTrackDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("index")]
        public long Index { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("artist")]
        public string Artist { get; set; }

        [JsonProperty("album")]
        public string Album { get; set; }

        [JsonProperty("albumArt")]
        public string AlbumArt { get; set; }

        [JsonProperty("duration")]
        public long Duration { get; set; }

        [JsonProperty("playCount")]
        public long PlayCount { get; set; }
    }
}

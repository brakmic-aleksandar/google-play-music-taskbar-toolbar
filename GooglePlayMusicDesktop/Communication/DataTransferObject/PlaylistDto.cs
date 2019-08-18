using System.Collections.Generic;
using Newtonsoft.Json;

namespace GooglePlayMusicDesktop.Communication.DataTransferObject
{
    class PlaylistDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tracks")]
        public List<PlaylistTrackDto> Tracks { get; set; }
    }
}

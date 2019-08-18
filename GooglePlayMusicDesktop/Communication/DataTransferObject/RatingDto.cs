using Newtonsoft.Json;

namespace GooglePlayMusicDesktop.Communication.DataTransferObject
{
    class RatingDto
    {
        [JsonProperty("liked")]
        public bool Liked { get; set; }

        [JsonProperty("disliked")]
        public bool Disliked { get; set; }
    }
}

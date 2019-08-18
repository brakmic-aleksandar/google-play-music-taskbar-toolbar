using Newtonsoft.Json;

namespace GooglePlayMusicDesktop.Communication
{
    public class Message
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }
    }

    public class Message<T> : Message
    {
        [JsonProperty("payload")]
        public T Payload { get; set; }
    }
}

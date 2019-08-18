using System.Collections.Generic;
using System.Diagnostics;
using GooglePlayMusicDesktop.Communication.DataTransferObject;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GooglePlayMusicDesktop.Communication
{
    class ResponseMessageFactory
    {
        public static ResponseMessageFactory Instance { get; } = new ResponseMessageFactory();

        public Message Create(string json)
        {
            switch (GetChannelName(json))
            {
                case Channel.ApiVersion:
                    return CreateMessage<string>(json);
                case Channel.Lyrics:
                    return CreateMessage<string>(json);
                case Channel.PlayState:
                    return CreateMessage<bool>(json);
                case Channel.Playlists:
                    return CreateMessage<List<PlaylistDto>>(json);
                case Channel.Rating:
                    return CreateMessage<RatingDto>(json);
                case Channel.Repeat:
                    return CreateMessage<string>(json);
                case Channel.SearchResults:
                    return CreateMessage<SearchResultsDto>(json);
                case Channel.Shuffle:
                    return CreateMessage<string>(json);
                case Channel.Time:
                    return CreateMessage<TimeDto>(json);
                case Channel.Track:
                    return CreateMessage<TrackDto>(json);
                case Channel.Volume:
                    return CreateMessage<ushort>(json);
                case Channel.Queue:
                    return CreateMessage<List<PlaylistTrackDto>>(json);
                case Channel.SettingsTheme:
                    return CreateMessage<bool>(json);
                case Channel.SettingsThemeColor:
                    return CreateMessage<string>(json);
                case Channel.SettingsThemeType:
                    return CreateMessage<string>(json);
                case Channel.Library:
                    return CreateMessage<LibraryDto>(json);
                case Channel.Connect:
                    return CreateMessage<string>(json);
                default:
                    Debug.WriteLine(GetChannelName(json) + "\n");
                    return new Message();
            }
        }

        private string GetChannelName(string json)
        {
            return JObject.Parse(json)["channel"].Value<string>();
        }

        private Message<T> CreateMessage<T>(string json)
        {
            return JsonConvert.DeserializeObject<Message<T>>(json);
        }

    }
}

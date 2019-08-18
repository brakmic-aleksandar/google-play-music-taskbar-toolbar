using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GooglePlayMusicDesktop.Communication;
using GooglePlayMusicDesktop.Communication.DataTransferObject;

namespace GooglePlayMusicDesktop
{
    public class GooglePlayMusicDesktop
    {
        private static GooglePlayMusicDesktop _instance;
        public static GooglePlayMusicDesktop Instance => _instance ?? (_instance = new GooglePlayMusicDesktop());

        private readonly CommunicationService _communicationService = new CommunicationService();

        public string ApiVersion { get; private set; }
        public bool IsPlaying { get; private set; }
        public bool Shuffle { get; private set; }
        public ushort Volume { get; private set; }
        public Repeat Repeat { get; private set; }
        public Track CurrentTrack { get; private set; }
        public string ApplicationName { get; set; } = "GPMLibrary";
        public bool IsConnected => _communicationService.IsConnected;

        public event EventHandler<EventArgs> OnConnected;
        public event EventHandler<PlayStateChangedEventArgs> PlayStateChanged;
        public event EventHandler<LyricsChangedEventArgs> LyricsChanged;
        public event EventHandler<TrackChangedEventArgs> TrackChanged;
        public event EventHandler<TimeUpdatedEventArgs> TimeUpdated;
        public event EventHandler<RatingUpdatedEventArgs> RatingUpdated;
        public event EventHandler<ShuffleUpdatedEventArgs> ShuffleUpdated;
        public event EventHandler<VolumeUpdatedEventArgs> VolumeUpdated;
        public event EventHandler<RepeatUpdatedEventArgs> RepeatUpdated;
        public event EventHandler<AuthorizationCodeRequiredEventArgs> AuthorizationCodeRequiered;
        public event EventHandler<AuthorizationTokenReceivedEventArgs> AuthorizationTokenReceived;

        public GooglePlayMusicDesktop()
        {
            _communicationService.MessageRecieved += OnMessageRecieved;
            _communicationService.OnConnected += (sender, args) => OnConnected?.Invoke(this, EventArgs.Empty);
        }

        public async void SendControlPermissionRequestCode(string code)
        {
            await _communicationService.SendRequestAsync(new ControllerRequest
            {
                Namespace = "connect",
                Method = "connect",
                Arguments = new List<string> { ApplicationName, code }
            });
        }

        public async void RequestControlPermission(string token)
        {
            await _communicationService.SendRequestAsync(new ControllerRequest
            {
                Namespace = "connect",
                Method = "connect",
                Arguments = new List<string> { ApplicationName, token }
            });
        }

        public async void ToggleIsPlaying()
        {
            await _communicationService.SendRequestAsync(new ControllerRequest
            {
                Namespace = "playback",
                Method = "playPause"
            });
        }

        public async void Forward()
        {
            await _communicationService.SendRequestAsync(new ControllerRequest
            {
                Namespace = "playback",
                Method = "forward"
            });
        }

        public async void Rewind()
        {
            await _communicationService.SendRequestAsync(new ControllerRequest
            {
                Namespace = "playback",
                Method = "rewind"
            });
        }

        public async Task<string> GetCurrentTrackUrl()
        {
            return (await _communicationService.SendRequestAsync(new ControllerRequest
            {
                Namespace = "extras",
                Method = "getTrackURL",
                HasResponse = true     
            })).Value.ToString();
        }

        public async Task<int> GetVolume()
        {
            return Int32.Parse((await _communicationService.SendRequestAsync(new ControllerRequest
            {
                Namespace = "volume",
                Method = "getVolume",
                HasResponse = true
            })).Value.ToString());
        }

        public async void SetVolume(ushort value)
        {
            if (value > 100)
                throw new ArgumentException("Volume can't be higher than 100");
            await _communicationService.SendRequestAsync(new ControllerRequest
            {
                Namespace = "volume",
                Method = "setVolume",
                Arguments = new List<string> { value.ToString() }
            });
        }

        public async void IncreaseVolume(ushort value)
        {
            await _communicationService.SendRequestAsync(new ControllerRequest
            {
                Namespace = "volume",
                Method = "increaseVolume",
                Arguments = new List<string> { value.ToString() }
            });
        }

        public async void DecreaseVolume(ushort value)
        {
            await _communicationService.SendRequestAsync(new ControllerRequest
            {
                Namespace = "volume",
                Method = "decreaseVolume",
                Arguments = new List<string> { value.ToString() }
            });
        }

        public async void SetCurrentTime(int value)
        {
            await _communicationService.SendRequestAsync(new ControllerRequest
            {
                Namespace = "playback",
                Method = "setCurrentTime",
                Arguments = new List<string> { value.ToString() }
            });
        }

        public async Task<Rating> GetCurrentTrackRating()
        {
            return (Rating) (await _communicationService.SendRequestAsync(new ControllerRequest
            {
                Namespace = "rating",
                Method = "getRating"
            })).Value;
        }

        public async void SetCurrentTrackRating(Rating rating)
        {
            switch (rating)
            {
                case Rating.None:
                    await _communicationService.SendRequestAsync(new ControllerRequest
                    {
                        Namespace = "rating",
                        Method = "resetRating"
                    });
                    break;
                case Rating.Like:
                case Rating.Disliked:
                    await _communicationService.SendRequestAsync(new ControllerRequest
                    {
                        Namespace = "rating",
                        Method = "setRating"
                    });
                    break;
            }
            
        }

        private void OnMessageRecieved(object sender, MessageEventArgs messageEventArgs)
        {
            Message message = messageEventArgs.Message;
            switch (message.Channel)
            {
                case Channel.ApiVersion:
                    ApiVersion = CastMessage<string>(message).Payload;
                    break;
                case Channel.PlayState:
                    HandlePlayStateMessage(CastMessage<bool>(message).Payload);
                    break;
                case Channel.Shuffle:
                    HandleShuffleMessage(CastMessage<string>(message).Payload);
                    break;
                case Channel.Repeat:
                    HandleRepeatMessage(CastMessage<string>(message).Payload);
                    break;
                case Channel.Queue:
                    break;
                case Channel.SearchResults:
                    break;
                case Channel.Volume:
                    HandleVolumeMessage(CastMessage<ushort>(message).Payload);
                    break;
                case Channel.Track:
                    HandleTrackMessage(CastMessage<TrackDto>(message).Payload);
                    break;
                case Channel.Time:
                    HandleTimeMessage(CastMessage<TimeDto>(message).Payload);
                    break;
                case Channel.Lyrics:
                    HandleLyricsMessage(CastMessage<string>(message).Payload);
                    break;
                case Channel.Rating:
                    HandleRatingMessage(CastMessage<RatingDto>(message).Payload);
                    break;
                case Channel.SettingsThemeColor:
                    break;
                case Channel.SettingsTheme:
                    break;
                case Channel.SettingsThemeType:
                    break;
                case Channel.Playlists:
                    break;
                case Channel.Library:
                    break;
                case Channel.Connect:
                    HandleConnectMessage(CastMessage<string>(message).Payload);
                    break;
            }
        }

        private void HandleConnectMessage(string message)
        {
            if (message == "CODE_REQUIRED")
            {
                AuthorizationCodeRequiered?.Invoke(this, new AuthorizationCodeRequiredEventArgs());
            }
            else
            {
                AuthorizationTokenReceived?.Invoke(this, new AuthorizationTokenReceivedEventArgs(message));
            }
        }

        private void HandleRepeatMessage(string repeat)
        {
            Repeat = RepeatFromString(repeat);
            RepeatUpdated?.Invoke(this, new RepeatUpdatedEventArgs(Repeat));
        }

        private void HandleVolumeMessage(ushort volume)
        {
            Volume = volume;
            VolumeUpdated?.Invoke(this, new VolumeUpdatedEventArgs(Volume));
        }

        private void HandleShuffleMessage(string payload)
        {
            Shuffle = payload == "ALL_SHUFFLE";
            ShuffleUpdated?.Invoke(this, new ShuffleUpdatedEventArgs(Shuffle));
        }

        private void HandleRatingMessage(RatingDto payload)
        {
            CurrentTrack.Rating = ExtractRating(payload);
            RatingUpdated?.Invoke(this, new RatingUpdatedEventArgs(CurrentTrack.Rating));
        }

        private void HandlePlayStateMessage(bool isPlaying)
        {
            IsPlaying = isPlaying;
            PlayStateChanged?.Invoke(this, new PlayStateChangedEventArgs(IsPlaying));
        }

        private void HandleLyricsMessage(string lyrics)
        {
            CurrentTrack.Lyrics = lyrics;
            LyricsChanged?.Invoke(this, new LyricsChangedEventArgs(CurrentTrack.Lyrics));
        }

        private void HandleTimeMessage(TimeDto time)
        {
            CurrentTrack.Duration = time.Total;
            CurrentTrack.CurrentTime = time.Current;
            TimeUpdated?.Invoke(this, new TimeUpdatedEventArgs(CurrentTrack.Duration, CurrentTrack.CurrentTime));
        }

        private void HandleTrackMessage(TrackDto track)
        {
            CurrentTrack = new Track
            {
                Artist = track.Artist,
                Title = track.Title,
                Album = track.Album,
                AlbumArtUrl = track.AlbumArt
            };
            TrackChanged?.Invoke(this, new TrackChangedEventArgs(CurrentTrack));
        }

        private Message<T> CastMessage<T>(Message message)
        {
            return (Message<T>) message;
        }

        private Rating ExtractRating(RatingDto rating)
        {
            if (rating.Liked)
                return Rating.Like;
            if (rating.Disliked)
                return Rating.Disliked;

            return Rating.None;
        }

        private Repeat RepeatFromString(string repeatValue)
        {
            switch (repeatValue)
            {
                case "LIST_REPEAT":
                    return Repeat.List;
                case "SINGLE_REPEAT":
                    return Repeat.Single;
                default:
                    return Repeat.No;
            }
        }
    }

    public class PlayStateChangedEventArgs : EventArgs
    {
        public bool IsPlaying { get; }

        public PlayStateChangedEventArgs(bool isPlaying)
        {
            IsPlaying = isPlaying;
        }
    }

    public class LyricsChangedEventArgs : EventArgs
    {
        public string Lyrics { get; }

        public LyricsChangedEventArgs(string lyrics)
        {
            Lyrics = lyrics;
        }
    }

    public class TrackChangedEventArgs : EventArgs
    {
        public Track Track { get; }

        public TrackChangedEventArgs(Track track)
        {
            Track = track;
        }
    }

    public class TimeUpdatedEventArgs : EventArgs
    {
        public int Total { get; }
        public int Current { get; }

        public TimeUpdatedEventArgs(int total, int current)
        {
            Total = total;
            Current = current;
        }
    }

    public class RatingUpdatedEventArgs : EventArgs
    {
        public Rating Rating { get; }

        public RatingUpdatedEventArgs(Rating rating)
        {
            Rating = rating;
        }
    }

    public class ShuffleUpdatedEventArgs : EventArgs
    {
        public bool Shuffle { get; }

        public ShuffleUpdatedEventArgs(bool shuffle)
        {
            Shuffle = shuffle;
        }
    }

    public class VolumeUpdatedEventArgs : EventArgs
    {
        public ushort Volume { get; }

        public VolumeUpdatedEventArgs(ushort volume)
        {
            Volume = volume;
        }
    }

    public class RepeatUpdatedEventArgs : EventArgs
    {
        public Repeat Repeat { get; }

        public RepeatUpdatedEventArgs(Repeat repeat)
        {
            Repeat = repeat;
        }
    }

    public class AuthorizationCodeRequiredEventArgs : EventArgs
    {

    }

    public class AuthorizationTokenReceivedEventArgs : EventArgs
    {
        public string Token { get; }

        public AuthorizationTokenReceivedEventArgs(string token)
        {
            Token = token;
        }
    }
}

using System;
using GooglePlayMusicDesktop;

namespace WindowsTaskbarAudio.ViewModel
{
    class CurrentSongDetailsViewModel : ViewModel
    {
        private readonly GooglePlayMusicDesktop.GooglePlayMusicDesktop _service =
            GooglePlayMusicDesktop.GooglePlayMusicDesktop.Instance;

        private string _trackTitle;
        private string _trackArtist;
        private string _trackAlbum;
        private int _currentTrackTime;
        private int _totalTrackTime;
        private string _trackArtUrl;
        private ushort _volume;

        public String TrackTitle
        {
            get => _trackTitle;
            set
            {
                _trackTitle = value;
                OnPropertyChanged();
            }
        }

        public String TrackArtist
        {
            get => _trackArtist;
            set
            {
                _trackArtist = value;
                OnPropertyChanged();
            }
        }

        public String TrackAlbum
        {
            get => _trackAlbum;
            set
            {
                _trackAlbum = value;
                OnPropertyChanged();
            }
        }

        public String TrackArtUrl
        {
            get => _trackArtUrl;
            set
            {
                _trackArtUrl = value;
                OnPropertyChanged();
            }
        }

        public int CurrentTrackTime
        {
            get => _currentTrackTime;
            set
            {
                if (_service.IsConnected)
                    _service.SetCurrentTime(value);
            }
        }

        public int TotalTrackTime
        {
            get => _totalTrackTime;
            set
            {
                _totalTrackTime = value;
                OnPropertyChanged();
            }
        }

        public ushort Volume
        {
            get => _volume;
            set
            {
                if (_service.IsConnected)
                    _service.SetVolume(value);
            }
        }

        public bool IsUserAdjustingTrackTime { get; set; }

        public CurrentSongDetailsViewModel()
        {
            if (_service.IsConnected)
            {
                Track currentTrack = _service.CurrentTrack;
                if (currentTrack != null)
                {
                    TrackTitle = currentTrack.Title;
                    TrackAlbum = currentTrack.Album;
                    TrackArtUrl = currentTrack.AlbumArtUrl;
                    TrackArtist = currentTrack.Artist;
                    _currentTrackTime = currentTrack.CurrentTime;
                    OnPropertyChanged(nameof(CurrentTrackTime));
                    TotalTrackTime = currentTrack.Duration;
                }
                _volume = _service.Volume;
                OnPropertyChanged(nameof(Volume));
            }
            _service.TrackChanged += OnTrackChanged;
            _service.TimeUpdated += OnTimeUpdated;
            _service.VolumeUpdated += OnVolumeUpdated;
        }

        private void OnVolumeUpdated(object sender, VolumeUpdatedEventArgs volumeUpdatedEventArgs)
        {
            _volume = volumeUpdatedEventArgs.Volume;
            OnPropertyChanged(nameof(Volume));
        }

        private void OnTimeUpdated(object sender, TimeUpdatedEventArgs e)
        {
            if(IsUserAdjustingTrackTime)
                return;
            _currentTrackTime = e.Current;
            OnPropertyChanged(nameof(CurrentTrackTime));
            TotalTrackTime = e.Total;
        }

        private void OnTrackChanged(object sender, TrackChangedEventArgs e)
        {
            TrackTitle = e.Track.Title;
            TrackArtist = e.Track.Artist;
            TrackAlbum = e.Track.Album;
            TrackArtUrl = e.Track.AlbumArtUrl;
        }
    }
}
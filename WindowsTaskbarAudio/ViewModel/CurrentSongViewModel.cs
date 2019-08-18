using System;
using WindowsTaskbarAudio.Command;
using GooglePlayMusicDesktop;

namespace WindowsTaskbarAudio.ViewModel
{
    class CurrentSongViewModel : ViewModel
    {
        private readonly GooglePlayMusicDesktop.GooglePlayMusicDesktop _service = GooglePlayMusicDesktop.GooglePlayMusicDesktop.Instance;
        private bool _isPlaying;
        private string _artistAndSongName;
        private bool _isVisible = true;

        public RelayCommand PlayPauseCommand { get; }
        public RelayCommand ForwardCommand { get; }
        public RelayCommand RewindCommand { get; }
        public bool IsVisible
        {
            get => _isVisible;
            private set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        public string ArtistAndSongName
        {
            get => _artistAndSongName;
            set
            {
                _artistAndSongName = value;
                OnPropertyChanged();
            }
        }

        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                _isPlaying = value;
                OnPropertyChanged();
            }
        }

        public CurrentSongViewModel()
        {
            PlayPauseCommand = new RelayCommand(ToggleIsPlaying);
            ForwardCommand = new RelayCommand(Forward);
            RewindCommand = new RelayCommand(Rewind);
            _service.OnConnected += OnConnected;
            _service.PlayStateChanged += OnPlayStateChanged;
            _service.AuthorizationCodeRequiered += OnAuthorizationCodeRequiered;
            _service.AuthorizationTokenReceived += OnAuthorizationTokenReceived;
        }

        private void OnAuthorizationCodeRequiered(object sender, AuthorizationCodeRequiredEventArgs authorizationCodeRequiredEventArgs)
        {
            IsVisible = false;
        }

        private void OnAuthorizationTokenReceived(object sender, AuthorizationTokenReceivedEventArgs authorizationTokenReceivedEventArgs)
        {
            IsVisible = true;
        }

        private void OnConnected(object sender, EventArgs args)
        {
            _service.TrackChanged += OnTrackChanged;
            _service.RequestControlPermission(Properties.Settings.Default["Token"].ToString());
        }

        private void OnTrackChanged(object s, TrackChangedEventArgs a)
        {
            ArtistAndSongName = a.Track.Artist + " - " + a.Track.Title;
        }

        private void OnPlayStateChanged(object s, PlayStateChangedEventArgs a)
        {
            IsPlaying = a.IsPlaying;
        }

        private void ToggleIsPlaying()
        {
            if(_service.IsConnected)
                _service.ToggleIsPlaying();
        }

        private void Forward()
        {
            if (_service.IsConnected)
                _service.Forward();
        }

        private void Rewind()
        {
            if (_service.IsConnected)
                _service.Rewind();
        }
    }
}

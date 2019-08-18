using WindowsTaskbarAudio.Command;
using GooglePlayMusicDesktop;

namespace WindowsTaskbarAudio.ViewModel
{
    class AuthorizationCodeEntryViewModel : ViewModel
    {
        private readonly GooglePlayMusicDesktop.GooglePlayMusicDesktop _service = GooglePlayMusicDesktop.GooglePlayMusicDesktop.Instance;
        private bool _isVisible;
        private string _code;

        public bool IsVisible
        {
            get => _isVisible;
            private set
            {
                _isVisible = value; 
                OnPropertyChanged();
            }
        }

        public string Code
        {
            get => _code;
            set
            {
                _code = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand SubmitCodeCommand { get; }
        public RelayCommand ResendCodeRequestCommand { get; }

        public AuthorizationCodeEntryViewModel()
        {
            SubmitCodeCommand = new RelayCommand(SubmitCode);
            ResendCodeRequestCommand = new RelayCommand(ResendCodeRequest);
            _service.AuthorizationCodeRequiered += OnAuthorizationCodeRequiered;
            _service.AuthorizationTokenReceived += OnAuthorizationTokenReceived;
        }

        private void ResendCodeRequest()
        {
            _service.RequestControlPermission(Properties.Settings.Default["Token"].ToString());
        }

        private void SubmitCode()
        {
            if(!string.IsNullOrEmpty(Code))
            {
                _service.SendControlPermissionRequestCode(Code);
            }
        }

        private void OnAuthorizationTokenReceived(object sender, AuthorizationTokenReceivedEventArgs e)
        {
            Properties.Settings.Default["Token"] = e.Token;
            Properties.Settings.Default.Save();
            _service.RequestControlPermission(e.Token);
            IsVisible = false;
        }

        private void OnAuthorizationCodeRequiered(object sender, AuthorizationCodeRequiredEventArgs e)
        {
            Code = null;
            IsVisible = true;
        }
    }
}

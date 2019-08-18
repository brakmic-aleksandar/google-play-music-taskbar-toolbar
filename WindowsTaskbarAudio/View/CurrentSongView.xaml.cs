using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WindowsTaskbarAudio.ViewModel;

namespace WindowsTaskbarAudio.View
{
    public partial class CurrentSongView : UserControl
    {
        private CurrentSongDetailsView _details;

        public CurrentSongView()
        {
            InitializeComponent();
            DataContext = new CurrentSongViewModel();
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_details == null)
            {
                _details = new CurrentSongDetailsView();
            }
            Point point = PointToScreen(new Point(0, 0));
            _details.Left = point.X;
            _details.Top = point.Y - _details.Height;
            _details.Show();
            _details.Focus();
        }
    }
}

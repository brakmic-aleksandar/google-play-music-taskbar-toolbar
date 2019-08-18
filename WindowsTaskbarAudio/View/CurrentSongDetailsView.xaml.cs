using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using WindowsTaskbarAudio.ViewModel;

namespace WindowsTaskbarAudio.View
{
    public partial class CurrentSongDetailsView : Window
    {
        public CurrentSongDetailsView()
        {
            InitializeComponent();
            DataContext = new CurrentSongDetailsViewModel();
            LostKeyboardFocus += OnLostKeyboardFocus;
        }

        private void OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs keyboardFocusChangedEventArgs)
        {
            Hide();
        }

        private void TrackTimeDragStarted(object sender, DragStartedEventArgs e)
        {
            ((CurrentSongDetailsViewModel) DataContext).IsUserAdjustingTrackTime = true;
        }

        private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            ((CurrentSongDetailsViewModel)DataContext).IsUserAdjustingTrackTime = false;
        }
    }
}

using System.Windows.Controls;
using WindowsTaskbarAudio.ViewModel;

namespace WindowsTaskbarAudio.View
{
    public partial class AuthorizationCodeEntryView : UserControl
    {
        public AuthorizationCodeEntryView()
        {
            InitializeComponent();
            DataContext = new  AuthorizationCodeEntryViewModel();
        }
    }
}

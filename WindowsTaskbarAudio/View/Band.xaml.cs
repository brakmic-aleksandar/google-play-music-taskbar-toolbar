using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using CSDeskBand;
using CSDeskBand.ContextMenu;

namespace WindowsTaskbarAudio.View
{
    [ComVisible(true)]
    [Guid("89BF6B36-A0B0-4C95-A666-87A55C226986")]
    [CSDeskBandRegistration(Name = "Google Play Music DeskBand", ShowDeskBand = true)]
    public partial class Band : INotifyPropertyChanged
    {
        private Orientation _taskbarOrientation;
        private int _taskbarWidth;
        private int _taskbarHeight;
        private Edge _taskbarEdge;

        public Orientation TaskbarOrientation
        {
            get => _taskbarOrientation;
            set
            {
                if (value == _taskbarOrientation) return;
                _taskbarOrientation = value;
                OnPropertyChanged();
            }
        }

        public int TaskbarWidth
        {
            get => _taskbarWidth;
            set
            {
                if (value == _taskbarWidth) return;
                _taskbarWidth = value;
                OnPropertyChanged();
            }
        }

        public int TaskbarHeight
        {
            get => _taskbarHeight;
            set
            {
                if (value == _taskbarHeight) return;
                _taskbarHeight = value;
                OnPropertyChanged();
            }
        }

        public Edge TaskbarEdge
        {
            get => _taskbarEdge;
            set
            {
                if (value == _taskbarEdge) return;
                _taskbarEdge = value;
                OnPropertyChanged();
            }
        }

        private List<DeskBandMenuItem> ContextMenuItems => new List<DeskBandMenuItem>();

        public Band()
        {
            try
            {

                InitializeComponent();
                Options.MinHorizontalSize.Width = 200;
                Options.MinVerticalSize.Width = 130;
                Options.MinVerticalSize.Height = 200;

                TaskbarInfo.TaskbarEdgeChanged += (sender, args) => TaskbarEdge = args.Edge;
                TaskbarInfo.TaskbarOrientationChanged += (sender, args) => TaskbarOrientation = args.Orientation == CSDeskBand.TaskbarOrientation.Horizontal ? Orientation.Horizontal : Orientation.Vertical;
                TaskbarInfo.TaskbarSizeChanged += (sender, args) =>
                {
                    TaskbarWidth = args.Size.Width;
                    TaskbarHeight = args.Size.Height;
                };

                TaskbarEdge = TaskbarInfo.Edge;
                TaskbarOrientation = TaskbarInfo.Orientation == CSDeskBand.TaskbarOrientation.Horizontal ? Orientation.Horizontal : Orientation.Vertical;
                TaskbarWidth = TaskbarInfo.Size.Width;
                TaskbarHeight = TaskbarInfo.Size.Height;

                Options.ContextMenuItems = ContextMenuItems;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MessageBox.Show(e.Message);
                MessageBox.Show(e.Source);
                MessageBox.Show(e.StackTrace);
                throw;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

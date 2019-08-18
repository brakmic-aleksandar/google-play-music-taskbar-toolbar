using System;
using System.Windows.Input;

namespace WindowsTaskbarAudio.Command
{
    class RelayCommand : ICommand
    {
        private readonly Action _action;
        private bool _active = true;

        public bool Active
        {
            get => _active;
            set
            {
                _active = value;
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }

        public RelayCommand(Action action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return Active;
        }

        public void Execute(object parameter = null)
        {
            if (CanExecute(null))
            {
                _action.Invoke();
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}

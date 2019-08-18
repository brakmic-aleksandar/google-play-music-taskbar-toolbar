using System;
using System.Windows.Input;

namespace WindowsTaskbarAudio.Command
{
    class ParamtereizedRelayCommand<T> : ICommand
    {
        private readonly Action<T> _action;
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

        public ParamtereizedRelayCommand(Action<T> action)
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
                _action.Invoke((T) parameter);
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}

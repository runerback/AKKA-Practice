using System;
using System.Windows.Input;

namespace GithubActors
{
    sealed class DelegateCommand : ICommand
    {
        private readonly Action<object> action;
        private readonly Predicate<object> predicate;

        public DelegateCommand(Action<object> action, Predicate<object> predicate)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            this.predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
        }

        bool ICommand.CanExecute(object parameter)
        {
            return predicate.Invoke(parameter);
        }

        void ICommand.Execute(object parameter)
        {
            action.Invoke(parameter);
        }

        public void NotifyCanExecuteChanged()
        {
            canExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        private EventHandler canExecuteChanged;
        event EventHandler ICommand.CanExecuteChanged
        {
            add { canExecuteChanged += value; }
            remove { canExecuteChanged -= value; }
        }
    }
}

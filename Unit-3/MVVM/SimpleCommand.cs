using System;
using System.Windows.Input;

namespace GithubActors
{
    sealed class SimpleCommand : ICommand
    {
        private readonly Action<object> action;

        public SimpleCommand(Action<object> action)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
        }

        bool ICommand.CanExecute(object parameter) => true;

        void ICommand.Execute(object parameter)
        {
            action.Invoke(parameter);
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add { }
            remove { }
        }
    }
}

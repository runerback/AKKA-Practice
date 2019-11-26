using System.Windows.Input;

namespace GithubActors
{
    interface IDispatcherCommand : ICommand
    {
        void NotifyCanExecuteChanged();
    }
}

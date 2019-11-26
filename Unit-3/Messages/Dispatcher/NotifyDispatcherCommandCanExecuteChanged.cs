using System;

namespace GithubActors.Messages
{
    sealed class NotifyDispatcherCommandCanExecuteChanged
    {
        public NotifyDispatcherCommandCanExecuteChanged(IDispatcherCommand command)
        {
            Command = command ?? throw new ArgumentNullException(nameof(command));
        }

        public IDispatcherCommand Command { get; }
    }
}

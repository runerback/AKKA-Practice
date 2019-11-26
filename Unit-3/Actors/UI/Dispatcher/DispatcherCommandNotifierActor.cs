using Akka.Actor;
using GithubActors.Messages;

namespace GithubActors.Actors
{
    sealed class DispatcherCommandNotifierActor : ReceiveActor
    {
        public DispatcherCommandNotifierActor()
        {
            ActorPathPrinter.Print(Self);

            Receive<NotifyDispatcherCommandCanExecuteChanged>(msg =>
           {
               msg.Command.NotifyCanExecuteChanged();
           });
        }
    }
}

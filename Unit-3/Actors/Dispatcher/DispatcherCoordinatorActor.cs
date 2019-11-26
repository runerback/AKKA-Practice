using Akka.Actor;
using GithubActors.Messages;

namespace GithubActors.Actors
{
    sealed class DispatcherCoordinatorActor : ReceiveActor
    {
        public DispatcherCoordinatorActor()
        {
            Receive<NotifyDispatcherCommandCanExecuteChanged>(msg =>
            {
                Context.ActorSelection(ActorPaths.DispatcherCommandNotifier).Tell(msg);
            });

            Receive<PageNavigate>(msg =>
            {
                Context.ActorSelection(ActorPaths.PageNavigator).Tell(msg);
            });

            Receive<PageNavigateBack>(msg =>
            {
                Context.ActorSelection(ActorPaths.PageNavigator).Tell(msg);
            });

            Receive<CreateDispatcherActor>(msg =>
            {
                Context.ActorOf(
                    Props.Create(msg.ActorType, msg.ConstructorParams)
                        .WithDispatcher("akka.actor.synchronized-dispatcher"),
                    msg.ActorName);
            });

            Self.Tell(CreateDispatcherActor.Build<DispatcherCommandNotifierActor>(
                ActorNames.DispatcherCommandNotifier));
        }
    }
}

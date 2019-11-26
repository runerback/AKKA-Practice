using Akka.Actor;
using GithubActors.Messages;

namespace GithubActors.Actors
{
    sealed class DispatcherCoordinatorActor : ReceiveActor//, IWithUnboundedStash
    {
        public DispatcherCoordinatorActor()
        {
            System.Console.WriteLine("DispatcherCoordinatorActor created");

            ActorPathPrinter.Print(Self);

            Receive<NotifyDispatcherCommandCanExecuteChanged>(msg =>
            {
                Context.ActorSelection(ActorPaths.DispatcherCommandNotifier).Tell(msg);
            });

            Receive<PageNavigate>(msg =>
            {
                System.Console.WriteLine("Navigating . . .");
                Context.ActorSelection(ActorPaths.PageNavigator).Tell(msg);
            });
            System.Console.WriteLine("Receiving PageNavigate . . .");

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

        private void Initializing()
        {
            Receive<NotifyDispatcherCommandCanExecuteChanged>(msg =>
            {
                Context.ActorSelection(ActorPaths.DispatcherCommandNotifier).Tell(msg);
            });

            Receive<PageNavigate>(msg =>
            {
                System.Console.WriteLine("Navigating . . .");
                Context.ActorSelection(ActorPaths.PageNavigator).Tell(msg);
            });
            System.Console.WriteLine("Receiving PageNavigate . . .");

            Receive<PageNavigateBack>(msg =>
            {
                Context.ActorSelection(ActorPaths.PageNavigator).Tell(msg);
            });

        }
    }
}

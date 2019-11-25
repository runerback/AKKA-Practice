using Akka.Actor;
using GithubActors.Messages;
using System;

namespace GithubActors.Actors
{
    sealed class AuthStatusCoordinatorActor : ReceiveActor
    {
        private readonly IActorRef authStatusActor;
        private readonly IActorRef busyStatusActor;

        public AuthStatusCoordinatorActor(
            Action<string> updateStatus, 
            Action<string> updateStatusColor,
            Action<bool> setIsBusy)
        {
            authStatusActor = Context.ActorOf(
                Props.Create<AuthStatusActor>(updateStatus, updateStatusColor),
                ActorNames.AuthStatus);
            busyStatusActor = Context.ActorOf(
                Props.Create<BusyStatusUpdatorActor>(setIsBusy),
                ActorNames.AuthBusy);

            Receive<Authenticate>(msg =>
            {
                authStatusActor.Tell(msg);
                busyStatusActor.Tell(SystemBusy.Instance);
            });
            Receive<AuthenticationFailed>(msg =>
            {
                authStatusActor.Tell(msg);
                busyStatusActor.Tell(SystemIdle.Instance);
            });
            Receive<AuthenticationCancelled>(msg =>
            {
                authStatusActor.Tell(msg);
                busyStatusActor.Tell(SystemIdle.Instance);
            });
            Receive<AuthenticationSuccess>(msg =>
            {
                authStatusActor.Tell(msg);
                busyStatusActor.Tell(SystemIdle.Instance);
            });
        }
    }
}

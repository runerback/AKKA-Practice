using Akka.Actor;
using GithubActors.Messages;
using System;
using System.Windows.Media;

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

            Receive<Authenticate>(_ =>
            {
                authStatusActor.Tell(new AuthenticateStatus(
                    "Authenticating...",
                    Colors.DeepSkyBlue));
                busyStatusActor.Tell(SystemBusy.Instance);
            });
            Receive<AuthenticationFailed>(_ =>
            {
                authStatusActor.Tell(new AuthenticateStatus(
                    "Authentication failed. Please try again.",
                    Colors.Red));
                busyStatusActor.Tell(SystemIdle.Instance);
            });
            Receive<AuthenticationCancelled>(_ =>
            {
                authStatusActor.Tell(new AuthenticateStatus(
                    "Authentication timed out. Please try again later.",
                    Colors.Red));
                busyStatusActor.Tell(SystemIdle.Instance);
            });
            Receive<AuthenticationSuccess>(_ =>
            {
                authStatusActor.Tell(new AuthenticateStatus(
                    "Authentication succeed.",
                    Colors.Green));
                busyStatusActor.Tell(SystemIdle.Instance);
            });
        }
    }
}

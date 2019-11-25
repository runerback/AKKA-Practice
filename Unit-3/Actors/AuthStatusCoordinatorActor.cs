using Akka.Actor;
using GithubActors.Messages;
using System.Windows.Media;

namespace GithubActors.Actors
{
    sealed class AuthStatusCoordinatorActor : ReceiveActor
    {
        public AuthStatusCoordinatorActor()
        {
            Receive<Authenticate>(_ =>
            {
                GetStatusActor()?.Tell(new AuthenticateStatus(
                    "Authenticating...",
                    Colors.DeepSkyBlue));
                GetAuthCommandActor()?.Tell(SystemBusy.Instance);
            });
            Receive<AuthenticationFailed>(_ =>
            {
                GetStatusActor()?.Tell(new AuthenticateStatus(
                    "Authentication failed. Please try again.",
                    Colors.Red));
                GetAuthCommandActor()?.Tell(SystemIdle.Instance);
            });
            Receive<AuthenticationCancelled>(_ =>
            {
                GetStatusActor()?.Tell(new AuthenticateStatus(
                    "Authentication timed out. Please try again later.",
                    Colors.Red));
                GetAuthCommandActor()?.Tell(SystemIdle.Instance);
            });
            Receive<AuthenticationSuccess>(_ =>
            {
                GetStatusActor()?.Tell(new AuthenticateStatus(
                    "Authentication succeed.",
                    Colors.Green));
                GetAuthCommandActor()?.Tell(SystemIdle.Instance);
            });
        }

        private ActorSelection GetStatusActor()
        {
            return App.UIActors.ActorSelection(ActorPaths.AuthStatus);
        }

        private ActorSelection GetAuthCommandActor()
        {
            return App.UIActors.ActorSelection(ActorPaths.AuthBusy);
        }
    }
}

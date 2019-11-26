using Akka.Actor;
using GithubActors.Messages;

namespace GithubActors.Actors
{
    sealed class GithubAuthenticationActor : ReceiveActor
    {
        public GithubAuthenticationActor()
        {
            Unauthenticated();
        }

        private void Unauthenticated()
        {
            Receive<Authenticate>(auth =>
            {
                BecomeAuthenticating(auth);

                if (string.IsNullOrWhiteSpace(auth.OAuthToken))
                {
                    Self.Tell(new AuthenticationFailed());
                    return;
                }

                //need a client to test our credentials with
                GithubClientFactory
                    .GetClient(auth.OAuthToken)
                    .User
                    .Current()
                    .ContinueWith<object>(tr =>
                    {
                        if (tr.IsFaulted)
                            return new AuthenticationFailed();
                        if (tr.IsCanceled)
                            return new AuthenticationCancelled();
                        return new AuthenticationSuccess(auth.OAuthToken);
                    })
                    .PipeTo(Self);
            });
        }

        private void BecomeAuthenticating(object message)
        {
            GetAuthStatusCoordinator().Tell(message);
            Become(Authenticating);
        }

        private void Authenticating()
        {
            Receive<AuthenticationFailed>(failed => BecomeUnauthenticated(failed));
            Receive<AuthenticationCancelled>(cancelled => BecomeUnauthenticated(cancelled));
            Receive<AuthenticationSuccess>(success =>
            {
                GetAuthStatusCoordinator().Tell(success);

                var launcherActor = App.UIActors.ActorOf(
                    Props.Create<RepoLauncherActor>()
                        .WithDispatcher("akka.actor.synchronized-dispatcher"),
                    ActorNames.RepoLauncher);
                launcherActor.Tell(success);
            });
        }

        private void BecomeUnauthenticated(object message)
        {
            GetAuthStatusCoordinator().Tell(message);
            Become(Unauthenticated);
        }

        private ActorSelection GetAuthStatusCoordinator()
        {
            return App.UIActors.ActorSelection(ActorPaths.AuthStatusCooridnator);
        }
    }
}

using Akka.Actor;
using GithubActors.Messages;

namespace GithubActors.Actors
{
    sealed class GithubAuthenticationActor : ReceiveActor
    {
        private readonly IActorRef authStatusPresentActor;

        public GithubAuthenticationActor()
        {
            authStatusPresentActor = Context.ActorOf(
                Props.Create<AuthStatusCoordinatorActor>(),
                ActorNames.AuthStatusPresent);

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
                        return new AuthenticationSuccess();
                    })
                    .PipeTo(Self);
            });
        }

        private void BecomeAuthenticating(object message)
        {
            authStatusPresentActor.Tell(message);
            Become(Authenticating);
        }

        private void Authenticating()
        {
            Receive<AuthenticationFailed>(failed => BecomeUnauthenticated(failed));
            Receive<AuthenticationCancelled>(cancelled => BecomeUnauthenticated(cancelled));
            Receive<AuthenticationSuccess>(success =>
            {
                authStatusPresentActor.Tell(success);

                App.UIActors
                    .ActorSelection(ActorPaths.PageNavigator)
                    ?.Tell(PageNavigate.Create<Views.LauncherForm, ViewModels.LauncherForm>("Who Starred This Repo?"));
            });
        }

        private void BecomeUnauthenticated(object message)
        {
            authStatusPresentActor.Tell(message);
            Become(Unauthenticated);
        }
    }
}

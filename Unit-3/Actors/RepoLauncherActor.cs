using Akka.Actor;
using GithubActors.Messages;

namespace GithubActors.Actors
{
    sealed class RepoLauncherActor : ReceiveActor
    {
        private readonly IActorRef repoValidatorActor;

        public RepoLauncherActor()
        {
            repoValidatorActor = App.GithubActors.ActorOf(
                Props.Create<GetHubRepoValidatorActor>(),
                ActorNames.RepoValidator);

            Receive<AuthenticationSuccess>(auth =>
            {
                App.UIActors
                    .ActorSelection(ActorPaths.PageNavigator)
                    ?.Tell(PageNavigate.Create<Views.LauncherForm, ViewModels.LauncherForm>("Who Starred This Repo?"));

                repoValidatorActor.Tell(GithubClientFactory.GetClient(auth.Token));
           });
        }
    }
}

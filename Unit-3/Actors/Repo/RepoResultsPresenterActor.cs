using Akka.Actor;
using GithubActors.Messages;

namespace GithubActors.Actors
{
    sealed class RepoResultsPresenterActor : ReceiveActor
    {
        public RepoResultsPresenterActor()
        {
            Receive<ProcessRepo>(repo =>
            {
                App.UIActors
                    .ActorSelection(ActorPaths.PageNavigator)
                    .Tell(PageNavigate.Create<Views.RepoResultsForm, ViewModels.RepoResultsForm>($"Repos Similar to {repo}"));
            });

            Receive<LaunchRepo>(repo =>
            {
                Context.ActorSelection(ActorPaths.RepoCoordinator).Tell(repo);
            });
        }
    }
}

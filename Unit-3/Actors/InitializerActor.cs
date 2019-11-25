using Akka.Actor;
using GithubActors.Messages;

namespace GithubActors.Actors
{
    sealed class InitializerActor : ReceiveActor
    {
        public InitializerActor()
        {
            Receive<Initialize>(_ =>
            {
                App.UIActors
                    .ActorSelection(ActorPaths.PageNavigator)
                    .Tell(PageNavigate.Create<Views.GithubAuth, ViewModels.GithubAuth>("Sign in to GitHub"));

                Become(Initialized);
            });
        }

        //ignore Initialize message
        private void Initialized() { }
    }
}

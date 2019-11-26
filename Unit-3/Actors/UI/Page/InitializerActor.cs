using Akka.Actor;
using GithubActors.Messages;

namespace GithubActors.Actors
{
    sealed class InitializerActor : ReceiveActor
    {
        public InitializerActor()
        {
            ActorPathPrinter.Print(Self);

            Receive<Initialize>(_ =>
            {
                System.Console.WriteLine("Initialize");

                App.UIActors
                    .ActorSelection(ActorPaths.DispatcherCoordinator)
                    .Tell(PageNavigate.Create<Views.GithubAuth, ViewModels.GithubAuth>("Sign in to GitHub"));

                Become(Initialized);
            });
        }

        //ignore Initialize message
        private void Initialized() { }
    }
}

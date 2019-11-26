using Akka.Actor;
using GithubActors.Messages;
using System.Collections.Generic;

namespace GithubActors.Actors
{
    sealed class RepoResultsPresenterActor : ReceiveActor, IWithUnboundedStash
    {
        private PageNavigate pageNavigate;

        public RepoResultsPresenterActor()
        {
            Initializing();
        }

        private void Initializing()
        {
            Receive<ProcessRepo>(repo =>
            {
                App.UIActors
                    .ActorSelection(ActorPaths.DispatcherCoordinator)
                    .Tell(pageNavigate);

                App.UIActors
                    .ActorSelection(ActorPaths.PageTitle)
                    .Tell(new PageTitle($"Repos Similar to {repo}"));

                BecomeReady();
            });

            Receive<GithubProgressStats>(stats =>
            {
                ((IWithUnboundedStash)this).Stash.Stash();
            });
            Receive<IEnumerable<SimilarRepo>>(repos =>
            {
                ((IWithUnboundedStash)this).Stash.Stash();
            });
            Receive<JobFailed>(failed =>
            {
                ((IWithUnboundedStash)this).Stash.Stash();
            });
        }

        private void BecomeReady()
        {
            Become(Ready);

            ((IWithUnboundedStash)this).Stash.UnstashAll();
        }

        private void Ready()
        {
            Receive<GithubProgressStats>(stats =>
            {
                GetRepoResultsCoordinator().Tell(stats);
            });

            Receive<IEnumerable<SimilarRepo>>(repos =>
            {
                GetRepoResultsCoordinator().Tell(repos);
            });

            Receive<JobFailed>(failed =>
            {
                GetRepoResultsCoordinator().Tell(failed);
            });
        }

        private ActorSelection GetRepoResultsCoordinator()
        {
            return Context.ActorSelection(ActorPaths.RepoCoordinator);
        }

        protected override void PreStart()
        {
            pageNavigate = PageNavigate.Create<Views.RepoResultsForm, ViewModels.RepoResultsForm>();

            base.PreStart();
        }

        IStash IActorStash.Stash { get; set; }
    }
}

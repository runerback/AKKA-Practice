using Akka.Actor;
using GithubActors.Messages;
using System;
using System.Collections.Generic;

namespace GithubActors.Actors
{
    sealed class MainFormActor : ReceiveActor, IWithUnboundedStash
    {
        private readonly IActorRef repoResultsPresenterActor;

        public MainFormActor()
        {
            ActorPathPrinter.Print(Self);

            repoResultsPresenterActor = Context.ActorOf(
                Props.Create<RepoResultsPresenterActor>(),
                ActorNames.RepoResultsPresenter);

            Ready();
        }

        /// <summary>
        /// State for when we're able to accept new jobs
        /// </summary>
        private void Ready()
        {
            Receive<ProcessRepo>(repo =>
            {
                BecomeBusy(repo);
            });
        }

        /// <summary>
        /// Make any necessary URI updates, then switch our state to busy
        /// </summary>
        private void BecomeBusy(ProcessRepo repo)
        {
            repoResultsPresenterActor.Tell(repo);

            App.GithubActors.ActorSelection(ActorPaths.GithubCoordinator).Tell(new SubscribeToProgressUpdates(Self));

            Become(Busy);
        }

        /// <summary>
        /// State for when we're currently processing a job
        /// </summary>
        private void Busy()
        {
            Receive<GithubProgressStats>(stats =>
            {
                repoResultsPresenterActor.Tell(stats);
            });

            Receive<IEnumerable<SimilarRepo>>(repos =>
            {
                repoResultsPresenterActor.Tell(repos);
            });

            Receive<JobFailed>(failed =>
            {
                repoResultsPresenterActor.Tell(failed);
            });

            Receive<ProcessRepo>(repo => ((IWithUnboundedStash)this).Stash.Stash());
        }

        private void BecomeReady()
        {
            ((IWithUnboundedStash)this).Stash.UnstashAll();
            Become(Ready);
        }

        IStash IActorStash.Stash { get; set; }
    }
}

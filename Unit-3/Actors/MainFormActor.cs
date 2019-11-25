using Akka.Actor;
using GithubActors.Messages;

namespace GithubActors.Actors
{
    sealed class MainFormActor : ReceiveActor, IWithUnboundedStash
    {
        private readonly IActorRef repoResultsPresenterActor;

        public MainFormActor()
        {
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
                repoResultsPresenterActor.Tell(repo);
                BecomeBusy(repo);
            });

            //launch the repo results
            Receive<LaunchRepo>(repo =>
            {
                repoResultsPresenterActor.Tell(repo);
            });
        }

        /// <summary>
        /// Make any necessary URI updates, then switch our state to busy
        /// </summary>
        private void BecomeBusy(ProcessRepo repo)
        {
            Become(Busy);
        }

        /// <summary>
        /// State for when we're currently processing a job
        /// </summary>
        private void Busy()
        {
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

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
                Context.ActorSelection(ActorPaths.RepoValidator).Tell(new ValidateRepo(repo.URL));
                GetRepoValidateStatusActor().Tell(repo);
                BecomeBusy(repo.URL);
            });

            //launch the window
            Receive<LaunchRepo>(repo =>
            {
                repoResultsPresenterActor.Tell(repo);
                Context.ActorSelection(ActorPaths.RepoResults).Tell(repo);
            });
        }

        /// <summary>
        /// Make any necessary URI updates, then switch our state to busy
        /// </summary>
        private void BecomeBusy(string repoUrl)
        {
            Context.ActorSelection(ActorPaths.RepoValidateBusy).Tell(SystemBusy.Instance);
            Become(Busy);
        }

        /// <summary>
        /// State for when we're currently processing a job
        /// </summary>
        private void Busy()
        {
            Receive<ValidRepo>(valid =>
            {
                GetRepoValidateStatusActor().Tell(valid);
                BecomeReady();
            });
            Receive<InvalidRepo>(invalid =>
            {
                GetRepoValidateStatusActor().Tell(invalid);
                BecomeReady();
            });
            Receive<UnableToAcceptJob>(job =>
            {
                GetRepoValidateStatusActor().Tell(job);
                BecomeReady();
            });
            Receive<AbleToAcceptJob>(job =>
            {
                GetRepoValidateStatusActor().Tell(job);
                BecomeReady();
            });

            Receive<LaunchRepo>(repo => ((IWithUnboundedStash)this).Stash.Stash());
        }

        private void BecomeReady()
        {
            ((IWithUnboundedStash)this).Stash.UnstashAll();
            Become(Ready);
        }

        private ActorSelection GetRepoValidateStatusActor()
        {
            return Context.ActorSelection(ActorPaths.RepoValidateStatus);
        }

        IStash IActorStash.Stash { get; set; }
    }
}

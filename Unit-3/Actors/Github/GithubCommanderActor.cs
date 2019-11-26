using Akka.Actor;
using GithubActors.Messages;
using Octokit;
using System;

namespace GithubActors.Actors
{
    /// <summary>
    /// Top-level actor responsible for coordinating and launching repo-processing jobs
    /// </summary>
    sealed class GithubCommanderActor : ReceiveActor
    {
        private readonly Func<IGitHubClient> gitHubClientFactory;

        private IActorRef githubCoordinator;
        private IActorRef canAcceptJobSender;

        public GithubCommanderActor(Func<IGitHubClient> gitHubClientFactory)
        {
            ActorPathPrinter.Print(Self);

            this.gitHubClientFactory = gitHubClientFactory ??
                throw new ArgumentNullException(nameof(gitHubClientFactory));

            Receive<CanAcceptJob>(job =>
            {
                canAcceptJobSender = Sender;
                githubCoordinator.Tell(job);
            });

            Receive<UnableToAcceptJob>(job =>
            {
                canAcceptJobSender.Tell(job);
            });

            Receive<AbleToAcceptJob>(job =>
            {
                canAcceptJobSender.Tell(job);

                //start processing messages
                githubCoordinator.Tell(new BeginJob(job));
            });
        }

        protected override void PreStart()
        {
            githubCoordinator = Context.ActorOf(
                Props.Create<GithubCoordinatorActor>(gitHubClientFactory),
                ActorNames.GithubCoordinator);

            base.PreStart();
        }

        protected override void PreRestart(Exception reason, object message)
        {
            //kill off the old coordinator so we can recreate it from scratch
            githubCoordinator.Tell(PoisonPill.Instance);
            githubCoordinator = null;

            base.PreRestart(reason, message);
        }
    }
}

using Akka.Actor;
using GithubActors.Messages;
using System;

namespace GithubActors.Actors
{
    sealed class RepoStatusCoordinatorActor : ReceiveActor
    {
        private readonly IActorRef repoStatusActor;
        private readonly IActorRef busyStatusActor;

        public RepoStatusCoordinatorActor(
            Action<string> updateStatus,
            Action<string> updateStatusColor,
            Action<bool> setIsBusy)
        {
            repoStatusActor = App.UIActors.ActorOf(
                Props.Create<RepoStatusActor>(updateStatus, updateStatusColor),
                ActorNames.RepoStatus);
            busyStatusActor = App.UIActors.ActorOf(
                Props.Create<BusyStatusUpdatorActor>(setIsBusy),
                ActorNames.RepoBusy);
            
            Receive<ValidateRepo>(repo =>
            {
                repoStatusActor.Tell(repo);
                busyStatusActor.Tell(SystemBusy.Instance);
            });
            Receive<ValidRepo>(valid =>
            {
                repoStatusActor.Tell(valid);
                busyStatusActor.Tell(SystemIdle.Instance);
            });
            Receive<InvalidRepo>(invalid =>
            {
                repoStatusActor.Tell(invalid);
                busyStatusActor.Tell(SystemIdle.Instance);
            });
            Receive<CanAcceptJob>(ask =>
            {
                repoStatusActor.Tell(ask);
                busyStatusActor.Tell(SystemBusy.Instance);
            });
            Receive<UnableToAcceptJob>(job =>
            {
                repoStatusActor.Tell(job);
                busyStatusActor.Tell(SystemIdle.Instance);
            });
            Receive<AbleToAcceptJob>(job =>
            {
                repoStatusActor.Tell(job);
                busyStatusActor.Tell(SystemIdle.Instance);
            });
        }
    }
}

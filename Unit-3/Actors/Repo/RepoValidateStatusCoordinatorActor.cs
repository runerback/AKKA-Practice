using Akka.Actor;
using GithubActors.Messages;
using System;

namespace GithubActors.Actors
{
    sealed class RepoValidateStatusCoordinatorActor : ReceiveActor
    {
        private readonly IActorRef validateStatusActor;
        private readonly IActorRef busyStatusActor;

        public RepoValidateStatusCoordinatorActor(
            Action<string> updateStatus,
            Action<string> updateStatusColor,
            Action<bool> setIsBusy)
        {
            validateStatusActor = App.UIActors.ActorOf(
                Props.Create<RepoValidateStatusActor>(updateStatus, updateStatusColor),
                ActorNames.RepoValidateStatus);
            busyStatusActor = App.UIActors.ActorOf(
                Props.Create<BusyStatusUpdatorActor>(setIsBusy),
                ActorNames.RepoValidateBusy);
            
            Receive<ProcessRepo>(repo =>
            {
                validateStatusActor.Tell(repo);
                busyStatusActor.Tell(SystemBusy.Instance);
            });
            Receive<ValidRepo>(valid =>
            {
                validateStatusActor.Tell(valid);
                busyStatusActor.Tell(SystemIdle.Instance);
            });
            Receive<InvalidRepo>(invalid =>
            {
                validateStatusActor.Tell(invalid);
                busyStatusActor.Tell(SystemIdle.Instance);
            });
            Receive<UnableToAcceptJob>(job =>
            {
                validateStatusActor.Tell(job);
                busyStatusActor.Tell(SystemIdle.Instance);
            });
            Receive<AbleToAcceptJob>(job =>
            {
                validateStatusActor.Tell(job);
                busyStatusActor.Tell(SystemIdle.Instance);
            });
        }
    }
}

using Akka.Actor;
using GithubActors.Messages;
using System;

namespace GithubActors.Actors
{
    sealed class BusyStatusUpdatorActor : ReceiveActor
    {
        private readonly Action<bool> setIsBusy;

        public BusyStatusUpdatorActor(Action<bool> setIsBusy)
        {
            ActorPathPrinter.Print(Self);

            this.setIsBusy = setIsBusy ?? throw new ArgumentNullException(nameof(setIsBusy));

            Receive<SystemBusy>(_ => this.setIsBusy(true));
            Receive<SystemIdle>(_ => this.setIsBusy(false));
        }
    }
}

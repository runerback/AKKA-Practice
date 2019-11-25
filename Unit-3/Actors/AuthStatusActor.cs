using Akka.Actor;
using GithubActors.Messages;
using System;
using System.Windows.Media;

namespace GithubActors.Actors
{
    sealed class AuthStatusActor : ReceiveActor
    {
        private readonly Action<string> updateStatus;
        private readonly Action<Color> updateStatusColor;

        public AuthStatusActor(Action<string> updateStatus, Action<Color> updateStatusColor)
        {
            this.updateStatus = updateStatus;
            this.updateStatusColor = updateStatusColor;

            Receive<AuthenticateStatus>(value =>
           {
               this.updateStatus(value.Status);
               this.updateStatusColor(value.Color);
           });
        }
    }
}

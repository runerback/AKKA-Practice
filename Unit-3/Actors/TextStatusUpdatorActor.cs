using Akka.Actor;
using System;

namespace GithubActors.Actors
{
    abstract class TextStatusUpdatorActor : ReceiveActor
    {
        private readonly Action<string> updateStatus;
        private readonly Action<string> updateStatusColor;

        public TextStatusUpdatorActor(Action<string> updateStatus, Action<string> updateStatusColor)
        {
            this.updateStatus = updateStatus ??
                throw new ArgumentNullException(nameof(updateStatus));
            this.updateStatusColor = updateStatusColor ??
                throw new ArgumentNullException(nameof(updateStatusColor));

            UpdateColor(StatusColors.Idle);
        }

        protected void UpdateStatus(string status)
        {
            updateStatus(status);
        }

        protected void UpdateColor(string color)
        {
            updateStatusColor(color);
        }
    }
}

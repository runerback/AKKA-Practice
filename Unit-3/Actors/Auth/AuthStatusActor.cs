using GithubActors.Messages;
using System;

namespace GithubActors.Actors
{
    sealed class AuthStatusActor : TextStatusUpdatorActor
    {
        public AuthStatusActor(Action<string> updateStatus, Action<string> updateStatusColor) : 
            base(updateStatus, updateStatusColor)
        {
            Receive<Authenticate>(_ =>
            {
                UpdateStatus("Authenticating...");
                UpdateColor(StatusColors.Querying);
            });

            Receive<AuthenticationFailed>(_ =>
            {
                UpdateStatus("Authentication failed. Please try again.");
                UpdateColor(StatusColors.Failed);
            });

            Receive<AuthenticationCancelled>(_ =>
            {
                UpdateStatus("Authentication timed out. Please try again later.");
                UpdateColor(StatusColors.Failed);
            });

            Receive<AuthenticationSuccess>(_ =>
            {
                UpdateStatus("Authentication succeed.");
                UpdateColor(StatusColors.Succeed);
            });
        }
    }
}

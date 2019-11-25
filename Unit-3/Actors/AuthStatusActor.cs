using GithubActors.Messages;
using System;

namespace GithubActors.Actors
{
    sealed class AuthStatusActor : TextStatusUpdatorActor
    {
        public AuthStatusActor(Action<string> updateStatus, Action<string> updateStatusColor) : 
            base(updateStatus, updateStatusColor)
        {
            Receive<AuthenticateStatus>(value =>
           {
               UpdateStatus(value.Status);
               UpdateColor(value.Color.ToString());
           });
        }
    }
}

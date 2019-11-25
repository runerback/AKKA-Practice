using GithubActors.Messages;
using System;

namespace GithubActors.Actors
{
    sealed class RepoStatusActor : TextStatusUpdatorActor
    {
        public RepoStatusActor(Action<string> updateStatus, Action<string> updateStatusColor) : 
            base(updateStatus, updateStatusColor)
        {
            Receive<ValidateRepo>(repo =>
            {
                UpdateStatus($"Validating {repo.URL} . . .");
                UpdateColor(StatusColors.Querying);
            });
            
            Receive<ValidRepo>(valid =>
            {
                UpdateStatus("Valid!");
                UpdateColor(StatusColors.Succeed);
            });

            Receive<InvalidRepo>(invalid =>
            {
                UpdateStatus(invalid.Reason);
                UpdateColor(StatusColors.Failed);
            });

            Receive<CanAcceptJob>(ask =>
            {
                UpdateStatus($"Asking {ask} . . .");
                UpdateColor(StatusColors.Querying);
            });

            Receive<UnableToAcceptJob>(job =>
            {
                UpdateStatus($"{job} is a valid repo, but system can't accept additional jobs");
                UpdateColor(StatusColors.Failed);
            });
            
            Receive<AbleToAcceptJob>(job =>
            {
                UpdateStatus($"{job} is a valid repo - starting job!");
                UpdateColor(StatusColors.Succeed);
            });
        }
    }
}

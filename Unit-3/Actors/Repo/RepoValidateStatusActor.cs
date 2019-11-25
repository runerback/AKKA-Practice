using GithubActors.Messages;
using System;
using System.Windows.Media;

namespace GithubActors.Actors
{
    sealed class RepoValidateStatusActor : TextStatusUpdatorActor
    {
        public RepoValidateStatusActor(Action<string> updateStatus, Action<string> updateStatusColor) : 
            base(updateStatus, updateStatusColor)
        {
            Receive<ProcessRepo>(repo =>
            {
                UpdateStatus($"Validating {repo.URL} . . .");
                UpdateColor(Colors.DeepSkyBlue.ToString());
            });
            
            Receive<ValidRepo>(valid =>
            {
                UpdateStatus("Valid!");
                UpdateColor(Colors.Green.ToString());
            });

            Receive<InvalidRepo>(invalid =>
            {
                UpdateStatus(invalid.Reason);
                UpdateColor(Colors.Red.ToString());
            });

            //yes
            Receive<UnableToAcceptJob>(job =>
            {
                UpdateStatus($"{job} is a valid repo, but system can't accept additional jobs");
                UpdateColor(Colors.Red.ToString());
            });

            //no
            Receive<AbleToAcceptJob>(job =>
            {
                UpdateStatus($"{job} is a valid repo - starting job!");
                UpdateColor(Colors.Green.ToString());
            });
        }
    }
}

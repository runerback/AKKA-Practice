using Akka.Actor;
using GithubActors.Messages;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubActors.Actors
{
    sealed class GetHubRepoValidatorActor : ReceiveActor, IWithUnboundedStash
    {
        public GetHubRepoValidatorActor()
        {
            Receive<IGitHubClient>(client =>
            {

            });
        }

        IStash IActorStash.Stash { get; set; }
    }
}

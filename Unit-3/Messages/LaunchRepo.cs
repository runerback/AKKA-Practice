using Akka.Actor;
using System;

namespace GithubActors.Messages
{
    sealed class LaunchRepo : RepoKey
    {
        public LaunchRepo(RepoKey other, IActorRef coordinator) : base(other)
        {
            Coordinator = coordinator ?? throw new ArgumentNullException(nameof(coordinator));
        }
        
        public IActorRef Coordinator { get; }
    }
}

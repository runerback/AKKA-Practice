using Akka.Actor;

namespace GithubActors.Messages
{
    sealed class SubscribeToProgressUpdates
    {
        public SubscribeToProgressUpdates(IActorRef subscriber)
        {
            Subscriber = subscriber;
        }

        public IActorRef Subscriber { get; }
    }
}

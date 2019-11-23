using Akka.Actor;
using ChartApp.Messages;
using System.Collections.Generic;

namespace ChartApp.Actors
{
    sealed class ToggleCoordinatorActor : ReceiveActor, IWithUnboundedStash
    {
        private readonly IDictionary<CounterType, IActorRef> _counterToggleActors =
            new Dictionary<CounterType, IActorRef>();

        private IActorRef pauseToggleActor;

        public ToggleCoordinatorActor()
        {
            Initializing();
        }
        
        IStash IActorStash.Stash { get; set; }

        private void Initializing()
        {
            Receive<InitializeToggles>(message =>
            {
                HandleInitialize(message);
                Become(Initialized);
                ((IWithUnboundedStash)this).Stash.UnstashAll();
            });
            Receive<ToggleCounter>(toggle => ((IWithUnboundedStash)this).Stash.Stash());
            Receive<TogglePause>(toggle => ((IWithUnboundedStash)this).Stash.Stash());
        }

        private void HandleInitialize(InitializeToggles message)
        {
            _counterToggleActors.Clear();
            foreach (var pairs in message.Toggles)
                _counterToggleActors.Add(pairs);
            
            pauseToggleActor = Context.ActorOf(
                Props.Create<PauseButtonToggleActor>()
                    .WithDispatcher("akka.actor.synchronized-dispatcher"),
                "pauseToggle");
        }

        private void Initialized()
        {
            //Receive<InitializeToggles>(_ => { });
            Receive<ToggleCounter>(
                toggle => _counterToggleActors.ContainsKey(toggle.CounterType),
                toggle => HandleCounterToggle(toggle));
            Receive<TogglePause>(toggle => HandlePauseToggle(toggle));
        }

        private void HandleCounterToggle(ToggleCounter toggle)
        {
            _counterToggleActors[toggle.CounterType].Tell(toggle);
        }

        private void HandlePauseToggle(TogglePause toggle)
        {
            pauseToggleActor.Tell(toggle);
        }
    }
}

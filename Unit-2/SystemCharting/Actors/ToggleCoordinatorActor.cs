using Akka.Actor;
using ChartApp.Messages;
using System.Collections.Generic;

namespace ChartApp.Actors
{
    sealed class ToggleCoordinatorActor : ReceiveActor
    {
        private bool _initialized = false;

        /// <summary>
        /// Stash toggles before initialized.
        /// </summary>
        private List<ToggleCounter> _stashedCounterToggles;

        private readonly IDictionary<CounterType, IActorRef> _counterToggleActors =
            new Dictionary<CounterType, IActorRef>();

        private IActorRef pauseToggleActor;

        public ToggleCoordinatorActor()
        {
            Receive<InitializeToggles>(message => HandleInitialized(message));
            Receive<ToggleCounter>(
                toggle => !_initialized || _counterToggleActors.ContainsKey(toggle.CounterType),
                toggle => HandleCounterToggle(toggle));
            Receive<TogglePause>(toggle => HandlePauseToggle(toggle));
        }

        private void HandleInitialized(InitializeToggles message)
        {
            if (_initialized)
                return;

            _counterToggleActors.Clear();
            foreach (var pairs in message.Toggles)
                _counterToggleActors.Add(pairs);

            _initialized = true;

            if (_stashedCounterToggles != null)
            {
                var toggles = _stashedCounterToggles.ToArray();
                _stashedCounterToggles = null;

                foreach (var toggle in toggles)
                {
                    Self.Tell(toggle);
                }
            }

            pauseToggleActor = Context.ActorOf(
                Props.Create<PauseButtonToggleActor>()
                    .WithDispatcher("akka.actor.synchronized-dispatcher"),
                "pauseToggle");
        }

        private void HandleCounterToggle(ToggleCounter toggle)
        {
            if (!_initialized)
            {
                if (_stashedCounterToggles == null)
                    _stashedCounterToggles = new List<ToggleCounter>();
                _stashedCounterToggles.Add(toggle);
            }
            else
            {
                _counterToggleActors[toggle.CounterType].Tell(toggle);
            }
        }

        private void HandlePauseToggle(TogglePause toggle)
        {
            pauseToggleActor.Tell(toggle);
        }
    }
}

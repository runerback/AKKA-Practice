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
        private List<Toggle> _stashedToggles;

        private readonly IDictionary<CounterType, IActorRef> _toggleActors =
            new Dictionary<CounterType, IActorRef>();

        public ToggleCoordinatorActor()
        {
            Receive<InitializeToggles>(message => HandleInitialized(message));
            Receive<Toggle>(
                toggle => !_initialized || _toggleActors.ContainsKey(toggle.CounterType),
                toggle => HandleToggle(toggle));
        }

        private void HandleInitialized(InitializeToggles message)
        {
            if (_initialized)
                return;

            _toggleActors.Clear();
            foreach (var pairs in message.Toggles)
                _toggleActors.Add(pairs);

            _initialized = true;

            if (_stashedToggles != null)
            {
                var toggles = _stashedToggles.ToArray();
                _stashedToggles = null;

                foreach (var toggle in toggles)
                {
                    Self.Tell(toggle);
                }
            }
        }

        private void HandleToggle(Toggle toggle)
        {
            if (!_initialized)
            {
                if (_stashedToggles == null)
                    _stashedToggles = new List<Toggle>();
                _stashedToggles.Add(toggle);
            }
            else
            {
                _toggleActors[toggle.CounterType].Tell(toggle);
            }
        }
    }
}

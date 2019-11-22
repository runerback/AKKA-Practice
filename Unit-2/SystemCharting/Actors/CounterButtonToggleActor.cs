using Akka.Actor;
using ChartApp.Messages;

namespace ChartApp.Actors
{
    /// <summary>
    /// Actor responsible for managing button toggles
    /// </summary>
    sealed class CounterButtonToggleActor : ReceiveActor
    {
        private readonly IActorRef _counterCoorActor;

        private bool _isToggledOn = false;

        public CounterButtonToggleActor(IActorRef counterCoorActor)
        {
            _counterCoorActor = counterCoorActor;

            Receive<ToggleCounter>(toggle => HandleToggleCounterMessage(toggle));
        }

        private void HandleToggleCounterMessage(ToggleCounter toggle)
        {
            if (_isToggledOn)
            {
                // toggle is currently on

                // stop watching this counter
                _counterCoorActor.Tell(new Unwatch(toggle.CounterType));
            }
            else
            {
                // toggle is currently off

                // start watching this counter
                _counterCoorActor.Tell(new Watch(toggle.CounterType));
            }

            // flip the toggle
            _isToggledOn = !_isToggledOn;

            // change the text of the button
            toggle.Button.Text = toggle.CounterType.ToString().ToUpperInvariant() +
                (_isToggledOn ? " (ON)" : " (OFF)");
        }
    }
}

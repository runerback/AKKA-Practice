using Akka.Actor;
using ChartApp.Messages;

namespace ChartApp.Actors
{
    /// <summary>
    /// Actor responsible for managing button toggles
    /// </summary>
    sealed class ButtonToggleActor : ReceiveActor
    {
        private readonly IActorRef _coordinatorActor;

        private bool _isToggledOn = false;

        public ButtonToggleActor(IActorRef coordinatorActor)
        {
            _coordinatorActor = coordinatorActor;

            Receive<Toggle>(toggle => HandleToggleMessage(toggle));
            ReceiveAny(message => Unhandled(message));
        }

        private void HandleToggleMessage(Toggle toggle)
        {
            if (_isToggledOn)
            {
                // toggle is currently on

                // stop watching this counter
                _coordinatorActor.Tell(new Unwatch(toggle.CounterType));
            }
            else
            {
                // toggle is currently off

                // start watching this counter
                _coordinatorActor.Tell(new Watch(toggle.CounterType));
            }

            // flip the toggle
            _isToggledOn = !_isToggledOn;

            // change the text of the button
            toggle.Button.Text = toggle.CounterType.ToString().ToUpperInvariant() +
                (_isToggledOn ? " (ON)" : " (OFF)");
        }
    }
}

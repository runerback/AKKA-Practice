using Akka.Actor;
using ChartApp.Messages;

namespace ChartApp.Actors
{
    sealed class PauseButtonToggleActor : ReceiveActor
    {
        private bool paused = false;

        public PauseButtonToggleActor()
        {
            Receive<TogglePause>(toggle => HandleTogglePauseMessage(toggle));
        }

        private void HandleTogglePauseMessage(TogglePause toggle)
        {
            paused = !paused;

            toggle.Button.Text = paused ? "RESUME ▷" : "PAUSE ||";
        }
    }
}

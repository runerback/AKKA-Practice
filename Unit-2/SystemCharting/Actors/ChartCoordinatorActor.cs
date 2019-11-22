using Akka.Actor;
using ChartApp.Messages;
using System.Collections.Generic;

namespace ChartApp.Actors
{
    sealed class ChartCoordinatorActor : ReceiveActor
    {
        private readonly IActorRef _toggleCoordinatorActor;

        private IActorRef _chartActor;
        private IActorRef _counterCoordinatorActor;

        public ChartCoordinatorActor()
        {
            _toggleCoordinatorActor = Context.ActorOf(
                Props.Create<ToggleCoordinatorActor>()
                    .WithDispatcher("akka.actor.synchronized-dispatcher"),
                "toggleCoor");

            Receive<Load>(load => HandleLoad(load));
            Receive<Exit>(message => HandleExit(message));

            Receive<ToggleCounter>(toggle => HandleCounterToggle(toggle));
            Receive<TogglePause>(toggle => HandlePauseToggle(toggle));
        }

        private void HandleLoad(Load load)
        {
            _chartActor = Context.ActorOf(
                Props.Create<ChartingActor>(load.Chart)
                    .WithDispatcher("akka.actor.synchronized-dispatcher"),
                "charting");
            _chartActor.Tell(new InitializeChart()); //no initial series

            _counterCoordinatorActor = Context.ActorOf(
                Props.Create<PerformanceCounterCoordinatorActor>(_chartActor)
                    .WithDispatcher("akka.actor.synchronized-dispatcher"),
                "counters");

            var toggleActors = new Dictionary<CounterType, IActorRef>()
            {
                {
                    // CPU button toggle actor
                    CounterType.Cpu,
                    Context.ActorOf(
                        Props.Create<CounterButtonToggleActor>(_counterCoordinatorActor)
                            .WithDispatcher("akka.actor.synchronized-dispatcher"))
                },
                {
                    // MEMORY button toggle actor
                    CounterType.Memory,
                    Context.ActorOf(
                        Props.Create<CounterButtonToggleActor>(_counterCoordinatorActor)
                            .WithDispatcher("akka.actor.synchronized-dispatcher"))
                },
                {
                    // DISK button toggle actor
                    CounterType.Disk,
                    Context.ActorOf(
                        Props.Create<CounterButtonToggleActor>(_counterCoordinatorActor)
                            .WithDispatcher("akka.actor.synchronized-dispatcher"))
                }
            };

            _toggleCoordinatorActor.Tell(new InitializeToggles(toggleActors));
        }

        private void HandlePauseToggle(TogglePause toggle)
        {
            _chartActor.Tell(toggle);
            _toggleCoordinatorActor.Tell(toggle);
        }

        private void HandleCounterToggle(ToggleCounter toggle)
        {
            _toggleCoordinatorActor.Tell(toggle);
        }

        private void HandleExit(Exit message)
        {
            //shut down the toggle coordinator actor
            _toggleCoordinatorActor.Tell(PoisonPill.Instance);

            //shut down the charting actor
            _chartActor.Tell(PoisonPill.Instance);
        }
    }
}

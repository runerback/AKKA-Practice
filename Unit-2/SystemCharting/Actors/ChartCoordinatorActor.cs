using Akka.Actor;
using ChartApp.Messages;
using System;
using System.Collections.Generic;

namespace ChartApp.Actors
{
    sealed class ChartCoordinatorActor : ReceiveActor
    {
        private readonly IActorRef _toggleCoordinatorActor;

        private IActorRef _chartActor;
        private IActorRef _coordinatorActor;

        public ChartCoordinatorActor(IActorRef toggleCoordinatorActor)
        {
            _toggleCoordinatorActor = toggleCoordinatorActor ??
                throw new ArgumentNullException(nameof(toggleCoordinatorActor));

            Receive<Load>(load => HandleLoad(load));
            Receive<Exit>(message => HandleExit(message));
        }

        private void HandleLoad(Load load)
        {
            _chartActor = Program.ChartActors.ActorOf(
                Props.Create<ChartingActor>(load.Chart),
                "charting");
            _chartActor.Tell(new InitializeChart()); //no initial series

            _coordinatorActor = Program.ChartActors.ActorOf(
                Props.Create<PerformanceCounterCoordinatorActor>(_chartActor),
                "counters");

            var toggleActors = new Dictionary<CounterType, IActorRef>()
            {
                {
                    // CPU button toggle actor
                    CounterType.Cpu,
                    Program.ChartActors.ActorOf(
                        Props.Create<ButtonToggleActor>(_coordinatorActor)
                            .WithDispatcher("akka.actor.synchronized-dispatcher"))
                },
                {
                    // MEMORY button toggle actor
                    CounterType.Memory,
                    Program.ChartActors.ActorOf(
                        Props.Create<ButtonToggleActor>(_coordinatorActor)
                            .WithDispatcher("akka.actor.synchronized-dispatcher"))
                },
                {
                    // DISK button toggle actor
                    CounterType.Disk,
                    Program.ChartActors.ActorOf(
                        Props.Create<ButtonToggleActor>(_coordinatorActor)
                            .WithDispatcher("akka.actor.synchronized-dispatcher"))
                }
            };
            
            _toggleCoordinatorActor.Tell(new InitializeToggles(toggleActors));
        }

        private void HandleExit(Exit message)
        {
            //shut down the charting actor
            _chartActor.Tell(PoisonPill.Instance);
        }
    }
}

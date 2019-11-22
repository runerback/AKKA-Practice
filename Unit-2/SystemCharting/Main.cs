using Akka.Actor;
using ChartApp.Actors;
using ChartApp.Messages;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ChartApp
{
    sealed partial class Main : Form
    {
        private IActorRef _chartCoordinatorActor;
        private IActorRef _toggleCoordinatorActor;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            _toggleCoordinatorActor = Program.ChartActors.ActorOf(
                Props.Create<ToggleCoordinatorActor>()
                    .WithDispatcher("akka.actor.synchronized-dispatcher"),
                "toggleCoor");

            _chartCoordinatorActor = Program.ChartActors.ActorOf(
                Props.Create<ChartCoordinatorActor>(_toggleCoordinatorActor)
                    .WithDispatcher("akka.actor.synchronized-dispatcher"),
                "chartingCoor");
            _chartCoordinatorActor.Tell(new Load(sysChart));
            
            // Set the CPU toggle to ON so we start getting some data
            _toggleCoordinatorActor.Tell(new Toggle(cpuSwitchButton, CounterType.Cpu));
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            //shut down the charting coor actor
            _chartCoordinatorActor.Tell(new Exit());

            //shut down the ActorSystem
            Program.ChartActors.Terminate();
        }

        private void cpuSwitchButton_Click(object sender, EventArgs e)
        {
            _toggleCoordinatorActor.Tell(new Toggle(sender as Button, CounterType.Cpu));
        }

        private void memorySwitchButton_Click(object sender, EventArgs e)
        {
            _toggleCoordinatorActor.Tell(new Toggle(sender as Button, CounterType.Memory));
        }

        private void diskSwitchButton_Click(object sender, EventArgs e)
        {
            _toggleCoordinatorActor.Tell(new Toggle(sender as Button, CounterType.Disk));
        }
    }
}

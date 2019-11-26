using Akka.Actor;
using GithubActors.Actors;
using GithubActors.Messages;
using System;
using System.Windows;

namespace GithubActors
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    partial class App : Application
    {
        /// <summary>
        /// ActorSystem we'llbe using to collect and process data
        /// from Github using their official .NET SDK, Octokit
        /// </summary>
        public static readonly ActorSystem GithubActors = ActorSystem.Create(ActorNames.Github);
        public static readonly ActorSystem UIActors = ActorSystem.Create(ActorNames.UI);
        
        public App()
        {
            Exit += OnExiting;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            GithubActors.ActorOf(
                   Props.Create<GithubAuthenticationActor>()
                        .WithDispatcher("akka.actor.synchronized-dispatcher"),
                   ActorNames.Auth);

            UIActors.ActorOf(
                Props.Create<DispatcherCoordinatorActor>(),
                ActorNames.DispatcherCoordinator);

            UIActors.ActorOf(
                Props.Create<InitializerActor>()
                    .WithDispatcher("akka.actor.synchronized-dispatcher"), 
                ActorNames.Initializer
                )
                .Tell(new Initialize());
        }
        
        private void OnExiting(object sender, ExitEventArgs e)
        {
            GithubActors.Terminate();
            UIActors.Terminate();
        }
    }
}

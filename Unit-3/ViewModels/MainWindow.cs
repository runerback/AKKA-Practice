using Akka.Actor;
using GithubActors.Actors;
using System;

namespace GithubActors.ViewModels
{
    sealed class MainWindow : ViewModelBase
    {
        public MainWindow()
        {
            Action<string> setTitle = value => Title = value;
            App.UIActors.ActorOf(
                Props.Create<PageTitleActor>(setTitle),
                ActorNames.PageTitle);
        }

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                NotifyPropertyChanged(nameof(Title));
            }
        }
    }
}

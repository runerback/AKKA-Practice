using Akka.Actor;
using GithubActors.Actors;
using GithubActors.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GithubActors.ViewModels
{
    sealed class RepoResultsForm : ViewModelBase
    {
        public RepoResultsForm()
        {
            InitializeActor();
        }

        private void InitializeActor()
        {
            Action<Repo> addRepo = repos.Add;

            Func<RepoRequestProgress, RepoRequestProgress> updateProgress = progress =>
            {
                if (progress != null)
                {
                    this.progress = progress;
                    NotifyPropertyChanged(nameof(Progress));
                }
                return progress;
            };
            Action<string> updateStatus = status =>
            {
                this.status = status;
                NotifyPropertyChanged(nameof(Status));
            };

            App.UIActors.ActorOf(
                Props.Create<RepoResultsCoordinatorActor>(addRepo, updateProgress, updateStatus),
                ActorNames.RepoCoordinator);
        }
        
        private readonly ObservableCollection<Repo> repos = new ObservableCollection<Repo>();
        public IEnumerable<Repo> Repos => repos;

        private RepoRequestProgress progress = null;
        public RepoRequestProgress Progress => progress;

        private string status = null;
        public string Status => status;
    }
}

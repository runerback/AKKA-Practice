using Akka.Actor;
using GithubActors.Actors;
using System;
using System.Windows.Input;
using System.Windows.Media;

namespace GithubActors.ViewModels
{
    sealed class LauncherForm : ViewModelBase
    {
        private bool isValidating = false;

        public LauncherForm()
        {
            launchCommand = new DelegateCommand(Launch, CanLaunch);

            Action<string> updateStatus = status =>
            {
                RepoURLValidateStatus = status;
            };
            Action<string> updateStatusColor = color =>
            {
                RepoURLValidateStatusColor = color;
            };
            Action<bool> setIsValidating = validating =>
            {
                RepoURLEnabled = !validating;
                isValidating = validating;
                launchCommand.NotifyCanExecuteChanged();
            };
            App.UIActors.ActorOf(
                Props.Create<RepoValidateStatusCoordinatorActor>(updateStatus, updateStatusColor, setIsValidating),
                ActorNames.RepoValidateCoordinator);
        }

        private string repoUrl;
        public string RepoURL
        {
            set { repoUrl = value; }
        }

        private bool repoUrlEnabled = true;
        public bool RepoURLEnabled
        {
            get { return repoUrlEnabled; }
            private set
            {
                repoUrlEnabled = value;
                NotifyPropertyChanged(nameof(RepoURLEnabled));
            }
        }

        private string repoUrlValidateStatus;
        public string RepoURLValidateStatus
        {
            get { return repoUrlValidateStatus; }
            private set
            {
                repoUrlValidateStatus = value;
                NotifyPropertyChanged(nameof(RepoURLValidateStatus));
            }
        }

        private string repoUrlValidateStatusColor = Colors.Transparent.ToString();
        public string RepoURLValidateStatusColor
        {
            get { return repoUrlValidateStatusColor; }
            set
            {
                repoUrlValidateStatusColor = value;
                NotifyPropertyChanged(nameof(RepoURLValidateStatusColor));
            }
        }

        private readonly DelegateCommand launchCommand;
        public ICommand LaunchCommand => launchCommand;

        private bool CanLaunch(object obj)
        {
            return !isValidating;
        }

        private void Launch(object obj)
        {
            throw new NotImplementedException();
        }
    }
}

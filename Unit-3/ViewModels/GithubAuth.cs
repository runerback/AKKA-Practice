using Akka.Actor;
using GithubActors.Actors;
using GithubActors.Messages;
using System;
using System.Windows.Input;

namespace GithubActors.ViewModels
{
    sealed class GithubAuth : ViewModelBase
    {
        private bool authenticating = false;

        public GithubAuth()
        {
            authenticateCommand = new DelegateCommand(Authenticate, CanAuthenticate);
            helpLinkCommand = new SimpleCommand(HelpLink);

            InitializeActors();
        }

        private void InitializeActors()
        {
            Action<string> updateStatus = status => AuthStatus = status;
            Action<string> updateStatusColor = color => AuthStatusColor = color;
            Action<bool> setIsAuthenticating = authenticating =>
            {
                AuthTokenEnabled = !authenticating;
                this.authenticating = authenticating;

                App.UIActors.ActorSelection(ActorPaths.DispatcherCoordinator).Tell(
                    new NotifyDispatcherCommandCanExecuteChanged(authenticateCommand));
            };
            
            App.UIActors.ActorOf(
                Props.Create<AuthStatusCoordinatorActor>(updateStatus, updateStatusColor, setIsAuthenticating),
                ActorNames.AuthStatusCooridnator);
        }

        private string authToken;
        public string AuthToken
        {
            get { return null; }
            set
            {
                authToken = value;
                NotifyPropertyChanged(nameof(AuthToken));
            }
        }

        private bool authTokenEnabled = true;
        public bool AuthTokenEnabled
        {
            get { return authTokenEnabled; }
            private set
            {
                authTokenEnabled = value;
                NotifyPropertyChanged(nameof(AuthTokenEnabled));
            }
        }

        private string authStatus;
        public string AuthStatus
        {
            get { return authStatus; }
            private set
            {
                authStatus = value;
                NotifyPropertyChanged(nameof(AuthStatus));
            }
        }

        private string authStatusColor;
        public string AuthStatusColor
        {
            get { return authStatusColor; }
            set
            {
                authStatusColor = value;
                NotifyPropertyChanged(nameof(AuthStatusColor));
            }
        }
        
        private readonly SimpleCommand helpLinkCommand;
        public ICommand HelpLinkCommand => helpLinkCommand;

        private void HelpLink(object obj)
        {
            System.Diagnostics.Process.Start("https://help.github.com/articles/creating-an-access-token-for-command-line-use/");
        }

        private readonly DelegateCommand authenticateCommand;
        public ICommand AuthenticateCommand => authenticateCommand;

        private bool CanAuthenticate(object obj)
        {
            return !authenticating;
        }

        private void Authenticate(object obj)
        {
            App.GithubActors
                .ActorSelection(ActorPaths.Auth)
                .Tell(new Authenticate(authToken));
        }
    }
}

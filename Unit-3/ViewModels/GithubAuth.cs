using Akka.Actor;
using GithubActors.Actors;
using GithubActors.Messages;
using System;
using System.Windows.Input;
using System.Windows.Media;

namespace GithubActors.ViewModels
{
    sealed class GithubAuth : ViewModelBase
    {
        private bool authenticating = false;

        public GithubAuth()
        {
            authenticateCommand = new DelegateCommand(Authenticate, CanAuthenticate);
            helpLinkCommand = new SimpleCommand(HelpLink);

            Action<string> updateStatus = status => AuthStatus = status;
            Action<Color> updateStatusColor = color => AuthStatusColor = color.ToString();
            App.UIActors.ActorOf(
                Props.Create<AuthStatusActor>(updateStatus, updateStatusColor),
                ActorNames.AuthStatus);

            Action<bool> setIsAuthenticating = authenticating =>
            {
                AuthTokenEnabled = !authenticating;
                this.authenticating = authenticating;
                authenticateCommand.NotifyCanExecuteChanged();
            };
            App.UIActors.ActorOf(
                Props.Create<BusyStatusUpdatorActor>(setIsAuthenticating),
                ActorNames.AuthBusy);
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

        private string authStatusColor = Colors.Transparent.ToString();
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
                ?.Tell(new Authenticate(authToken));
        }
    }
}

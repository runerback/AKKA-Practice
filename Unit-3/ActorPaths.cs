namespace GithubActors
{
    /// <summary>
    /// Static helper class used to define paths to fixed-name actors
    /// (helps eliminate errors when using <see cref="ActorSelection"/>)
    /// </summary>
    static class ActorPaths
    {
        #region Github

        public static readonly string Auth = $"akka://{ActorNames.Github}/user/{ActorNames.Auth}";
        public static readonly string RepoValidator = $"akka://{ActorNames.Github}/user/{ActorNames.RepoValidator}";
        public static readonly string GithubCommander = $"akka://{ActorNames.Github}/user/{ActorNames.GithubCommander}";
        public static readonly string GithubCoordinator = $"akka://{ActorNames.Github}/user/{ActorNames.GithubCoordinator}";

        #endregion Github

        #region UI

        public static readonly string Initializer = $"akka://{ActorNames.UI}/user/{ActorNames.Initializer}";
        
        public static readonly string PageNavigator = $"akka://{ActorNames.UI}/user/{ActorNames.PageNavigator}";
        public static readonly string PageTitle = $"akka://{ActorNames.UI}/user/{ActorNames.PageTitle}";

        public static readonly string AuthStatusCooridnator = $"akka://{ActorNames.UI}/user/{ActorNames.AuthStatusCooridnator}";
        public static readonly string AuthStatus = $"akka://{ActorNames.UI}/user/{ActorNames.AuthStatus}";
        public static readonly string AuthBusy = $"akka://{ActorNames.UI}/user/{ActorNames.AuthBusy}";

        public static readonly string RepoLauncher = $"akka://{ActorNames.UI}/user/{ActorNames.RepoLauncher}";
        public static readonly string RepoStatusCoordinator = $"akka://{ActorNames.UI}/user/{ActorNames.RepoStatusCoordinator}";
        public static readonly string RepoBusy = $"akka://{ActorNames.UI}/user/{ActorNames.RepoBusy}";
        public static readonly string RepoStatus = $"akka://{ActorNames.UI}/user/{ActorNames.RepoStatus}";
        public static readonly string RepoCoordinator = $"akka://{ActorNames.UI}/user/{ActorNames.RepoCoordinator}";
        public static readonly string RepoProgress = $"akka://{ActorNames.UI}/user/{ActorNames.RepoProgress}";

        public static readonly string MainForm = $"akka://{ActorNames.UI}/user/{ActorNames.MainForm}";

        public static readonly string RepoResultsPresenter = $"akka://{ActorNames.UI}/user/{ActorNames.RepoResultsPresenter}";
        public static readonly string RepoResults = $"akka://{ActorNames.UI}/user/{ActorNames.RepoResults}";

        #endregion UI
    }
}

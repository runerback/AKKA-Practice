﻿namespace GithubActors
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

        #endregion Github

        #region UI

        public static readonly string Initializer = $"akka://{ActorNames.UI}/user/{ActorNames.Initializer}";
        
        public static readonly string PageNavigator = $"akka://{ActorNames.UI}/user/{ActorNames.PageNavigator}";
        public static readonly string PageTitle = $"akka://{ActorNames.UI}/user/{ActorNames.PageTitle}";

        public static readonly string AuthStatusCooridnator = $"akka://{ActorNames.UI}/user/{ActorNames.AuthStatusCooridnator}";
        public static readonly string AuthStatus = $"akka://{ActorNames.UI}/user/{ActorNames.AuthStatus}";
        public static readonly string AuthBusy = $"akka://{ActorNames.UI}/user/{ActorNames.AuthBusy}";

        public static readonly string RepoLauncher = $"akka://{ActorNames.UI}/user/{ActorNames.RepoLauncher}";
        public static readonly string RepoValidateCoordinator = $"akka://{ActorNames.UI}/user/{ActorNames.RepoValidateCoordinator}";
        public static readonly string RepoValidateBusy = $"akka://{ActorNames.UI}/user/{ActorNames.RepoValidateBusy}";
        public static readonly string RepoValidateStatus = $"akka://{ActorNames.UI}/user/{ActorNames.RepoValidateStatus}";
        public static readonly string RepoCoordinator = $"akka://{ActorNames.UI}/user/{ActorNames.RepoCoordinator}";
        public static readonly string RepoProgress = $"akka://{ActorNames.UI}/user/{ActorNames.RepoProgress}";

        public static readonly string RepoResultsPresenter = $"akka://{ActorNames.UI}/user/{ActorNames.RepoResultsPresenter}";
        public static readonly string RepoResults = $"akka://{ActorNames.UI}/user/{ActorNames.RepoResults}";

        #endregion UI
    }
}

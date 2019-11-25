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
        public static readonly string AuthStatusPresent = $"akka://{ActorNames.Github}/user/{ActorNames.AuthStatusPresent}";

        #endregion Github

        #region UI

        public static readonly string Initializer = $"akka://{ActorNames.UI}/user/{ActorNames.Initializer}";
        
        public static readonly string PageNavigator = $"akka://{ActorNames.UI}/user/{ActorNames.PageNavigator}";
        public static readonly string PageTitle = $"akka://{ActorNames.UI}/user/{ActorNames.PageTitle}";
        public static readonly string AuthStatus = $"akka://{ActorNames.UI}/user/{ActorNames.AuthStatus}";
        public static readonly string AuthBusy = $"akka://{ActorNames.UI}/user/{ActorNames.AuthBusy}";
        public static readonly string RepoValidateBusy = $"akka://{ActorNames.UI}/user/{ActorNames.RepoValidateBusy}";

        #endregion UI
    }
}

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
        public static readonly string GithubCoordinator = $"akka://{ActorNames.Github}/user/{ActorNames.GithubCommander}/{ActorNames.GithubCoordinator}";
        public static readonly string GithubWorker = $"akka://{ActorNames.Github}/user/{ActorNames.GithubCommander}/{ActorNames.GithubCoordinator}/{ActorNames.GithubWorker}*";

        #endregion Github

        #region UI

        public static readonly string DispatcherCoordinator = $"akka://{ActorNames.UI}/user/{ActorNames.DispatcherCoordinator}";
        public static readonly string PageNavigator = $"akka://{ActorNames.UI}/user/{ActorNames.DispatcherCoordinator}/{ActorNames.PageNavigator}";
        public static readonly string DispatcherCommandNotifier = $"akka://{ActorNames.UI}/user/{ActorNames.DispatcherCoordinator}/{ActorNames.DispatcherCommandNotifier}";

        public static readonly string Initializer = $"akka://{ActorNames.UI}/user/{ActorNames.Initializer}";

        public static readonly string PageTitle = $"akka://{ActorNames.UI}/user/{ActorNames.PageTitle}";

        public static readonly string AuthStatusCooridnator = $"akka://{ActorNames.UI}/user/{ActorNames.AuthStatusCooridnator}";
        public static readonly string AuthStatus = $"akka://{ActorNames.UI}/user/{ActorNames.AuthStatusCooridnator}/{ActorNames.AuthStatus}";
        public static readonly string AuthBusy = $"akka://{ActorNames.UI}/user/{ActorNames.AuthStatusCooridnator}/{ActorNames.AuthBusy}";

        public static readonly string RepoLauncher = $"akka://{ActorNames.UI}/user/{ActorNames.RepoLauncher}";

        public static readonly string RepoStatusCoordinator = $"akka://{ActorNames.UI}/user/{ActorNames.RepoStatusCoordinator}";
        public static readonly string RepoBusy = $"akka://{ActorNames.UI}/user/{ActorNames.RepoStatusCoordinator}/{ActorNames.RepoBusy}";
        public static readonly string RepoStatus = $"akka://{ActorNames.UI}/user/{ActorNames.RepoStatusCoordinator}/{ActorNames.RepoStatus}";

        public static readonly string MainForm = $"akka://{ActorNames.UI}/user/{ActorNames.RepoLauncher}/{ActorNames.MainForm}";

        public static readonly string RepoResultsPresenter = $"akka://{ActorNames.UI}/user/{ActorNames.RepoLauncher}/{ActorNames.MainForm}/{ActorNames.RepoResultsPresenter}";

        public static readonly string RepoResultsCoordinator = $"akka://{ActorNames.UI}/user/{ActorNames.RepoResultsCoordinator}";
        public static readonly string RepoProgress = $"akka://{ActorNames.UI}/user/{ActorNames.RepoResultsCoordinator}/{ActorNames.RepoProgress}";
        public static readonly string RepoResults = $"akka://{ActorNames.UI}/user/{ActorNames.RepoResultsCoordinator}/{ActorNames.RepoResults}";

        #endregion UI

        public static void PrintAll()
        {
            System.Console.WriteLine(Auth);
            System.Console.WriteLine(RepoValidator);
            System.Console.WriteLine(GithubCommander);
            System.Console.WriteLine(GithubCoordinator);
            System.Console.WriteLine(GithubWorker);

            System.Console.WriteLine(DispatcherCoordinator);
            System.Console.WriteLine(PageNavigator);
            System.Console.WriteLine(DispatcherCommandNotifier);

            System.Console.WriteLine(Initializer);
            System.Console.WriteLine(PageTitle);
            System.Console.WriteLine(AuthStatusCooridnator);
            System.Console.WriteLine(AuthStatus);
            System.Console.WriteLine(AuthBusy);

            System.Console.WriteLine(RepoLauncher);
            System.Console.WriteLine(RepoStatusCoordinator);
            System.Console.WriteLine(RepoBusy);
            System.Console.WriteLine(RepoStatus);
            System.Console.WriteLine(MainForm);

            System.Console.WriteLine(RepoResultsPresenter);
            System.Console.WriteLine(RepoResultsCoordinator);
            System.Console.WriteLine(RepoProgress);
            System.Console.WriteLine(RepoResults);
        }
    }
}

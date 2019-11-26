using Akka.Actor;
using GithubActors.Messages;
using Octokit;
using System;
using System.Linq;

namespace GithubActors.Actors
{
    /// <summary>
    /// Individual actor responsible for querying the Github API
    /// </summary>
    sealed class GithubWorkerActor : ReceiveActor
    {
        private IGitHubClient _gitHubClient;
        private readonly Func<IGitHubClient> _gitHubClientFactory;

        public GithubWorkerActor(Func<IGitHubClient> gitHubClientFactory)
        {
            ActorPathPrinter.Print(Self);

            _gitHubClientFactory = gitHubClientFactory;
            InitialReceives();
        }

        protected override void PreStart()
        {
            _gitHubClient = _gitHubClientFactory();
        }

        private void InitialReceives()
        {
            //query an individual starrer
            Receive<RetryableQuery>(query => query.Query is QueryStarrer, query =>
            {
                // ReSharper disable once PossibleNullReferenceException (we know from the previous IS statement that this is not null)
                var starrer = (query.Query as QueryStarrer).Login;
                try
                {
                    var getStarrer = _gitHubClient.Activity.Starring.GetAllForUser(starrer);
                    
                    var starredRepos = getStarrer.Result;

                    Sender.Tell(new StarredReposForUser(starrer, starredRepos));
                }
                catch
                {
                    //operation failed - let the parent know
                    Sender.Tell(query.NextTry());
                }
            });

            //query all starrers for a repository
            Receive<RetryableQuery>(
                query => query.Query is QueryStarrers,
                query =>
                {
                    // ReSharper disable once PossibleNullReferenceException (we know from the previous IS statement that this is not null)
                    var starrers = query.Query as QueryStarrers;
                    try
                    {
                        var getStars = _gitHubClient.Activity.Starring.GetAllStargazers(starrers.Owner, starrers.Repo);

                        Sender.Tell(getStars.Result.ToArray());
                    }
                    catch
                    {
                        //operation failed - let the parent know
                        Sender.Tell(query.NextTry());
                    }
                });
        }
    }
}

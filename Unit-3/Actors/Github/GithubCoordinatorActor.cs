using Akka.Actor;
using GithubActors.Messages;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GithubActors.Actors
{
    /// <summary>
    /// Actor responsible for publishing data about the results
    /// of a github operation
    /// </summary>
    sealed class GithubCoordinatorActor : ReceiveActor
    {
        private readonly Func<IGitHubClient> gitHubClientFactory;

        private IActorRef githubWorker;

        private RepoKey _currentRepo;
        private Dictionary<string, SimilarRepo> _similarRepos;
        private HashSet<IActorRef> _subscribers;
        private ICancelable _publishTimer;
        private GithubProgressStats _githubProgressStats;

        private bool _receivedInitialUsers = false;

        public GithubCoordinatorActor(Func<IGitHubClient> gitHubClientFactory)
        {
            ActorPathPrinter.Print(Self);

            this.gitHubClientFactory = gitHubClientFactory ??
                throw new ArgumentNullException(nameof(gitHubClientFactory));

            Waiting();
        }

        protected override void PreStart()
        {
            githubWorker = Context.ActorOf(
                Props.Create<GithubWorkerActor>(gitHubClientFactory),
                ActorNames.GithubWorker);
        }

        private void Waiting()
        {
            Receive<CanAcceptJob>(job => Sender.Tell(new AbleToAcceptJob(job)));
            Receive<BeginJob>(job =>
            {
                BecomeWorking(job);

                //kick off the job to query the repo's list of starrers
                githubWorker.Tell(new RetryableQuery(new QueryStarrers(job), 4));
            });
        }

        private void BecomeWorking(RepoKey repo)
        {
            _receivedInitialUsers = false;
            _currentRepo = repo;
            _subscribers = new HashSet<IActorRef>();
            _similarRepos = new Dictionary<string, SimilarRepo>();
            _publishTimer = new Cancelable(Context.System.Scheduler);
            _githubProgressStats = new GithubProgressStats();

            Become(Working);
        }

        private void Working()
        {
            //received a downloaded user back from the github worker
            Receive<StarredReposForUser>(user =>
            {
                _githubProgressStats = _githubProgressStats.UserQueriesFinished();
                foreach (var repo in user.Repos)
                {
                    if (!_similarRepos.ContainsKey(repo.HtmlUrl))
                    {
                        _similarRepos[repo.HtmlUrl] = new SimilarRepo(repo);
                    }

                    //increment the number of people who starred this repo
                    _similarRepos[repo.HtmlUrl].SharedStarrers++;
                }
            });

            Receive<PublishUpdate>(_ =>
            {
                //check to see if the job is done
                if (_receivedInitialUsers && _githubProgressStats.IsFinished)
                {
                    _githubProgressStats = _githubProgressStats.Finish();

                    //all repos minus forks of the current one
                    var sortedSimilarRepos = _similarRepos
                        .Values
                        .Where(x => x.Repo.Name != _currentRepo.Repo)
                        .OrderByDescending(x => x.SharedStarrers)
                        .ToArray();
                    foreach (var subscriber in _subscribers)
                    {
                        subscriber.Tell(sortedSimilarRepos);
                    }
                    BecomeWaiting();
                }

                foreach (var subscriber in _subscribers)
                {
                    subscriber.Tell(_githubProgressStats);
                }
            });

            //completed our initial job - we now know how many users we need to query
            Receive<IEnumerable<User>>(
                users => users?.Any() ?? false,
                users =>
                {
                    _receivedInitialUsers = true;
                    _githubProgressStats = _githubProgressStats.SetExpectedUserCount(users.Count());

                    //queue up all of the jobs
                    foreach (var user in users)
                        githubWorker.Tell(new RetryableQuery(new QueryStarrer(user.Login), 3));
                });

            Receive<CanAcceptJob>(job => Sender.Tell(new UnableToAcceptJob(job)));

            Receive<SubscribeToProgressUpdates>(updates =>
            {
                //this is our first subscriber, which means we need to turn publishing on
                if (_subscribers.Count == 0)
                {
                    Context.System.Scheduler.ScheduleTellRepeatedly(
                        TimeSpan.FromMilliseconds(1000),
                        TimeSpan.FromMilliseconds(1000),
                        Self,
                        PublishUpdate.Instance,
                        Self,
                        _publishTimer);
                }

                _subscribers.Add(updates.Subscriber);
            });

            ReceiveQuery();
        }

        private void ReceiveQuery()
        {
            //query failed, but can be retried
            Receive<RetryableQuery>(
                query => query.CanRetry,
                query => githubWorker.Tell(query));

            //query failed, can't be retried, and it's a QueryStarrers operation - means the entire job failed
            Receive<RetryableQuery>(
                query => !query.CanRetry && query.Query is QueryStarrers,
                query =>
                {
                    _receivedInitialUsers = true;
                    foreach (var subscriber in _subscribers)
                    {
                        subscriber.Tell(new JobFailed(_currentRepo));
                    }
                    BecomeWaiting();
                });

            //query failed, can't be retried, and it's a QueryStarrers operation - means individual operation failed
            Receive<RetryableQuery>(
                query => !query.CanRetry && query.Query is QueryStarrer,
                query => _githubProgressStats.IncrementFailures());
        }

        private void BecomeWaiting()
        {
            //stop publishing
            _publishTimer.Cancel();

            Become(Waiting);
        }
    }
}

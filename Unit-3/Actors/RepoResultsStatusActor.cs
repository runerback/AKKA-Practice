using Akka.Actor;
using GithubActors.Messages;
using GithubActors.Model;
using System;

namespace GithubActors.Actors
{
    sealed class RepoResultsStatusActor : ReceiveActor
    {
        private readonly Func<RepoRequestProgress, RepoRequestProgress> updateProgress;
        private readonly Action<string> updateStatus;

        private bool progressInitialized = false;

        public RepoResultsStatusActor(Func<RepoRequestProgress, RepoRequestProgress> updateProgress, Action<string> updateStatus)
        {
            this.updateProgress = updateProgress;
            this.updateStatus = updateStatus;

            //progress update
            Receive<GithubProgressStats>(
                stats => stats.ExpectedUsers > 0,
                stats =>
                {
                    if (!progressInitialized)
                    {
                        this.updateProgress(new RepoRequestProgress(stats.ExpectedUsers, stats.UsersThusFar));
                        progressInitialized = true;
                    }
                    else
                    {
                        this.updateProgress(null).Current = stats.UsersThusFar + stats.QueryFailures;
                    }

                    this.updateStatus($"{stats.UsersThusFar} out of {stats.ExpectedUsers} users " +
                        $"({stats.QueryFailures} failures) [{stats.Elapsed} elapsed]");
                });

            //critical failure, like not being able to connect to Github
            Receive<JobFailed>(failed =>
            {
                if (!progressInitialized)
                {
                    var progress = new RepoRequestProgress(1, 1);
                    progress.Fail();

                    this.updateProgress(progress);
                    progressInitialized = true;
                }
                else
                {
                    this.updateProgress(null).Fail();
                }

                this.updateStatus($"Failed to gather data for Github repository {failed.Owner} / {failed.Repo}");
            });
        }
    }
}

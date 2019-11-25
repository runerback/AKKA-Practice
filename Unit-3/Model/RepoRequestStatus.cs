using System;

namespace GithubActors.Model
{
    sealed class RepoRequestStatus
    {
        public RepoRequestStatus(int failures, TimeSpan elapsed)
        {
            Failures = failures;
            Elapsed = elapsed;
        }

        public int Failures { get; }
        public TimeSpan Elapsed { get; }
    }
}

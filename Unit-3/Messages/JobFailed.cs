namespace GithubActors.Messages
{
    sealed class JobFailed : RepoKey
    {
        public JobFailed(string repoName, string owner) : base(repoName, owner)
        {
        }

        public JobFailed(RepoKey other) : base(other)
        {
        }
    }
}

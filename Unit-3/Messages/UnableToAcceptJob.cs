namespace GithubActors.Messages
{
    sealed class UnableToAcceptJob : RepoKey
    {
        public UnableToAcceptJob(string repoName, string owner) : base(repoName, owner)
        {
        }

        public UnableToAcceptJob(RepoKey other) : base(other)
        {
        }
    }
}

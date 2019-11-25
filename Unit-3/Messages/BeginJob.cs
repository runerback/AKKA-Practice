namespace GithubActors.Messages
{
    sealed class BeginJob : RepoKey
    {
        public BeginJob(string repoName, string owner) : base(repoName, owner)
        {
        }

        public BeginJob(RepoKey other) : base(other)
        {
        }
    }
}

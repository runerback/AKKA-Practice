namespace GithubActors.Messages
{
    sealed class CanAcceptJob : RepoKey
    {
        public CanAcceptJob(string repoName, string owner) : base(repoName, owner)
        {
        }
    }
}

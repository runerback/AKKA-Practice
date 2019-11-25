namespace GithubActors.Messages
{
    sealed class AbleToAcceptJob : RepoKey
    {
        public AbleToAcceptJob(string repoName, string owner) : base(repoName, owner)
        {
        }
    }
}

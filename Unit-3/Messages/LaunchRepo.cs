namespace GithubActors.Messages
{
    sealed class LaunchRepo : RepoKey
    {
        public LaunchRepo(string repoName, string owner) : base(repoName, owner)
        {
        }
    }
}

namespace GithubActors.Messages
{
    abstract class RepoAddress
    {
        public RepoAddress(string repoUri)
        {
            URL = repoUri;
        }

        public string URL { get; }
    }
}

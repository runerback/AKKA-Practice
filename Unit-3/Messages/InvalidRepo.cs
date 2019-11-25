namespace GithubActors.Messages
{
    sealed class InvalidRepo : RepoAddress
    {
        public InvalidRepo(string repoUri, string reason) : base(repoUri)
        {
            Reason = reason;
        }

        public string Reason { get; }
    }
}

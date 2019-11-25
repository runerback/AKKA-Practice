namespace GithubActors.Messages
{
    sealed class ValidRepo : RepoAddress
    {
        public ValidRepo(string repoUri) : base(repoUri)
        {
        }
    }
}

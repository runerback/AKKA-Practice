namespace GithubActors.Messages
{
    sealed class ValidateRepo : RepoAddress
    {
        public ValidateRepo(string repoUri) : base(repoUri)
        {
        }
    }
}

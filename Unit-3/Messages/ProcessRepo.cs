namespace GithubActors.Messages
{
    /// <summary>
    /// Begin processing a new Github repository for analysis
    /// </summary>
    sealed class ProcessRepo : RepoAddress
    {
        public ProcessRepo(string repoUri) : base(repoUri)
        {
        }
    }
}

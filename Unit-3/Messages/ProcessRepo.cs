namespace GithubActors.Messages
{
    /// <summary>
    /// Begin processing a new Github repository for analysis
    /// </summary>
    sealed class ProcessRepo : RepoKey
    {
        public ProcessRepo(RepoKey other) : base(other)
        {
        }
    }
}

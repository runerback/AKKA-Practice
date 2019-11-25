namespace GithubActors.Messages
{
    sealed class QueryStarrers : RepoKey
    {
        public QueryStarrers(string repoName, string owner) : base(repoName, owner)
        {
        }

        public QueryStarrers(RepoKey other) : base(other)
        {
        }
    }
}

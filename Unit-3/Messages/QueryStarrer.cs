namespace GithubActors.Messages
{
    /// <summary>
    /// Query an individual starrer
    /// </summary>
    sealed class QueryStarrer
    {
        public QueryStarrer(string login)
        {
            Login = login;
        }

        public string Login { get; }
    }
}

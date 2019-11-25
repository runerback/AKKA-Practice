using Octokit;
using System.Collections.Generic;
using System.Linq;

namespace GithubActors.Messages
{
    sealed class StarredReposForUser
    {
        public StarredReposForUser(string login, IEnumerable<Repository> repos)
        {
            Login = login;
            Repos = repos ?? Enumerable.Empty<Repository>();
        }

        public string Login { get; }
        public IEnumerable<Repository> Repos { get; }
    }
}

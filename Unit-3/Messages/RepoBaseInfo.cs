using System;
using System.Linq;

namespace GithubActors.Messages
{
    class RepoKey
    {
        public RepoKey(string repo, string owner)
        {
            Repo = repo;
            Owner = owner;
        }

        public RepoKey(RepoKey other) : this(other.Repo, other.Owner)
        {
        }

        public string Repo { get; }
        public string Owner { get; }

        public override string ToString()
        {
            return $"{Owner}/{Repo}";
        }

        public static RepoKey FromAddress(RepoAddress address)
        {
            //uri path without trailing slash
            var split = new Uri(address.URL, UriKind.Absolute)
                .PathAndQuery
                .TrimEnd('/')
                .Split('/')
                .Reverse()
                .Take(2)
                .ToArray();

            return new RepoKey(split[0], split[1]);
        }
    }
}

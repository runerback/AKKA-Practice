using Octokit;
using Octokit.Internal;
using System;

namespace GithubActors
{
    /// <summary>
    /// Creates <see cref="GitHubClient"/> instances.
    /// </summary>
    sealed class GithubClientFactory
    {
        public static GitHubClient GetClient(string token)
        {
            return new GitHubClient(
                new ProductHeaderValue("AkkaBootcamp-Unit3"),
                new InMemoryCredentialStore(new Credentials(token)));
        }

        //use this way to create anonymous factory class
        public static Func<GitHubClient> GetClientFactory(string token)
        {
            var _token = token;
            return () => GetClient(_token);
        }
    }
}

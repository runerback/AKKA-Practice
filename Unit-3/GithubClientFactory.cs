using Octokit;
using Octokit.Internal;

namespace GithubActors
{
    /// <summary>
    /// Creates <see cref="GitHubClient"/> instances.
    /// </summary>
    static class GithubClientFactory
    {
        public static GitHubClient GetClient(string token)
        {
            return new GitHubClient(
                new ProductHeaderValue("AkkaBootcamp-Unit3"),
                new InMemoryCredentialStore(new Credentials(token)));
        }
    }
}

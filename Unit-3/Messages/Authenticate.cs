namespace GithubActors.Messages
{
    sealed class Authenticate
    {
        public Authenticate(string oAuthToken)
        {
            OAuthToken = oAuthToken;
        }

        public string OAuthToken { get; }
    }
}
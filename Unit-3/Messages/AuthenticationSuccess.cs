namespace GithubActors.Messages
{
    sealed class AuthenticationSuccess
    {
        public AuthenticationSuccess(string token)
        {
            Token = token;
        }

        public string Token { get; }
    }
}

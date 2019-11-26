using System.IO;

namespace GithubActors
{
    sealed class AuthTokenStore
    {
        public string Token
        {
            get
            {
                if (!File.Exists("auth.token"))
                    return null;
                return File.ReadAllText("auth.token");
            }
        }
    }
}

using System.Windows.Media;

namespace GithubActors.Messages
{
    sealed class AuthenticateStatus
    {
        public AuthenticateStatus(string status, Color color)
        {
            Status = status;
            Color = color;
        }

        public string Status { get; }
        public Color Color { get; }
    }
}

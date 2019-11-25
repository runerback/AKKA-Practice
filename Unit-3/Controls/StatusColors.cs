using System.Windows.Media;

namespace GithubActors
{
    sealed class StatusColors
    {
        public static readonly string Idle = Colors.Transparent.ToString();
        public static readonly string Querying = Colors.DeepSkyBlue.ToString();
        public static readonly string Succeed = Colors.Green.ToString();
        public static readonly string Failed = Colors.Red.ToString();
    }
}

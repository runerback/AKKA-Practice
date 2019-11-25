namespace GithubActors.Messages
{
    sealed class SystemIdle
    {
        private SystemIdle() { }

        public static readonly SystemIdle Instance = new SystemIdle();
    }
}

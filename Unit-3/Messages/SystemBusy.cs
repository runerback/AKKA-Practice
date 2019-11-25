namespace GithubActors.Messages
{
    sealed class SystemBusy
    {
        private SystemBusy() { }

        public static readonly SystemBusy Instance = new SystemBusy();
    }
}

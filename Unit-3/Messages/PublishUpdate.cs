namespace GithubActors.Messages
{
    sealed class PublishUpdate
    {
        private PublishUpdate() { }

        public static readonly PublishUpdate Instance = new PublishUpdate();
    }
}

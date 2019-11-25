namespace GithubActors.Messages
{
    sealed class PageTitle
    {
        public PageTitle(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}

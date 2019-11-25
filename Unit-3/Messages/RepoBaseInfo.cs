namespace GithubActors.Messages
{
    class RepoKey
    {
        public RepoKey(string repo, string owner)
        {
            Repo = repo;
            Owner = owner;
        }

        public string Repo { get; }
        public string Owner { get; }

        public override string ToString()
        {
            return $"{Owner}/{Repo}";
        }
    }
}

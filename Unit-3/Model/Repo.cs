namespace GithubActors.Model
{
    sealed class Repo //one-time binding
    {
        public Repo(string name, string owner, string url)
        {
            Name = name;
            Owner = owner;
            URL = url;
        }

        public string Owner { get; }
        public string Name { get; }
        public string URL { get; }
        public int SharedStarsCount { get; set; }
        public int OpenIssuesCount { get; set; }
        public int StarsCount { get; set; }
        public int ForksCount { get; set; }
    }
}

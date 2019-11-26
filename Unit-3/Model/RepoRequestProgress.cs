namespace GithubActors.Model
{
    sealed class RepoRequestProgress : ViewModelBase
    {
        private int current = 0;

        public RepoRequestProgress(int total, int current = 0)
        {
            Total = total;
            Empty = total == 0;
            this.current = current;
        }

        public int Total { get; private set; }

        public int Current
        {
            get { return current; }
            set
            {
                if (current != value && !Failed && !Empty)
                {
                    current = value;
                    NotifyPropertyChanged(nameof(Current));
                }
            }
        }

        public bool Failed { get; private set; }
        public bool Empty { get; }

        public void Fail()
        {
            if (!Failed)
            {
                Failed = true;
                NotifyPropertyChanged(nameof(Failed));
            }
        }
    }
}

namespace GithubActors.Model
{
    sealed class RepoRequestProgress : ViewModelBase
    {
        private int current = 0;

        public RepoRequestProgress(int total, int current = 0)
        {
            Total = total;
            this.current = current;
        }

        public int Total { get; private set; }

        public int Current
        {
            get { return current; }
            set
            {
                if (current != value && !Failed)
                {
                    current = value;
                    NotifyPropertyChanged(nameof(Current));
                }
            }
        }

        public bool Failed { get; private set; }

        public void Fail()
        {
            if (!Failed)
            {
                Total = 1;
                NotifyPropertyChanged(nameof(Total));

                Current = 1;

                Failed = true;
                NotifyPropertyChanged(nameof(Failed));
            }
        }
    }
}

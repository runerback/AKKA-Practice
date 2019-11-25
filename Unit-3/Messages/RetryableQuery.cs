namespace GithubActors.Messages
{
    sealed class RetryableQuery
    {
        private RetryableQuery(object query, int allowableTries, int currentAttempt)
        {
            AllowableTries = allowableTries;
            Query = query;
            CurrentAttempt = currentAttempt;
        }

        public RetryableQuery(object query, int allowableTries) : this(query, allowableTries, 0)
        {
        }

        public object Query { get; }
        public int AllowableTries { get; }
        public int CurrentAttempt { get; }

        public bool CanRetry => RemainingTries > 0;

        public int RemainingTries => AllowableTries - CurrentAttempt;

        public RetryableQuery NextTry()
        {
            return new RetryableQuery(Query, AllowableTries, CurrentAttempt + 1);
        }
    }
}

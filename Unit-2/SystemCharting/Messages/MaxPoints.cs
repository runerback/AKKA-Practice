namespace ChartApp.Messages
{
    /// <summary>
    /// Maximum number of points we will allow in a series
    /// </summary>
    sealed class MaxPoints
    {
        private const int VALUE = 250;

        public int Value => VALUE;
    }
}

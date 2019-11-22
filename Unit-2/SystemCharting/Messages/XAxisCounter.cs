namespace ChartApp.Messages
{
    /// <summary>
    /// Incrementing counter we use to plot along the X-axis
    /// </summary>
    sealed class XAxisCounter
    {
        public XAxisCounter(int value = 0)
        {
            Value = value;
        }

        public int Value { get; }
    }
}

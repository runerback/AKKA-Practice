namespace ChartApp.Messages
{
    /// <summary>
    /// Y-axis boundary
    /// </summary>
    sealed class YBoundary
    {
        public YBoundary(double minValue, double maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
        
        public double MinValue { get; }
        public double MaxValue { get; }
    }
}

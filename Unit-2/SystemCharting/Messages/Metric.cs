namespace ChartApp.Messages
{
    /// <summary>
    /// Metric data at the time of sample
    /// </summary>
    sealed class Metric
    {
        public Metric(string series, float counterValue)
        {
            CounterValue = counterValue;
            Series = series;
        }

        public string Series { get; }
        public float CounterValue { get; }

        public override string ToString()
        {
            return $"{Series}: {CounterValue:0.00}";
        }
    }
}

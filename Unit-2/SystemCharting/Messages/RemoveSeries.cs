using System.Windows.Forms.DataVisualization.Charting;

namespace ChartApp.Messages
{
    /// <summary>
    /// Remove an existing <see cref="Series"/> from the chart
    /// </summary>
    sealed class RemoveSeries
    {
        public RemoveSeries(string seriesName)
        {
            SeriesName = seriesName;
        }

        public string SeriesName { get; }
    }
}

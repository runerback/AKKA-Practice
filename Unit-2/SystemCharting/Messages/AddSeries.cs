using System.Windows.Forms.DataVisualization.Charting;

namespace ChartApp.Messages
{
    /// <summary>
    /// Add a new <see cref="Series"/> to the chart
    /// </summary>
    sealed class AddSeries
    {
        public AddSeries(Series series)
        {
            Series = series;
        }

        public Series Series { get; }
    }
}

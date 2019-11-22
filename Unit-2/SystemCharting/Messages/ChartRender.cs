using System.Windows.Forms.DataVisualization.Charting;

namespace ChartApp.Messages
{
    sealed class ChartRender
    {
        public ChartRender(Chart chart, Series series, float counter)
        {
            Chart = chart;
            Series = series;
            Counter = counter;
        }

        public Chart Chart { get; }
        public Series Series { get; }
        public float Counter { get; }
    }
}

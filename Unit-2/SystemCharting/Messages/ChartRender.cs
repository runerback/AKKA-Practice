using System.Windows.Forms.DataVisualization.Charting;

namespace ChartApp.Messages
{
    sealed class ChartRender
    {
        public ChartRender(Series series, double counter)
        {
            Series = series;
            Counter = counter;
        }
        
        public Series Series { get; }
        public double Counter { get; }
    }
}

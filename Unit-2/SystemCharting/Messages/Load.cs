using System;
using System.Windows.Forms.DataVisualization.Charting;

namespace ChartApp.Messages
{
    sealed class Load
    {
        public Load(Chart chart)
        {
            Chart = chart ?? throw new ArgumentNullException(nameof(chart));
        }

        public Chart Chart { get; }
    }
}

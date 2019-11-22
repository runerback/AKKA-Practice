using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace ChartApp.Messages
{
    sealed class InitializeChart
    {
        public InitializeChart(params Series[] initialSeries)
        {
            InitialSeries = initialSeries.ToDictionary(it => it.Name, it => it);
        }

        public IDictionary<string, Series> InitialSeries { get; }
    }
}

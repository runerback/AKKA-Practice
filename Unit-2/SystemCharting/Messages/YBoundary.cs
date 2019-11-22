using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace ChartApp.Messages
{
    /// <summary>
    /// Indicate Y boundary may changed.
    /// </summary>
    sealed class YBoundary
    {
        public YBoundary(IEnumerable<Series> series)
        {
            Series = series ?? Enumerable.Empty<Series>();
        }

        public IEnumerable<Series> Series { get; }
    }
}

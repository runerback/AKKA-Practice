using Akka.Actor;
using ChartApp.Messages;
using System;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace ChartApp.Actors
{
    sealed class ChartBoundariesActor : ReceiveActor
    {
        private readonly Chart _chart;

        public ChartBoundariesActor(Chart chart)
        {
            _chart = chart;

            Receive<MaxPoints>(maxPoints => SetMinXAxis(maxPoints));
            Receive<XAxisCounter>(counter => SetMaxXAxis(counter));
            Receive<YBoundary>(message => SetChartYBoundaries(message));
        }

        private void SetMaxXAxis(XAxisCounter counter)
        {
            if (_chart.TryGetFirstArea(out ChartArea area))
                area.AxisX.Maximum = counter.Value;
        }

        private void SetMinXAxis(MaxPoints maxPoints)
        {
            if (_chart.TryGetFirstArea(out ChartArea area))
                area.AxisX.Minimum = Math.Max(0, area.AxisX.Maximum - maxPoints.Value);
        }

        private void SetChartYBoundaries(YBoundary boundary)
        {
            if (_chart.TryGetFirstArea(out ChartArea area))
            {
                area.AxisY.Minimum = Math.Floor(boundary.MinValue);
                area.AxisY.Maximum = Math.Ceiling(boundary.MaxValue);
            }
        }
    }
}

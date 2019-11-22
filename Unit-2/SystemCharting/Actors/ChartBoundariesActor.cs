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
            {
                try
                {
                    area.AxisX.Maximum = counter.Value;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{Self.Path}XAxisCounter: {ex}");
                }
            }
        }

        private void SetMinXAxis(MaxPoints maxPoints)
        {
            if (_chart.TryGetFirstArea(out ChartArea area))
            {
                try
                {
                    area.AxisX.Minimum = area.AxisX.Maximum - maxPoints.Value;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{Self.Path}MaxPoints: {ex}");
                }
            }
        }

        private void SetChartYBoundaries(YBoundary message)
        {
            var allPoints = message.Series.SelectMany(series => series.Points).ToArray();
            if (allPoints.Length > 2)
            {
                var yValues = allPoints.SelectMany(point => point.YValues).ToArray();

                if (_chart.TryGetFirstArea(out ChartArea area))
                {
                    try
                    {
                        area.AxisY.Minimum = yValues.Length > 0 ? Math.Floor(yValues.Min()) : 0.0d;
                        area.AxisY.Maximum = yValues.Length > 0 ? Math.Ceiling(yValues.Max()) : 1.0d;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{Self.Path}YBoundary: {ex}");
                    }
                }
            }
        }
    }
}

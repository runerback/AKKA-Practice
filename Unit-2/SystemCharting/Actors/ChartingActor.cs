using Akka.Actor;
using ChartApp.Messages;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;

namespace ChartApp.Actors
{
    sealed class ChartingActor : ReceiveActor
    {
        private readonly Chart _chart;
        private readonly IActorRef _chartBoundariesActor;
        private readonly IActorRef chartRenderActor;

        private readonly MaxPoints maxPoints = new MaxPoints();

        private IDictionary<string, Series> _seriesIndex = new Dictionary<string, Series>();

        private int xPosCounter = 0;

        public ChartingActor(Chart chart)
        {
            _chart = chart;

            _chartBoundariesActor = Context.ActorOf(
                Props.Create<ChartBoundariesActor>(_chart)
                    .WithDispatcher("akka.actor.synchronized-dispatcher"),
                "chartBoundaries");

            chartRenderActor = Context.ActorOf(
                Props.Create<ChartRenderActor>()
                    .WithDispatcher("akka.actor.synchronized-dispatcher"),
                "chartRender");

            Charting();
        }

        private void Charting()
        {
            Receive<InitializeChart>(ic => HandleInitialize(ic));
            Receive<AddSeries>(
                addSeries =>
                    !string.IsNullOrEmpty(addSeries.Series.Name) &&
                    !_seriesIndex.ContainsKey(addSeries.Series.Name),
                addSeries => HandleAddSeries(addSeries));
            Receive<RemoveSeries>(
                series =>
                    !string.IsNullOrEmpty(series.SeriesName) &&
                    _seriesIndex.ContainsKey(series.SeriesName),
                series => HandleRemoveSeries(series));
            Receive<Metric>(
                metric => !string.IsNullOrEmpty(metric.Series),
                metric => HandleMetrics(metric, false));

            //new receive handler for the TogglePause message type
            Receive<TogglePause>(pause =>
            {
                BecomeStacked(Paused);
            });
        }

        private void Paused()
        {
            Receive<Metric>(
                metric => !string.IsNullOrEmpty(metric.Series),
                metric => HandleMetrics(metric, true));
            Receive<TogglePause>(pause =>
            {
                UnbecomeStacked();
            });
        }

        #region Individual Message Type Handlers

        private void HandleInitialize(InitializeChart ic)
        {
            if (ic.InitialSeries != null)
            {
                //swap the two series out
                _seriesIndex = ic.InitialSeries;
            }

            //delete any existing series
            _chart.Series.Clear();

            // set the axes up
            var area = _chart.ChartAreas[0];
            area.AxisX.IntervalType = DateTimeIntervalType.Number;
            area.AxisY.IntervalType = DateTimeIntervalType.Number;

            //attempt to render the initial chart
            if (_seriesIndex.Any())
            {
                foreach (var series in _seriesIndex)
                {
                    //force both the chart and the internal index to use the same names
                    series.Value.Name = series.Key;
                    _chart.Series.Add(series.Value);
                }
            }

            var xAxisCounter = new XAxisCounter();

            _chartBoundariesActor.Tell(maxPoints);
            _chartBoundariesActor.Tell(xAxisCounter);
            SetYBoundary();

            chartRenderActor.Tell(maxPoints);
            chartRenderActor.Tell(xAxisCounter);
        }

        private void HandleAddSeries(AddSeries series)
        {
            _seriesIndex.Add(series.Series.Name, series.Series);
            _chart.Series.Add(series.Series);

            SetYBoundary();
        }

        private void HandleRemoveSeries(RemoveSeries series)
        {
            var seriesToRemove = _seriesIndex[series.SeriesName];

            _seriesIndex.Remove(series.SeriesName);
            _chart.Series.Remove(seriesToRemove);

            SetYBoundary();
        }

        private void HandleMetrics(Metric metric, bool paused)
        {
            if (_seriesIndex.TryGetValue(metric.Series, out Series series))
            {
                chartRenderActor.Tell(
                    new ChartRender(series, paused ? 0 : metric.CounterValue));
            }

            var xAxisCounter = new XAxisCounter(xPosCounter++);

            _chartBoundariesActor.Tell(xAxisCounter);
            _chartBoundariesActor.Tell(maxPoints);
            SetYBoundary();

            chartRenderActor.Tell(xAxisCounter);
            chartRenderActor.Tell(maxPoints);
        }

        private void SetYBoundary()
        {
            var allPoints = _seriesIndex.Values
                .SelectMany(series => series?.Points ?? Enumerable.Empty<DataPoint>())
                .ToArray();
            if (allPoints.Length < 3)
                return;

            var yValues = allPoints.SelectMany(point => point.YValues).ToArray();
            if (yValues.Length < 3)
            {
                _chartBoundariesActor.Tell(new YBoundary(0, 1));
                return;
            }

            double min = double.MaxValue, max = double.MinValue;

            foreach (var value in yValues)
            {
                if (value > max)
                    max = value;

                if (value < min)
                    min = value;
            }

            _chartBoundariesActor.Tell(new YBoundary(min, max));
        }

        #endregion
    }
}

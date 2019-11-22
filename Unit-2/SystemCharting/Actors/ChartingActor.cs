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

        private IDictionary<string, Series> _seriesIndex = new Dictionary<string, Series>();

        private int xPosCounter = 0;

        public ChartingActor(Chart chart)
        {
            _chart = chart;

            _chartBoundariesActor = Context.ActorOf(
                Props.Create<ChartBoundariesActor>(_chart)
                    .WithDispatcher("akka.actor.synchronized-dispatcher"));

            chartRenderActor = Program.ChartActors.ActorOf(
                Props.Create<ChartRenderActor>()
                    .WithDispatcher("akka.actor.synchronized-dispatcher"),
                "chartRender");

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
            Receive<Metric>(metric => HandleMetrics(metric));
            Receive<PoisonPill>(thePill => TakePoisonPill(thePill));
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

            _chartBoundariesActor.Tell(new MaxPoints());
            _chartBoundariesActor.Tell(new XAxisCounter());
            UpdateYBoundaries();
        }

        private void HandleAddSeries(AddSeries series)
        {
            _seriesIndex.Add(series.Series.Name, series.Series);
            _chart.Series.Add(series.Series);

            UpdateYBoundaries();
        }

        private void HandleRemoveSeries(RemoveSeries series)
        {
            var seriesToRemove = _seriesIndex[series.SeriesName];

            _seriesIndex.Remove(series.SeriesName);
            _chart.Series.Remove(seriesToRemove);

            UpdateYBoundaries();
        }

        private void HandleMetrics(Metric metric)
        {
            _chartBoundariesActor.Tell(new XAxisCounter(xPosCounter++));
            _chartBoundariesActor.Tell(metric);
            UpdateYBoundaries();

            var series = _seriesIndex[metric.Series];
            if (series != null)
            {
                chartRenderActor.Tell(
                    new ChartRender(_chart, series, metric.CounterValue));
            }
        }

        private void UpdateYBoundaries()
        {
            _chartBoundariesActor.Tell(new YBoundary(_seriesIndex.Values.ToArray()));
        }

        #endregion

        private void TakePoisonPill(PoisonPill thePill)
        {
            chartRenderActor.Tell(thePill);
            _chartBoundariesActor.Tell(thePill);
        }
    }
}

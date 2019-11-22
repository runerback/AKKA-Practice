using Akka.Actor;
using ChartApp.Messages;
using System.Windows.Forms.DataVisualization.Charting;

namespace ChartApp.Actors
{
    sealed class ChartRenderActor : ReceiveActor
    {
        public ChartRenderActor()
        {
            Receive<ChartRender>(message => Render(message));
        }

        private void Render(ChartRender message)
        {
            var series = message.Series;

            if (message.Chart.TryGetFirstArea(out ChartArea area))
            {
                var xPosCounter = (int)area.AxisX.Minimum;
                series.Points.AddXY(xPosCounter, message.Counter);

                var maxPoints = (int)area.AxisX.Maximum;
                while (series.Points.Count > maxPoints)
                    series.Points.RemoveAt(0);
            }
        }
    }
}

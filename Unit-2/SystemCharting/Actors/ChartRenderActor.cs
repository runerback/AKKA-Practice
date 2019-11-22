using Akka.Actor;
using ChartApp.Messages;

namespace ChartApp.Actors
{
    sealed class ChartRenderActor : ReceiveActor
    {
        private int maxPointCount = 0;
        private int xAxisPosition = 0;

        public ChartRenderActor()
        {
            Receive<MaxPoints>(maxPoints => maxPointCount = maxPoints.Value);
            Receive<XAxisCounter>(counter => xAxisPosition = counter.Value);

            Receive<ChartRender>(message => Render(message));
        }

        private void Render(ChartRender message)
        {
            var points = message.Series.Points;
            if (points == null)
                return;

            points.AddXY(xAxisPosition, message.Counter);

            while (points.Count > maxPointCount)
                points.RemoveAt(0);
        }
    }
}

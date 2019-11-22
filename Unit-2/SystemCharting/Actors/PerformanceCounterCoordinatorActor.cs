using Akka.Actor;
using ChartApp.Messages;
using System;
using System.Collections.Generic;

namespace ChartApp.Actors
{
    /// <summary>
    /// Actor responsible for translating UI calls into ActorSystem messages
    /// </summary>
    sealed class PerformanceCounterCoordinatorActor : ReceiveActor
    {
        private readonly IDictionary<CounterType, IActorRef> _counterActors =
            new Dictionary<CounterType, IActorRef>();
        private IActorRef _chartingActor;

        public PerformanceCounterCoordinatorActor(IActorRef chartingActor)
        {
            _chartingActor = chartingActor;

            Receive<Watch>(watch => HandleWatch(watch));
            Receive<Unwatch>(unwatch => HandleUnwatch(unwatch));
        }

        private void HandleWatch(Watch watch)
        {
            if (!_counterActors.ContainsKey(watch.CounterType))
            {
                // create a child actor to monitor this counter if
                // one doesn't exist already
                var counterActor = Context.ActorOf(
                    Props.Create<PerformanceCounterActor>(
                        watch.CounterType.ToString(),
                        PerformanceCounterFactoryPool.GetFactory(watch.CounterType)),
                    $"{watch.CounterType}CounterActor");

                // add this counter actor to our index
                _counterActors[watch.CounterType] = counterActor;
            }

            // register this series with the ChartingActor
            _chartingActor.Tell(new AddSeries(CounterSeriesFactory.GetSeries(watch.CounterType)));

            // tell the counter actor to begin publishing its
            // statistics to the _chartingActor
            _counterActors[watch.CounterType].Tell(
                new SubscribeCounter(watch.CounterType,
                _chartingActor));
        }

        private void HandleUnwatch(Unwatch unwatch)
        {
            if (!_counterActors.ContainsKey(unwatch.Counter))
            {
                return; // noop
            }

            // unsubscribe the ChartingActor from receiving any more updates
            _counterActors[unwatch.Counter].Tell(
                new UnsubscribeCounter(unwatch.Counter, _chartingActor));

            // remove this series from the ChartingActor
            _chartingActor.Tell(new RemoveSeries(unwatch.Counter.ToString()));
        }
    }
}

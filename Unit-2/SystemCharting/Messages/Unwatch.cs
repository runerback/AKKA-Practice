using ChartApp.Actors;

namespace ChartApp.Messages
{
    /// <summary>
    /// Unsubscribe the <see cref="ChartingActor"/> to 
    /// updates for <see cref="Counter"/>
    /// </summary>
    sealed class Unwatch
    {
        public Unwatch(CounterType counter)
        {
            Counter = counter;
        }

        public CounterType Counter { get; }
    }
}

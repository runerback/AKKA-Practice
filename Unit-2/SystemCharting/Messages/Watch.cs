using ChartApp.Actors;

namespace ChartApp.Messages
{
    /// <summary>
    /// Subscribe the <see cref="ChartingActor"/> to 
    /// updates for <see cref="Counter"/>.
    /// </summary>
    sealed class Watch
    {
        public Watch(CounterType counter)
        {
            Counter = counter;
        }

        public CounterType Counter { get; }
    }
}

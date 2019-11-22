using ChartApp.Actors;

namespace ChartApp.Messages
{
    /// <summary>
    /// Subscribe the <see cref="ChartingActor"/> to 
    /// updates for <see cref="CounterType"/>.
    /// </summary>
    sealed class Watch
    {
        public Watch(CounterType counterType)
        {
            CounterType = counterType;
        }

        public CounterType CounterType { get; }
    }
}

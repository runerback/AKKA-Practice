using ChartApp.Actors;
using System;
using System.Windows.Forms;

namespace ChartApp.Messages
{
    /// <summary>
    /// Toggles this <see cref="Button"/> on or off and sends an appropriate messages
    /// to the <see cref="PerformanceCounterCoordinatorActor"/>
    /// </summary>
    sealed class ToggleCounter : ToggleButton
    {
        public ToggleCounter(Button button, CounterType counterType) : base(button)
        {
            CounterType = counterType;
        }

        public CounterType CounterType { get; }
    }
}

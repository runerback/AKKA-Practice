using ChartApp.Actors;
using System;
using System.Windows.Forms;

namespace ChartApp.Messages
{
    /// <summary>
    /// Toggles this <see cref="Button"/> on or off and sends an appropriate messages
    /// to the <see cref="PerformanceCounterCoordinatorActor"/>
    /// </summary>
    sealed class Toggle
    {
        public Toggle(Button button, CounterType counterType)
        {
            Button = button ?? throw new ArgumentNullException(nameof(button));
            CounterType = counterType;
        }

        public Button Button { get; }
        public CounterType CounterType { get; }
    }
}

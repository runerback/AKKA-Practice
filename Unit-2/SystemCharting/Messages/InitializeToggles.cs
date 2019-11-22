using Akka.Actor;
using System;
using System.Collections.Generic;

namespace ChartApp.Messages
{
    sealed class InitializeToggles
    {
        public InitializeToggles(IDictionary<CounterType, IActorRef> toggles)
        {
            Toggles = toggles ?? throw new ArgumentNullException(nameof(toggles));
        }

        public IDictionary<CounterType, IActorRef> Toggles { get; }
    }
}

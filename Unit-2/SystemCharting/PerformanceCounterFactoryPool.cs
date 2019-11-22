using System;
using System.Diagnostics;

namespace ChartApp
{
    static class PerformanceCounterFactoryPool
    {
        /// <summary>
        /// Method for generating new instances of all <see cref="PerformanceCounter"/>s
        /// we want to monitor
        /// </summary>
        public static Func<PerformanceCounter> GetFactory(CounterType counterType)
        {
            switch (counterType)
            {
                case CounterType.Cpu:
                    return () => new PerformanceCounter(
                        "Processor",
                        "% Processor Time",
                        "_Total",
                        true);
                case CounterType.Disk:
                    return () => new PerformanceCounter(
                        "LogicalDisk",
                        "% Disk Time",
                        "_Total",
                        true);
                case CounterType.Memory:
                    return () => new PerformanceCounter(
                        "Memory",
                        "% Committed Bytes In Use",
                        true);
                default:
                    throw new NotImplementedException(counterType.ToString());
            }
        }
    }
}

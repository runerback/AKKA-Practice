using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace ChartApp
{
    static class CounterSeriesFactory
    {
        /// <summary>
        /// Methods for creating new <see cref="Series"/> with distinct colors and names
		/// corresponding to each <see cref="PerformanceCounter"/>
        /// </summary>
        public static Series GetSeries(CounterType counterType)
        {
            switch (counterType)
            {
                case CounterType.Cpu:
                    return new Series(CounterType.Cpu.ToString())
                    {
                        ChartType = SeriesChartType.SplineArea,
                        Color = Color.DarkGreen
                    };
                case CounterType.Disk:
                    return new Series(CounterType.Disk.ToString())
                    {
                        ChartType = SeriesChartType.SplineArea,
                        Color = Color.DarkRed
                    };
                case CounterType.Memory:
                    return new Series(CounterType.Memory.ToString())
                    {
                        ChartType = SeriesChartType.FastLine,
                        Color = Color.MediumBlue
                    };
                default:
                    throw new NotImplementedException(counterType.ToString());
            }
        }
    }
}

using System.Windows.Forms.DataVisualization.Charting;

namespace ChartApp
{
    static class ChartAreaHelper
    {
        public static bool TryGetFirstArea(this Chart chart, out ChartArea area)
        {
            area = null;

            if (chart == null)
                return false;

            try
            {
                area = chart.ChartAreas[0]; //when window is closing, getter of ChartAreas property will throw.
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

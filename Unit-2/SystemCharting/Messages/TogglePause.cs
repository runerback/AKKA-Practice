using System.Windows.Forms;

namespace ChartApp.Messages
{
    /// <summary>
    /// Toggles the pausing between charts
    /// </summary>
    sealed class TogglePause : ToggleButton
    {
        public TogglePause(Button button) : base(button)
        {
        }
    }
}

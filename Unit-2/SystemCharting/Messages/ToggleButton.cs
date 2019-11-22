using System;
using System.Windows.Forms;

namespace ChartApp.Messages
{
    abstract class ToggleButton
    {
        public ToggleButton(Button button)
        {
            Button = button ?? throw new ArgumentNullException(nameof(button));
        }

        public Button Button { get; }
    }
}

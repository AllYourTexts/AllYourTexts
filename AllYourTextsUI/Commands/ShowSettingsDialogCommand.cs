using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AllYourTextsUi.Commands
{
    public class ShowSettingsDialogCommand
    {
        public static RoutedUICommand ShowSettingsDialog { get; private set; }

        static ShowSettingsDialogCommand()
        {
            InputGestureCollection gestures = new InputGestureCollection();
            gestures.Add(new KeyGesture(Key.D3, ModifierKeys.Control));

            ShowSettingsDialog = new RoutedUICommand("Settings", "Settings", typeof(ShowSettingsDialogCommand), gestures);
        }
    }
}

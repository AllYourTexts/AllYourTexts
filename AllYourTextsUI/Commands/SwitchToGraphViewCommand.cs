using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AllYourTextsUi.Commands
{
    public class SwitchToGraphViewCommand
    {
        public static RoutedUICommand SwitchToGraphView { get; private set; }

        static SwitchToGraphViewCommand()
        {
            InputGestureCollection gestures = new InputGestureCollection();
            gestures.Add(new KeyGesture(Key.D2, ModifierKeys.Control));

            SwitchToGraphView = new RoutedUICommand("Graph View", "Graph View", typeof(SwitchToGraphViewCommand), gestures);
        }
    }
}

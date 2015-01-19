using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AllYourTextsUi.Commands
{
    public class CopyGraphCommand
    {
        public static RoutedUICommand CopyGraph { get; private set; }

        static CopyGraphCommand()
        {
            InputGestureCollection gestures = new InputGestureCollection();
            gestures.Add(new KeyGesture(Key.C, ModifierKeys.Control));

            CopyGraph = new RoutedUICommand("Copy Graph", "Copy Graph", typeof(CopyGraphCommand), gestures);
        }
    }
}

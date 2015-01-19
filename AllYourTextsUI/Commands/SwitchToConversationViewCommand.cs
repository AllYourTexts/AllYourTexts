using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AllYourTextsUi.Commands
{
    public class SwitchToConversationViewCommand
    {
        public static RoutedUICommand SwitchToConversationView { get; private set; }

        static SwitchToConversationViewCommand()
        {
            InputGestureCollection gestures = new InputGestureCollection();
            gestures.Add(new KeyGesture(Key.D1, ModifierKeys.Control));

            SwitchToConversationView = new RoutedUICommand("Conversation View", "Conversation View", typeof(SwitchToConversationViewCommand), gestures);
        }
    }
}

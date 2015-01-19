using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AllYourTextsUi.Commands
{
    public class ExportSingleConversationCommand
    {
        public static RoutedUICommand ExportSingleConversation { get; private set; }

        static ExportSingleConversationCommand()
        {
            InputGestureCollection gestures = new InputGestureCollection();
            gestures.Add(new KeyGesture(Key.E, ModifierKeys.Control));

            ExportSingleConversation = new RoutedUICommand("Export Selected Conversation", "Export Selected Conversation", typeof(ExportSingleConversationCommand), gestures);
        }
    }
}

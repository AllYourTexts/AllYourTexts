using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AllYourTextsUi.Commands
{
    public class ExportAllConversationsCommand
    {
        public static RoutedUICommand ExportAllConversations { get; private set; }

        static ExportAllConversationsCommand()
        {
            InputGestureCollection gestures = new InputGestureCollection();
            gestures.Add(new KeyGesture(Key.E, ModifierKeys.Control | ModifierKeys.Shift));

            ExportAllConversations = new RoutedUICommand("Export All Conversations", "Export All Conversations", typeof(ExportAllConversationsCommand), gestures);
        }
    }
}

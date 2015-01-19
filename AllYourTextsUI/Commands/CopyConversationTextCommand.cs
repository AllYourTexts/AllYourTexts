using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AllYourTextsUi.Commands
{
    public class CopyConversationTextCommand
    {
        public static RoutedUICommand CopyConversationText { get; private set; }

        static CopyConversationTextCommand()
        {
            InputGestureCollection gestures = new InputGestureCollection();
            gestures.Add(new KeyGesture(Key.C, ModifierKeys.Control));

            CopyConversationText = new RoutedUICommand("Copy", "Copy", typeof(CopyConversationTextCommand), gestures);
        }
    }
}

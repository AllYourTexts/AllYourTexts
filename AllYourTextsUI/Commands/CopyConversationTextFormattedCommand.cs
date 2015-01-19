using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AllYourTextsUi.Commands
{
    public class CopyConversationTextFormattedCommand
    {
        public static RoutedUICommand CopyConversationTextFormatted { get; private set; }

        static CopyConversationTextFormattedCommand()
        {
            InputGestureCollection gestures = new InputGestureCollection();
            gestures.Add(new KeyGesture(Key.C, ModifierKeys.Control | ModifierKeys.Shift));

            CopyConversationTextFormatted = new RoutedUICommand("Copy Formatted", "Copy Formatted", typeof(CopyConversationTextFormattedCommand), gestures);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AllYourTextsUi.Commands
{
    public class ShareOnFacebookCommand
    {
        public static RoutedUICommand ShareOnFacebook { get; private set; }

        static ShareOnFacebookCommand()
        {
            InputGestureCollection gestures = new InputGestureCollection();
            gestures.Add(new KeyGesture(Key.F, ModifierKeys.Control | ModifierKeys.Shift));

            ShareOnFacebook = new RoutedUICommand("Share on Facebook", "Share on Facebook", typeof(ShareOnFacebookCommand), gestures);
        }
    }
}

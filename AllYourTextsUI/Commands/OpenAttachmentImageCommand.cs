using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AllYourTextsUi.Commands
{
    public class OpenAttachmentImageCommand
    {
        public static RoutedUICommand OpenAttachmentImage { get; private set; }

        static OpenAttachmentImageCommand()
        {
            OpenAttachmentImage = new RoutedUICommand("View Full Size", "View Full Size", typeof(OpenAttachmentImageCommand), new InputGestureCollection());
        }
    }
}

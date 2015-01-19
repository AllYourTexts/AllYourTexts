using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AllYourTextsUi.Commands
{
    public class OpenAttachmentVideoCommand
    {
        public static RoutedUICommand OpenAttachmentVideo { get; private set; }

        static OpenAttachmentVideoCommand()
        {
            OpenAttachmentVideo = new RoutedUICommand("Play video", "Play video", typeof(OpenAttachmentImageCommand), new InputGestureCollection());
        }
    }
}

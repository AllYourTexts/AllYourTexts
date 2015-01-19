using AllYourTextsLib.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsUi.Exporting
{
    public class ExportError
    {
        public IConversation Conversation { get; private set; }

        public Exception Error { get; private set; }

        public ExportError(IConversation conversation, Exception error)
        {
            Conversation = conversation;
            Error = error;
        }
    }
}

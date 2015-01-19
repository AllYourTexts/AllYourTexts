using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;

namespace AllYourTextsUi.Framework
{
    public interface IConversationListItem
    {
        string Description { get; }

        IConversation Conversation { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsLib.Framework
{
    public interface IConversation : IEnumerable<IConversationMessage>
    {
        IConversationMessage GetMessage(int messageIndex);

        IContactList AssociatedContacts { get; }

        int MessageCount { get; }

        void AddMessage(IConversationMessage message);
    }
}

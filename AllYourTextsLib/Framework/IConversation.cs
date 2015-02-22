using System.Collections.Generic;

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

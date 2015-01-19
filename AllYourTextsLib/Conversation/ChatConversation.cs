using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib.Conversation
{
    class ChatConversation : ConversationBase
    {
        public override IContactList AssociatedContacts { get; protected set; }

        public ChatConversation(IEnumerable<IContact> assocaiatedContacts)
        {
            AssociatedContacts = new ContactList(assocaiatedContacts);
            AssociatedContacts.Sort(ContactComparer.CompareAlphabetically);
        }
    }
}

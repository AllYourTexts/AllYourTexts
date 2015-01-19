using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib.Conversation
{
    public class SingleNumberConversation : ConversationBase
    {
        public override IContactList AssociatedContacts { get; protected set; }

        public SingleNumberConversation(IContact contact)
        {
            this.AssociatedContacts = new ContactList();
            if (contact != null)
            {
                this.AssociatedContacts.Add(contact);
            }
        }

    }
}

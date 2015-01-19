using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib.Conversation
{
    public class MergedConversation : ConversationBase
    {
        public override IContactList AssociatedContacts { get; protected set; }

        public MergedConversation(IConversation conversationA, IConversation conversationB)
        {
            AssociatedContacts = CoalesceContactLists(conversationA.AssociatedContacts, conversationB.AssociatedContacts);
            
            AddMessages(conversationA);
            AddMessages(conversationB);
        }

        private ContactList CoalesceContactLists(IContactList contactListA, IContactList contactListB)
        {
            Dictionary<long, IContactList> hashByContactId = new Dictionary<long, IContactList>();
            HashByContactId(hashByContactId, contactListA);
            HashByContactId(hashByContactId, contactListB);

            ContactList mergedContacts = new ContactList();

            foreach (IContactList contactListById in hashByContactId.Values)
            {
                IContact contactMerged = contactListById[0];
                for (int contactIndex = 1; contactIndex < contactListById.Count; contactIndex++)
                {
                    IContact contactCurrent = contactListById[contactIndex];
                    contactMerged = new MergedContact(contactMerged, contactCurrent);
                }
                mergedContacts.Add(contactMerged);
            }

            return mergedContacts;
        }

        private void HashByContactId(Dictionary<long, IContactList> hashByContactId, IContactList contactList)
        {
            foreach (IContact contact in contactList)
            {
                IContactList hashedContactList;
                if (hashByContactId.ContainsKey(contact.ContactId))
                {
                    hashedContactList = hashByContactId[contact.ContactId];
                    hashedContactList.Add(contact);
                }
                else
                {
                    hashedContactList = new ContactList();
                    hashedContactList.Add(contact);
                    hashByContactId[contact.ContactId] = contactList;
                }
            }
        }

        private void AddMessages(IEnumerable<IConversationMessage> messages)
        {
            foreach (IConversationMessage message in messages)
            {
                AddMessage(message);
            }
        }
    }
}

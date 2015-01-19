using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib;
using AllYourTextsLib.Conversation;
using AllYourTextsLib.Framework;

namespace AllYourTextsTest
{
    public class MockEmptyConversation : ConversationBase
    {
        public override IContactList AssociatedContacts { get; protected set; }

        public MockEmptyConversation(string phoneNumberValue)
        {
            AssociatedContacts = new ContactList();

            Contact contact = new Contact(Contact.UnknownContactId, null, null, null, new PhoneNumber(phoneNumberValue));

            AssociatedContacts.Add(contact);
        }

        public MockEmptyConversation(string firstName, string middleName, string lastName, string phoneNumberValue)
        {
            PhoneNumber phoneNumber = new PhoneNumber(phoneNumberValue);

            AssociatedContacts = new ContactList();
            Contact contact = new MockContact(firstName, middleName, lastName, phoneNumber);

            AssociatedContacts.Add(contact);
        }

        public MockEmptyConversation(IEnumerable<IContact> contacts)
        {
            AssociatedContacts = new ContactList(contacts);
        }
    }
}

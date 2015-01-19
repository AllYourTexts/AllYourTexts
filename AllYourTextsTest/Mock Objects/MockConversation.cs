using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;

namespace AllYourTextsTest
{
    public class MockConversation : IConversation
    {
        List<IConversationMessage> _messages;

        public MockConversation()
        {
            _messages = new List<IConversationMessage>();
        }

        public void AddMessage(IConversationMessage message)
        {
            _messages.Add(message);
        }

        public IConversationMessage GetMessage(int messageIndex)
        {
            return _messages[messageIndex];
        }

        public IContactList AssociatedContacts
        {
            get { throw new NotImplementedException(); }
        }

        public IContact Contact
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string ContactName
        {
            get { throw new NotImplementedException(); }
        }

        public int MessageCount
        {
            get
            {
                return _messages.Count;
            }
        }

        public IEnumerator<IConversationMessage> GetEnumerator()
        {
            for (int messageIndex = 0; messageIndex < _messages.Count; messageIndex++)
            {
                yield return GetMessage(messageIndex);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

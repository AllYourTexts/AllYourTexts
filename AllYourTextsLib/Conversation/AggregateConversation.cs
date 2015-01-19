using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib.Conversation
{
    public class AggregateConversation : IConversation
    {
        private IEnumerable<IConversation> _conversations;

        public IContactList AssociatedContacts
        {
            get
            {
                return null;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public AggregateConversation(IEnumerable<IConversation> conversations)
        {
            _conversations = conversations;
        }

        public IConversationMessage GetMessage(int messageIndex)
        {
            throw new NotImplementedException();
        }

        public int MessageCount
        {
            get
            {
                int messageCount = 0;
                foreach (IConversation conversation in _conversations)
                {
                    messageCount += conversation.MessageCount;
                }

                return messageCount;
            }
        }

        public IEnumerator<IConversationMessage> GetEnumerator()
        {
            foreach (IConversation conversation in _conversations)
            {
                foreach (IConversationMessage message in conversation)
                {
                    yield return message;
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void AddMessage(IConversationMessage message)
        {
            throw new NotImplementedException();
        }
    }
}

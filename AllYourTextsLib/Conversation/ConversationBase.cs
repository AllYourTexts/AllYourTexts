using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib.Conversation
{
    public abstract class ConversationBase : IConversation
    {
        private List<IConversationMessage> _messages;
        private bool _messagesSorted;

        protected ConversationBase()
        {
            _messages = new List<IConversationMessage>();
            _messagesSorted = false;
        }

        public void AddMessage(IConversationMessage message)
        {
            if ((message.MessageContents == null) && !message.HasAttachments())
            {
                return;
            }
            
            _messagesSorted = false;
            _messages.Add(message);
        }

        public abstract IContactList AssociatedContacts
        {
            get;
            protected set;
        }

        public virtual int MessageCount
        {
            get { return _messages.Count; }
        }

        protected void EnsureMessagesSorted()
        {
            if (_messagesSorted)
            {
                return;
            }

            SortMessages();
        }

        private void SortMessages()
        {
            _messages.Sort();
            _messagesSorted = true;
        }

        public IConversationMessage GetMessage(int messageIndex)
        {
            EnsureMessagesSorted();
            return _messages[messageIndex];
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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            foreach (IContact contact in AssociatedContacts)
            {
                sb.AppendFormat("{0}, ", contact.DisplayName);
            }
            sb.Remove(sb.Length - 2, 2);
            sb.AppendFormat(", MessageCount = {0}}}", MessageCount);

            return sb.ToString();
        }
    }
}

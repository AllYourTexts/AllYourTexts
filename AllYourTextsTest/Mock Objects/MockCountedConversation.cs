using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib;
using AllYourTextsLib.Conversation;
using AllYourTextsLib.Framework;

namespace AllYourTextsTest
{
    public class MockCountedConversation : ConversationBase
    {

        private int _MessageCount;

        public MockCountedConversation(int messageCount)
        {
            _MessageCount = messageCount;
        }

        public override IContactList AssociatedContacts
        {
            get
            {
                throw new NotImplementedException();
            }
            protected set
            {
                throw new NotImplementedException();
            }
        }

        public override int MessageCount
        {
            get
            {
                return _MessageCount;
            }
        }
    }
}

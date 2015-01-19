using System;
using System.Collections.Generic;

namespace AllYourTextsLib.Framework
{
    public interface IConversationManager : IEnumerable<IConversation>
    {
        int ConversationCount { get; }

        IConversation GetConversation(int conversationIndex);

        int FindConversationIndex(IConversation conversation);
    }
}

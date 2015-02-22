using System;
using System.Collections.Generic;

namespace AllYourTextsLib.Framework
{
    public interface IConversationMessage : IComparable
    {
        string Address { get; }
        string Country { get; }
        IContact Contact { get; set; }
        bool IsOutgoing { get;}
        string MessageContents { get; }
        DateTime Timestamp { get; }
        List<IMessageAttachment> Attachments { get; }

        bool HasAttachments();
    }
}

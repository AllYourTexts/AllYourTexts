using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsUi.Framework
{
    public interface IConversationRenderer
    {
        bool HasUnprocessedMessages { get; }

        string RenderMessagesAsString(int messageCount);

        List<System.Windows.Documents.Paragraph> RenderMessagesAsParagraphs(int messageCount);
    }
}

using System.Collections.Generic;
using System.IO;
using AllYourTextsLib.Framework;
using AllYourTextsUi.Exporting;

namespace AllYourTextsUi.Framework
{
    public interface IConversationExporter
    {
        List<ExportError> Export(IConversation conversation, IDisplayOptionsReadOnly displayOptions, string exportFilename);

        List<ExportError> ExportMultipleConversations(IEnumerable<IConversation> conversations, IDisplayOptionsReadOnly displayOptions, string exportPath, IProgressCallback progressCallback);
    }
}

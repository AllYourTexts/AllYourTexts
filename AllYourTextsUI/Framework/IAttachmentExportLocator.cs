using System;
using AllYourTextsLib.Framework;

namespace AllYourTextsUi.Framework
{
    public interface IAttachmentExportLocator
    {
        void AddAttachmentExportLocation(IMessageAttachment attachment, string exportLocation);

        string GetAttachmentExportRelativePath(IMessageAttachment attachment);

        string GetFileExportRelativePath(string originalPath);
    }
}

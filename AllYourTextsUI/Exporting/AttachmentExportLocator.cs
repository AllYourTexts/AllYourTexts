using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;
using AllYourTextsUi.Framework;

namespace AllYourTextsUi.Exporting
{
    public class AttachmentExportLocator : IAttachmentExportLocator
    {
        private Dictionary<string, string> _exportMappings;

        private string _exportBasePath;

        public AttachmentExportLocator(string exportBasePath)
        {
            _exportMappings = new Dictionary<string, string>();

            _exportBasePath = exportBasePath;
        }

        public void AddAttachmentExportLocation(IMessageAttachment attachment, string exportLocation)
        {
            string relativePath = FullPathToRelativePath(exportLocation);
            AddFileExportRelativeLocation(attachment.Path, relativePath);
        }

        public void AddFileExportLocation(string originalPath, string exportLocation)
        {
            string relativePath = FullPathToRelativePath(exportLocation);
            AddFileExportRelativeLocation(originalPath, relativePath);
        }

        private void AddFileExportRelativeLocation(string originalPath, string exportRelativeLocation)
        {
            _exportMappings[originalPath] = exportRelativeLocation;
        }

        public string GetAttachmentExportRelativePath(IMessageAttachment attachment)
        {
            return _exportMappings[attachment.Path];
        }

        public string GetFileExportRelativePath(string originalPath)
        {
            return _exportMappings[originalPath];
        }

        private string FullPathToRelativePath(string fullPath)
        {
            if (!fullPath.StartsWith(_exportBasePath))
            {
                throw new ArgumentException(string.Format("Path: {0} outside of expected directory: {1}", fullPath, _exportBasePath));
            }

            int relativePathStart = _exportBasePath.Length;
            if (!_exportBasePath.EndsWith("\\"))
            {
                relativePathStart += 1;
            }

            return fullPath.Substring(relativePathStart);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using AllYourTextsUi.Framework;
using AllYourTextsLib.Framework;
using AllYourTextsUi.Framework.Exceptions;

namespace AllYourTextsUi.Models
{
    public class AttachmentModel
    {
        public string BackupPath { get; private set; }

        public string OrignalFilename { get; private set; }

        private string _tempCopyPath;

        private IFileSystem _fileSystem;

        public AttachmentModel(IFileSystem fileSytem, IMessageAttachment attachment)
        {
            _fileSystem = fileSytem;

            BackupPath = attachment.Path;
            
            if (string.IsNullOrEmpty(BackupPath) || !_fileSystem.FileExists(BackupPath))
            {
                throw new AttachmentOpenException("Can't find expected attachment file: " + BackupPath);
            }

            OrignalFilename = attachment.OriginalFilename;

            _tempCopyPath = null;
        }

        public void SaveToFile(string filename)
        {
            _fileSystem.CopyFile(BackupPath, filename, true);
        }

        public void OpenTempCopy()
        {
            if (_tempCopyPath == null)
            {
                _tempCopyPath = CreateTempCopy();
            }

            Process.Start(_tempCopyPath);
        }

        private string CreateTempCopy()
        {
            string extension = Path.GetExtension(OrignalFilename);
            string copyFilePath;

            try
            {
                using (Stream tempCopyStream = _fileSystem.CreateTempFile(extension, out copyFilePath))
                {
                    _fileSystem.CopyFile(BackupPath, tempCopyStream);
                }
            }
            catch (Exception ex)
            {
                throw new AttachmentOpenException("Failed to open MMS attachment: " + BackupPath, BackupPath, ex);
            }

            return copyFilePath;
        }
    }
}

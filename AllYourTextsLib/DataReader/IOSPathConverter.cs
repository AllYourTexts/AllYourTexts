using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AllYourTextsLib.DataReader
{
    public class IOsPathConverter
    {
        public string TranslateiPhoneAttachmentPathToComputerPath(string iPhonePath, string computerBackupPath)
        {
            int pathStart;

            const string iOs6Prefix = "~/";
            const string iOs5Prefix = "/var/mobile/";

            if (iPhonePath.StartsWith(iOs6Prefix))
            {
                pathStart = iOs6Prefix.Length;
            }
            else if (iPhonePath.StartsWith(iOs5Prefix))
            {
                pathStart = iOs5Prefix.Length;
            }
            else
            {
                throw new ArgumentException("Unexpected path format: " + iPhonePath);
            }

            string domainAndPath = "MediaDomain-" + iPhonePath.Substring(pathStart);

            string attachmentFilename = MbdbPathConverter.MbdbPathToFilename(domainAndPath);

            string computerPath = Path.Combine(computerBackupPath, attachmentFilename);

            return computerPath;
        }

        public string GetFilenameFromiPhonePath(string iPhonePath)
        {
            int lastSlashIndex = iPhonePath.LastIndexOf("/");
            if (lastSlashIndex < 0)
            {
                throw new ArgumentException("Expected '/' in iPhone path: " + iPhonePath);
            }

            int filenameStart = lastSlashIndex + 1;
            if (filenameStart >= iPhonePath.Length)
            {
                throw new ArgumentException("Expected filename in iPhone path: " + iPhonePath);
            }

            return iPhonePath.Substring(filenameStart);
        }
    }
}

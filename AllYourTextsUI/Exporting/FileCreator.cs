using AllYourTextsUi.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AllYourTextsUi.Exporting
{
    public class FileCreator
    {
        public static string CreateNewDirectoryWithRenameAttempts(string directoryName, IFileSystem fileSystem, int maxRenameAttempts)
        {
            for (int directoryCreationAttempt = 1; directoryCreationAttempt < maxRenameAttempts; directoryCreationAttempt++)
            {
                string numberedDirectoryName;
                if (directoryCreationAttempt == 1)
                {
                    numberedDirectoryName = directoryName;
                }
                else
                {
                    string fullPath = Path.GetFullPath(directoryName);
                    string noTrailingSlash;
                    if (fullPath.EndsWith("\\"))
                    {
                        noTrailingSlash = fullPath.Substring(0, fullPath.Length - 1);
                    }
                    else
                    {
                        noTrailingSlash = fullPath;
                    }
                    numberedDirectoryName = string.Format("{0} ({1})", noTrailingSlash, directoryCreationAttempt.ToString());
                }

                if (fileSystem.DirectoryExists(numberedDirectoryName))
                {
                    continue;
                }

                fileSystem.CreateDirectory(numberedDirectoryName);
                return numberedDirectoryName;
            }

            throw new Exception(string.Format("Could not create directory: {0}. Exceeded {1} attempts.", directoryName, maxRenameAttempts));
        }

        public static Stream CreateNewFileWithRenameAttempts(string filename, IFileSystem fileSystem, int maxRenameAttempts, out string newFilename)
        {
            newFilename = null;
            Stream newFileStream = null;

            for (int fileCreationAttempt = 1; fileCreationAttempt < maxRenameAttempts; fileCreationAttempt++)
            {
                string numberedFilename;
                if (fileCreationAttempt == 1)
                {
                    numberedFilename = filename;
                }
                else
                {
                    string pathPart = Path.GetDirectoryName(filename);
                    string namePart = Path.GetFileNameWithoutExtension(filename);
                    string extensionPart = Path.GetExtension(filename);

                    numberedFilename = string.Format("{0} ({1}){2}", namePart, fileCreationAttempt.ToString(), extensionPart);
                    numberedFilename = Path.Combine(pathPart, numberedFilename);
                }

                try
                {
                    newFilename = numberedFilename;
                    newFileStream = fileSystem.CreateNewFile(numberedFilename);
                    return newFileStream;
                }
                catch (IOException)
                {
                    ;
                }
            }

            throw new Exception(string.Format("Could not create file: {0}. Exceeded {1} attempts.", newFilename, maxRenameAttempts));
        }

        public static Stream CreateNewFileWithRenameAttempts(string filename, IFileSystem fileSystem, int maxRenameAttempts)
        {
            string newFilename;
            return CreateNewFileWithRenameAttempts(filename, fileSystem, maxRenameAttempts, out newFilename);
        }
    }
}

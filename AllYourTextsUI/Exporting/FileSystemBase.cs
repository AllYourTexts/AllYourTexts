using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsUi.Framework;
using System.IO;

namespace AllYourTextsUi.Exporting
{
    public abstract class FileSystemBase : IFileSystem
    {
        public abstract void CreateDirectory(string directoryPath);
               
        public abstract bool DirectoryExists(string directoryPath);

        public abstract bool FileExists(string filePath);

        public abstract Stream CreateFile(string filename);

        public abstract Stream CreateNewFile(string filename);
        
        public abstract Stream OpenReadFile(string filename);

        public abstract Stream CreateTempFile(string extension, out string createdFilePath);

        public abstract void CopyFile(string sourcePath, string destinationPath, bool overwrite);

        public void CopyFile(string sourcePath, Stream destinationStream)
        {
            using (Stream inputStream = OpenReadFile(sourcePath))
            {
                inputStream.CopyTo(destinationStream);
            }
        }

        public abstract void DeleteFile(string filename);
    }
}

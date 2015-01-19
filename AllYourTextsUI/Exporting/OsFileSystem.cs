using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AllYourTextsUi.Framework;
using AllYourTextsUi.Exporting;

namespace AllYourTextsUi
{
    public class OsFileSystem : FileSystemBase
    {
        public override void CreateDirectory(string directoryPath)
        {
            Directory.CreateDirectory(directoryPath);
        }

        public override bool DirectoryExists(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        public override bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public override Stream CreateFile(string filename)
        {
            return new FileStream(filename, FileMode.CreateNew);
        }

        public override Stream CreateNewFile(string filename)
        {
            return new FileStream(filename, FileMode.CreateNew);
        }

        public override Stream OpenReadFile(string filename)
        {
            return File.OpenRead(filename);
        }

        public override Stream CreateTempFile(string extension, out string createdFilePath)
        {
            createdFilePath = CreateTempFilename(extension);

            return File.OpenWrite(createdFilePath);
        }

        private string CreateTempFilename(string fileExtension)
        {
            string tempFilePath = string.Format("{0}{1}{2}", Path.GetTempPath(), Guid.NewGuid().ToString(), fileExtension);
            
            return tempFilePath;
        }

        public override void CopyFile(string sourcePath, string destinationPath, bool overwrite)
        {
            File.Copy(sourcePath, destinationPath, overwrite);
        }

        public override void DeleteFile(string filename)
        {
            File.Delete(filename);
        }
    }
}

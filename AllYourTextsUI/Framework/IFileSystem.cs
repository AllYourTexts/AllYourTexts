using System;
using System.IO;

namespace AllYourTextsUi.Framework
{
    public interface IFileSystem
    {
        void CreateDirectory(string directoryPath);

        bool DirectoryExists(string directoryPath);

        bool FileExists(string filePath);

        Stream CreateFile(string filename);

        Stream CreateNewFile(string filename);

        Stream OpenReadFile(string filename);

        Stream CreateTempFile(string extension, out string createdFilePath);

        void CopyFile(string sourcePath, string destinationPath, bool overwrite);

        void CopyFile(string sourcePath, Stream destinationStream);

        void DeleteFile(string filename);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AllYourTextsUi.Framework;
using System.Collections;
using AllYourTextsUi.Exporting;

namespace AllYourTextsTest
{
    public class MockFileSystem : FileSystemBase
    {
        private List<string> _directories;

        private Dictionary<string, MemoryStream> _files;

        public MockFileSystem()
        {
            _directories = new List<string>();
            _files = new Dictionary<string, MemoryStream>();
        }

        public override void CreateDirectory(string directoryPath)
        {
            _directories.Add(Path.GetFullPath(directoryPath));
        }

        public override bool DirectoryExists(string directoryPath)
        {
            return _directories.Contains(Path.GetFullPath(directoryPath));
        }

        public override bool FileExists(string filePath)
        {
            return _files.ContainsKey(filePath);
        }

        public override Stream CreateFile(string filename)
        {
            if (_files.ContainsKey(filename))
            {
                _files.Remove(filename);
            }

            return CreateNewFile(filename);
        }

        public override Stream CreateNewFile(string filename)
        {
            MemoryStream mockStream = new MemoryStream();
            try
            {
                _files.Add(filename, mockStream);
            }
            catch (ArgumentException)
            {
                throw new IOException("Simulated file already exists: " + filename);
            }

            return mockStream;
        }

        public override Stream OpenReadFile(string filename)
        {
            MemoryStream fileStream = new MemoryStream(_files[filename].ToArray());
            fileStream.Seek(0, SeekOrigin.Begin);

            return fileStream;
        }

        public Stream OpenWriteFile(string filename)
        {
            if (FileExists(filename))
            {
                return OpenWriteFile(filename);
            }
            else
            {
                return CreateNewFile(filename);
            }
        }

        public override void DeleteFile(string filename)
        {
            _files.Remove(filename);
        }

        public int FileCount
        {
            get
            {
                return _files.Count;
            }
        }

        public int DirectoryCount
        {
            get
            {
                return _directories.Count;
            }
        }

        public IEnumerable<string> ListDirectories()
        {
            return _directories;
        }

        public IEnumerable<string> ListDirectoryContents(string path)
        {
            List<string> directoryContents = new List<string>();
            foreach (string filename in _files.Keys)
            {
                if (filename.StartsWith(path))
                {
                    directoryContents.Add(filename);
                }
            }

            return directoryContents;
        }

        public int CountDirectoryContents(string path)
        {
            int count = 0;
            foreach (string filename in ListDirectoryContents(path))
            {
                count++;
            }

            return count;
        }

        public override Stream CreateTempFile(string extension, out string createdFilePath)
        {
            createdFilePath = CreateTempFilename(extension);

            return CreateNewFile(createdFilePath);
        }

        private string CreateTempFilename(string fileExtension)
        {
            string tempFilePath = string.Format("{0}{1}{2}", @"C:\faketemp\", Guid.NewGuid().ToString(), fileExtension);

            return tempFilePath;
        }

        public override void CopyFile(string sourcePath, string destinationPath, bool overwrite)
        {
            Stream destinationStream = OpenWriteFile(destinationPath);

            CopyFile(sourcePath, destinationStream);
        }
    }
}

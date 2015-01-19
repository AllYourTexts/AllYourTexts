using AllYourTextsUi.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace AllYourTextsUi.Facebook
{
    public class AccessTokenSerializer : IAccessTokenSerializer
    {
        private IFileSystem _fileSystem;

        public AccessTokenSerializer(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public string Load()
        {
            string accessTokenFilename = GetFilename();

            if (!_fileSystem.FileExists(accessTokenFilename))
            {
                return null;
            }

            AccessToken accessToken;

            using (Stream fileStream = _fileSystem.OpenReadFile(accessTokenFilename))
            {
                BinaryFormatter deserializer = new BinaryFormatter();
                accessToken = (AccessToken)deserializer.Deserialize(fileStream);
            }

            if (accessToken.Expires <= DateTime.Now)
            {
                return null;
            }

            return accessToken.TokenValue;
        }

        public void Save(AccessToken accessToken)
        {
            string accessTokenFilename = GetFilename();

            string directoryName = Path.GetDirectoryName(accessTokenFilename);

            if (!_fileSystem.DirectoryExists(directoryName))
            {
                _fileSystem.CreateDirectory(directoryName);
            }

            using (Stream fileStream = _fileSystem.CreateNewFile(accessTokenFilename))
            {
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(fileStream, accessToken);
            }
        }

        public void Clear()
        {
            string accessTokenFilename = GetFilename();

            if (_fileSystem.FileExists(accessTokenFilename))
            {
                _fileSystem.DeleteFile(accessTokenFilename);
            }
        }

        private static string GetFilename()
        {
            string applicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string filename = "fboauth.cache";

            return Path.Combine(new string[] { applicationDataPath, "AllYourTexts Software", "AllYourTexts", filename });
        }
    }
}

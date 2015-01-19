using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace AllYourTextsLib.Framework
{
    [Serializable()]
    public class NoBackupsFoundException : BackupDatabaseReadException
    {
        public string Path { get; private set; }

        public NoBackupsFoundException(string path)
            : this(path, null)
        {
            ;
        }

        protected NoBackupsFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Path = info.GetString("Path");
        }

        public NoBackupsFoundException(string path, Exception inner)
            : base(CreateMessage(path), inner)
        {
            Path = path;
        }

        private static string CreateMessage(string path)
        {
            const string messageFormat = "The iPhone backup path contains no folders: '{0}'.";

            return string.Format(messageFormat, path);
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Path", Path);
        }
    }
}

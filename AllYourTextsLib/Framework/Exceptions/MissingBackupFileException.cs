using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace AllYourTextsLib.Framework
{

    [Serializable()]
    public class MissingBackupFileException : BackupDatabaseReadException
    {
        public string Filename { get; private set; }

        public MissingBackupFileException(string filename)
            : this(filename, null)
        {
            ;
        }

        protected MissingBackupFileException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Filename = info.GetString("Filename");
        }

        public MissingBackupFileException(string filename, Exception inner)
            : base(CreateMessage(filename), inner)
        {
            Filename = filename;
        }

        private static string CreateMessage(string filename)
        {
            const string messageFormat = "The required iPhone database file does not exist: '{0}'.";

            return string.Format(messageFormat, filename);
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Filename", Filename);
        }
    }
}

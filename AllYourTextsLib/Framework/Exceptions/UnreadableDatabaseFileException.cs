using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace AllYourTextsLib.Framework
{
    [Serializable()]
    public class UnreadableDatabaseFileException : BackupDatabaseReadException
    {
        public string Filename { get; private set; }

        public override string Message
        {
            get
            {
                return _Message;
            }
        }

        private const string _Message = "SQLite database file could not be read.";

        public UnreadableDatabaseFileException(string filename)
            : this(filename, null)
        {
            ;
        }

        public UnreadableDatabaseFileException(string filename, Exception inner)
            : base(_Message, inner)
        {
            Filename = filename;
        }

        protected UnreadableDatabaseFileException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Filename = info.GetString("Filename");
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Filename", Filename);
        }
    }
}

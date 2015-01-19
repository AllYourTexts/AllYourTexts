using System;
using System.Runtime.Serialization;

namespace AllYourTextsLib.Framework
{
    [Serializable()]
    public abstract class BackupDatabaseReadException : Exception
    {

        public BackupDatabaseReadException()
            : base()
        {
            ;
        }

        public BackupDatabaseReadException(string message)
            : base(message)
        {
            ;
        }

        public BackupDatabaseReadException(string message, Exception inner)
            : base(message, inner)
        {
            ;
        }

        protected BackupDatabaseReadException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            ;
        }
    }
}

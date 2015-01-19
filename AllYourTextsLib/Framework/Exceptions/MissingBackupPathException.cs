using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace AllYourTextsLib.Framework
{
    [Serializable()]
    public class MissingBackupPathException : BackupDatabaseReadException
    {
        public string BackupPath { get; private set; }

        public MissingBackupPathException()
            : this(null, null)
        {
            ;
        }

        protected MissingBackupPathException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            BackupPath = info.GetString("BackupPath");
        }

        public MissingBackupPathException(string backupPath)
            : this(backupPath, null)
        {
            ;
        }

        public MissingBackupPathException(string backupPath, Exception inner)
            : base(CreateMessage(backupPath), inner)
        {
            BackupPath = backupPath;
        }

        private static string CreateMessage(string backupPath)
        {
            const string message = "The iPhone backup directory does not exist";

            if (backupPath != null)
            {
                return message + ": '" + backupPath + "'.";
            }
            else
            {
                return message + ".";
            }
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("BackupPath", BackupPath);
        }
    }
}

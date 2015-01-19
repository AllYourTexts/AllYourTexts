using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace AllYourTextsUi.Framework.Exceptions
{
    [Serializable()]
    public class AttachmentOpenException : Exception
    {

        public AttachmentOpenException()
            : base("Failed to open MMS attachment.")
        {
            ;
        }

        public AttachmentOpenException(string message)
            : base(message)
        {
            ;
        }

        public AttachmentOpenException(string message, Exception inner)
            : base(message, inner)
        {
            ;
        }

        protected AttachmentOpenException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ;
        }

        public AttachmentOpenException(string message, string attachmentBackupPath, Exception inner)
            : base(message, inner)
        {
            ;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AllYourTextsUi.Facebook
{
    public class UploadFailedException : Exception
    {
        public UploadFailedException(Exception inner)
            : base("File upload failed.", inner)
        {
            ;
        }

        public UploadFailedException(string message, Exception inner)
            : base(message, inner)
        {
            ;
        }

        protected UploadFailedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ;
        }
    }
}

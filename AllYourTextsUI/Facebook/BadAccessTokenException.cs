using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AllYourTextsUi.Facebook
{
    public class BadAccessTokenException : Exception
    {
        public BadAccessTokenException()
            : this(null)
        {
            ;
        }

        public BadAccessTokenException(Exception inner)
            : base("Invalid OAuth token.", inner)
        {
            ;
        }

        public BadAccessTokenException(string message, Exception inner)
            : base(message, inner)
        {
            ;
        }

        protected BadAccessTokenException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ;
        }
    }
}

using System;
using System.Runtime.Serialization;

namespace AllYourTextsLib.Framework
{
    [Serializable()]
    public class MbdbDataInvalidException : Exception
    {
        public MbdbDataInvalidException(string message)
            : this(message, null)
        {
            ;
        }

        public MbdbDataInvalidException(string message, Exception inner)
            : base(message, inner)
        {
            ;
        }

        protected MbdbDataInvalidException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ;
        }
    }
}

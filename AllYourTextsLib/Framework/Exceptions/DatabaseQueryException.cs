using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace AllYourTextsLib.Framework
{
    [Serializable()]
    public class DatabaseQueryException : Exception
    {
        public string Query { get; private set; }

        public DatabaseQueryException(string query)
            : this(query, null)
        {
            ;
        }

        public DatabaseQueryException(string query, Exception inner)
            : base("The SQLite database query failed.", inner)
        {
            Query = query;
        }

        protected DatabaseQueryException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Query = info.GetString("Query");
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("Query", Query);
        }
    }
}

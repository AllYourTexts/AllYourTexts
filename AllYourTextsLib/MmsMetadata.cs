using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsLib
{
    public class MmsMetadata
    {
        public long MessageId { get; private set; }

        public string MessageContents { get; private set; }

        public MmsMetadata(long messageId, string messageContents)
        {
            MessageId = messageId;

            MessageContents = messageContents;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsLib
{
    public class MmsMessageData
    {
        public long MessageId { get; private set; }

        public string FullSizeFilename { get; private set; }

        public string PreviewFilename { get; private set; }

        public string MessageContents { get; private set; }

        public MmsMessageData(long messageId, string fullSizeFilename, string previewFilename, string messageContents)
        {
            MessageId = messageId;

            FullSizeFilename = fullSizeFilename;

            PreviewFilename = previewFilename;

            MessageContents = messageContents;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsLib
{
    public class MmsDataItem
    {
        public long MessageId { get; private set; }

        public string PreviewFilename { get; private set; }

        public string FullSizeFilename { get; private set; }

        public MmsDataItem(long messageId, string previewFilename, string fullSizeFilename)
        {
            MessageId = messageId;

            PreviewFilename = previewFilename;

            FullSizeFilename = fullSizeFilename;
        }

        public override bool  Equals(object obj)
        {
            MmsDataItem toCompare = obj as MmsDataItem;
            if (toCompare == null)
            {
                return false;
            }

            if (this.MessageId != toCompare.MessageId)
            {
                return false;
            }

            if (!string.Equals(this.PreviewFilename, toCompare.PreviewFilename))
            {
                return false;
            }

            if (!string.Equals(this.FullSizeFilename, toCompare.FullSizeFilename))
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

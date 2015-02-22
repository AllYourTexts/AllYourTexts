using AllYourTextsLib.Framework;

namespace AllYourTextsLib
{
    public class MessageAttachment : IMessageAttachment
    {
        public long MessageId { get; private set; }
        public AttachmentType Type { get; private set; }
        public string Path { get; private set; }
        public string OriginalFilename { get; private set; }

        public MessageAttachment(long messageId, AttachmentType type, string path, string originalFilename)
        {
            MessageId = messageId;

            Type = type;

            Path = path;

            OriginalFilename = originalFilename;
        }

        public override bool Equals(object obj)
        {
            return Equals((MessageAttachment)obj);
        }

        public bool Equals(MessageAttachment other)
        {
            if (this.MessageId != other.MessageId)
            {
                return false;
            }
            if (this.Type != other.Type)
            {
                return false;
            }

            if (this.Path != other.Path)
            {
                return false;
            }

            if (this.OriginalFilename != other.OriginalFilename)
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

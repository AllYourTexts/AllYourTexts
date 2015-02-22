using AllYourTextsLib.Framework;
using System;

namespace AllYourTextsLib.DataReader
{
    public abstract class TextMessageReaderBase : DatabaseParserBase<TextMessage>
    {
        public TextMessageReaderBase(string backupDataPath)
            :this(backupDataPath, new IOsPathConverter())
        {
        }

        public TextMessageReaderBase(string backupDataPath, IOsPathConverter iOsPathConverter)
            : base()
        {
            _backupDataPath = backupDataPath;
            _iOsPathConverter = iOsPathConverter;
        }

        protected override string DataCountQuery
        {
            get
            {
                return @"
SELECT
    COUNT(*)
FROM
    message";
            }
        }

        protected static DateTime UnixTimeToLocalTime(long unixTime)
        {
            if (unixTime < 0)
            {
                return UtcTimeToLocalTime(_Epoch);
            }
            try
            {
                return UtcTimeToLocalTime(_Epoch.AddSeconds(unixTime));
            }
            catch (ArgumentOutOfRangeException)
            {
                return DateTime.MaxValue;
            }
        }

        private static DateTime UtcTimeToLocalTime(DateTime utcTime)
        {
            return utcTime.ToLocalTime();
        }

        protected static DateTime IMessageTimeToLocalTime(long iMessageTime)
        {
            return UnixTimeToLocalTime(iMessageTime).AddYears(31);
        }

        protected static string SanitizeMessageContents(string unsanitized)
        {
            const char UnicodeObjectReplacementChar = '\uFFFC';
            const char UnicodeLineFeedChar = '\u00A0';
            string sanitized;

            if (string.IsNullOrEmpty(unsanitized))
            {
                return unsanitized;
            }

            sanitized = unsanitized;
            if ((sanitized[0] == UnicodeObjectReplacementChar) || (sanitized[0] == UnicodeLineFeedChar))
            {
                if (sanitized.Length == 1)
                {
                    return string.Empty;
                }

                sanitized = sanitized.Substring(1);
            }

            if (sanitized[sanitized.Length - 1] == UnicodeLineFeedChar)
            {
                sanitized = sanitized.Substring(0, sanitized.Length - 1);
            }

            return sanitized;
        }

        protected MessageAttachment CreateMessageAttachment(long messageId, string attachmentPath, string attachmentMimeType)
        {
            if ((string.IsNullOrEmpty(attachmentPath)) || (string.IsNullOrEmpty(attachmentMimeType)))
            {
                return null;
            }

            AttachmentType attachmentType = ParseAttachmentType(attachmentMimeType);
            if (attachmentType == AttachmentType.Unknown)
            {
                return null;
            }

            string translatedPath = _iOsPathConverter.TranslateiPhoneAttachmentPathToComputerPath(attachmentPath, _backupDataPath);

            string originalFilename = _iOsPathConverter.GetFilenameFromiPhonePath(attachmentPath);

            return new MessageAttachment(messageId, attachmentType, translatedPath, originalFilename);
        }

        protected AttachmentType ParseAttachmentType(string attachmentMimeType)
        {
            if (attachmentMimeType.StartsWith("image"))
            {
                return AttachmentType.Image;
            }
            else if (attachmentMimeType.StartsWith("video"))
            {
                return AttachmentType.Video;
            }
            else if (attachmentMimeType.StartsWith("audio"))
            {
                return AttachmentType.Audio;
            }
            else
            {
                return AttachmentType.Unknown;
            }
        }

        protected const string iMessageServiceIdentifier = "iMessage";
        private static readonly DateTime _Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private IOsPathConverter _iOsPathConverter;
        private string _backupDataPath;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;
using System.IO;

namespace AllYourTextsLib.DataReader
{
    public class TextMessageReaderiOS6 : TextMessageReaderBase
    {
        public TextMessageReaderiOS6(string backupDataPath)
            : base(backupDataPath)
        {
        }

        private enum ValueIndex
        {
            RowId,
            HandleId,
            ChatIdentifier,
            Text,
            Country,
            Service,
            Date,
            DateRead,
            DateDelivered,
            IsFromMe,
            AttachmentFilename,
            AttachmentMimeType
        }

        protected override string DataQuery
        {
            get
            {
                return @"
SELECT
    message.ROWID,
    handle.id,
    chat.chat_identifier,
    message.text,
    handle.country,
    message.service,
    message.date,
    message.date_read,
    message.date_delivered,
    message.is_from_me,
    attachment.filename,
    attachment.mime_type
FROM
    message,
    chat,
    chat_message_join,
    handle
LEFT JOIN
    message_attachment_join ON message.rowid=message_attachment_join.message_id
LEFT JOIN
    attachment ON attachment.rowid=message_attachment_join.attachment_id 
WHERE
    chat_message_join.chat_id=chat.ROWID AND
    chat_message_join.message_id=message.ROWID AND
    (message.handle_id=handle.ROWID OR
     message.handle_id=0) 
GROUP BY
    message.ROWID";
            }
        }

        protected override TextMessage ParseItemFromDatabase(IDatabaseReader databaseReader)
        {
            long messageId = databaseReader.GetInt64((int)ValueIndex.RowId);
            string handleId = databaseReader.GetString((int)ValueIndex.HandleId);
            string chatIndentifier = databaseReader.GetString((int)ValueIndex.ChatIdentifier);
            string messageContents = databaseReader.GetString((int)ValueIndex.Text);
            string country = databaseReader.GetString((int)ValueIndex.Country);
            string service = databaseReader.GetString((int)ValueIndex.Service);
            long date = databaseReader.GetInt64((int)ValueIndex.Date);
            long dateRead = databaseReader.GetInt64((int)ValueIndex.DateRead);
            long dateDelivered = databaseReader.GetInt64((int)ValueIndex.DateDelivered);
            long isFromMe = databaseReader.GetInt64((int)ValueIndex.IsFromMe);
            string attachmentPath = databaseReader.GetString((int)ValueIndex.AttachmentFilename);
            string attachmentMimeType = databaseReader.GetString((int)ValueIndex.AttachmentMimeType);

            bool isOutgoing = (isFromMe != 0);

            messageContents = SanitizeMessageContents(messageContents);

            long timestampToConvert = date;
            if (!isOutgoing && string.Equals(service, iMessageServiceIdentifier) && (dateRead != 0))
            {
                timestampToConvert = dateRead;
            }

            DateTime timestamp = IMessageTimeToLocalTime(timestampToConvert);

            string chatId;
            string address;
            if (chatIndentifier.StartsWith("chat"))
            {
                if (isOutgoing)
                {
                    address = null;
                }
                else
                {
                    address = handleId;
                }
                chatId = chatIndentifier;
            }
            else
            {
                address = handleId;
                chatId = null;
            }

            IMessageAttachment attachment;
            try
            {
                attachment = CreateMessageAttachment(messageId, attachmentPath, attachmentMimeType);
            }
            catch
            {
                attachment = null;
            }

            return new TextMessage(messageId,
                                   isOutgoing,
                                   timestamp,
                                   messageContents,
                                   address,
                                   chatId,
                                   country,
                                   attachment);
        }
    }
}

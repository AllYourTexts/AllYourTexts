using AllYourTextsLib.DataReader;
using System;

namespace AllYourTextsTest
{
    public class MockTextDatabaseRowiOS6
    {
        public long MessageId { get; private set; }
        public string HandleId { get; private set; }
        public string ChatIdentifier { get; private set; }
        public string Text { get; private set; }
        public string Country { get; private set; }
        public string Service { get; private set; }
        public long Date { get; private set; }
        public long DateRead { get; private set; }
        public long DateDelivered { get; private set; }
        public long IsFromMe { get; private set; }
        public string AttachmentFilename { get; private set; }
        public string AttachmentMimeType { get; private set; }

        public MockTextDatabaseRowiOS6(long messageId,
                                       string handleId,
                                       string chatIdentifier,
                                       string text,
                                       string country,
                                       string service,
                                       long date,
                                       long dateRead,
                                       long dateDelivered,
                                       long isFromMe,
                                       string attachmentFilename,
                                       string attachmentMimeType)
        {
            MessageId = messageId;
            HandleId = handleId;
            ChatIdentifier = chatIdentifier;
            Text = text;
            Country = country;
            Service = service;
            Date = date;
            DateRead = dateRead;
            DateDelivered = dateDelivered;
            IsFromMe = isFromMe;
            AttachmentFilename = attachmentFilename;
            AttachmentMimeType = attachmentMimeType;
        }
    }

    public class MockTextDatabaseReaderiOS6 : MockDatabase<MockTextDatabaseRowiOS6>
    {
        public void AddRow(long messageId,
                           string handleId,
                           string chatIdentifier,
                           string text,
                           string country,
                           string service,
                           long date,
                           long dateRead,
                           long dateDelivered,
                           long isFromMe,
                           string attachmentFilename,
                           string attachmentMimeType)
        {
            base.AddRow(new MockTextDatabaseRowiOS6(messageId, handleId, chatIdentifier, text, country, service, date, dateRead, dateDelivered, isFromMe, attachmentFilename, attachmentMimeType));
        }

        public override string GetString(int index)
        {
            MockTextDatabaseRowiOS6 currentRow = GetCurrentRow();

            if (index == TextMessageReaderiOS6_Accessor.ValueIndex.HandleId.value__)
            {
                return currentRow.HandleId;
            }
            else if (index == TextMessageReaderiOS6_Accessor.ValueIndex.ChatIdentifier.value__)
            {
                return currentRow.ChatIdentifier;
            }
            else if (index == TextMessageReaderiOS6_Accessor.ValueIndex.Text.value__)
            {
                return currentRow.Text;
            }
            else if (index == TextMessageReaderiOS6_Accessor.ValueIndex.Country.value__)
            {
                return currentRow.Country;
            }
            else if (index == TextMessageReaderiOS6_Accessor.ValueIndex.Service.value__)
            {
                return currentRow.Service;
            }
            else if (index == TextMessageReaderiOS6_Accessor.ValueIndex.AttachmentFilename.value__)
            {
                return currentRow.AttachmentFilename;
            }
            else if (index == TextMessageReaderiOS6_Accessor.ValueIndex.AttachmentMimeType.value__)
            {
                return currentRow.AttachmentMimeType;
            }
            else
            {
                throw new ArgumentException("Invalid column number.");
            }
        }

        public override long GetInt64(int index)
        {
            MockTextDatabaseRowiOS6 currentRow = GetCurrentRow();

            if (index == TextMessageReaderiOS6_Accessor.ValueIndex.RowId.value__)
            {
                return currentRow.MessageId;
            }
            else if (index == TextMessageReaderiOS6_Accessor.ValueIndex.Date.value__)
            {
                return currentRow.Date;
            }
            else if (index == TextMessageReaderiOS6_Accessor.ValueIndex.DateRead.value__)
            {
                return currentRow.DateRead;
            }
            else if (index == TextMessageReaderiOS6_Accessor.ValueIndex.DateDelivered.value__)
            {
                return currentRow.DateDelivered;
            }
            else if (index == TextMessageReaderiOS6_Accessor.ValueIndex.IsFromMe.value__)
            {
                return currentRow.IsFromMe;
            }
            else
            {
                throw new ArgumentException("Invalid column number.");
            }
        }
    }
}

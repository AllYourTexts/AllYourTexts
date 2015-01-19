using System;
using AllYourTextsLib.DataReader;

namespace AllYourTextsTest
{
    public class MockTextDatabaseRow
    {
        public long MessageId { get; private set; }
        public string Address { get; private set; }
        public long Timestamp { get; private set; }
        public string MessageContents { get; private set; }
        public long Flags { get; private set; }
        public string Country { get; private set; }

        public MockTextDatabaseRow(long messageId, string address, long timestamp, string messageContents, long flags, string country)
        {
            MessageId = messageId;
            Address = address;
            Timestamp = timestamp;
            MessageContents = messageContents;
            Flags = flags;
            Country = country;
        }
    }

    public class MockTextDatabaseReader : MockDatabase<MockTextDatabaseRow>
    {

        public virtual void AddRow(long messageId, string address, long timestamp, string messageContents, long flags, string country)
        {
            MockTextDatabaseRow row = new MockTextDatabaseRow(messageId, address, timestamp, messageContents, flags, country);

            AddRow(row);
        }

        public override string GetString(int index)
        {
            MockTextDatabaseRow currentRow = GetCurrentRow();

            if (index == TextMessageReader_Accessor.ValueIndex.Address.value__)
            {
                return currentRow.Address;
            }
            else if (index == TextMessageReader_Accessor.ValueIndex.MessageContents.value__)
            {
                return currentRow.MessageContents;
            }
            else if (index == TextMessageReader_Accessor.ValueIndex.Country.value__)
            {
                return currentRow.Country;
            }
            else
            {
                throw new ArgumentException("Invalid column number.");
            }
        }

        public override long GetInt64(int index)
        {
            MockTextDatabaseRow currentRow = GetCurrentRow();

            if (index == TextMessageReader_Accessor.ValueIndex.MessageId.value__)
            {
                return currentRow.MessageId;
            }
            else if (index == TextMessageReader_Accessor.ValueIndex.Timestamp.value__)
            {
                return currentRow.Timestamp;
            }
            else if (index == TextMessageReader_Accessor.ValueIndex.Flags.value__)
            {
                return currentRow.Flags;
            }
            else
            {
                throw new ArgumentException("Invalid column number.");
            }
        }
    }
}

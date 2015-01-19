using AllYourTextsLib.DataReader;

namespace AllYourTextsTest
{
    public class MockTextDatabaseRow2 : MockTextDatabaseRow
    {
        public long IsMadrid { get; private set; }
        public string MadridRoomName { get; private set; }
        public string MadridHandle { get; private set; }
        public long MadridFlags { get; private set; }
        public long MadridDateRead { get; private set; }
        public long MadridDateDelivered { get; private set; }

        public MockTextDatabaseRow2(long messageId,
                                    string address,
                                    long timestamp,
                                    long timestampRead,
                                    long timestampDelivered,
                                    string message,
                                    long flags,
                                    string country,
                                    long isMadrid,
                                    string madridRoomName,
                                    string madridHandle,
                                    long madridFlags)
            :base(messageId, address, timestamp, message, flags, country)
        {
            IsMadrid = isMadrid;
            MadridRoomName = madridRoomName;
            MadridHandle = madridHandle;
            MadridFlags = madridFlags;
            MadridDateRead = timestampRead;
            MadridDateDelivered = timestampDelivered;
        }
    }

    public class MockTextDatabaseReader2 : MockTextDatabaseReader
    {

        public virtual void AddNonMadridRow(long messageId,
                                            string address,
                                            long timestamp,
                                            string message,
                                            long flags,
                                            string country)
        {
            MockTextDatabaseRow2 row = new MockTextDatabaseRow2(messageId,
                                                                address,
                                                                timestamp,
                                                                0,
                                                                0,
                                                                message,
                                                                flags,
                                                                country,
                                                                0,
                                                                null,
                                                                null,
                                                                0);

            AddRow(row);
        }

        public virtual void AddMadridRow(long messageId,
                                         long timestamp,
                                         long timestampRead,
                                         long timestampDelivered,
                                         string message,
                                         string madridRoomName,
                                         string madridHandle,
                                         long madridFlags)
        {
            MockTextDatabaseRow2 row = new MockTextDatabaseRow2(messageId,
                                                                null,
                                                                timestamp,
                                                                timestampRead,
                                                                timestampDelivered,
                                                                message,
                                                                0,
                                                                null,
                                                                1,
                                                                madridRoomName,
                                                                madridHandle,
                                                                madridFlags);

            AddRow(row);
        }

        public override string GetString(int index)
        {
            MockTextDatabaseRow2 currentRow = (MockTextDatabaseRow2)GetCurrentRow();

            if (index == TextMessageReader2_Accessor.MadridValueIndex.Handle.value__)
            {
                return currentRow.MadridHandle;
            }
            else if (index == TextMessageReader2_Accessor.MadridValueIndex.MadridRoomName.value__)
            {
                return currentRow.MadridRoomName;
            }
            else
            {
                return base.GetString(index);
            }
        }

        public override long GetInt64(int index)
        {
            MockTextDatabaseRow2 currentRow = (MockTextDatabaseRow2)GetCurrentRow();

            if (index == TextMessageReader2_Accessor.MadridValueIndex.IsMadrid.value__)
            {
                return currentRow.IsMadrid;
            }
            else if (index == TextMessageReader2_Accessor.MadridValueIndex.Flags.value__)
            {
                return currentRow.MadridFlags;
            }
            else if (index == TextMessageReader2_Accessor.MadridValueIndex.DateRead.value__)
            {
                return currentRow.MadridDateRead;
            }
            else if (index == TextMessageReader2_Accessor.MadridValueIndex.DateDelivered.value__)
            {
                return currentRow.MadridDateDelivered;
            }
            else
            {
                return base.GetInt64(index);
            }
        }
    }
}

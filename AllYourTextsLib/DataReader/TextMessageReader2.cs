using AllYourTextsLib.Framework;
using System;

namespace AllYourTextsLib.DataReader
{
    public class TextMessageReader2 : TextMessageReader
    {
        public TextMessageReader2(string backupDataPath)
            : base(backupDataPath)
        {
        }

        [Flags]
        public enum TextMessageMadridFlags
        {
            None = 0,
            UnknownFlag1 = 1,           // outgoing group-outgoing   incoming   group-incoming  
            UnknownFlag2 = 2,           // 
            UnknownFlag3 = 4,           // outgoing group-outgoing                              
            Delivered = 4096,           // outgoing                  incoming   group-incoming
            UnknownFlag5 = 8192,        //                           incoming   group-incoming
            UnknownFlag6 = 32768        // outgoing group-outgoing                              
        }

        protected enum MadridValueIndex
        {
            IsMadrid = ValueIndex.Last + 1,
            MadridRoomName,
            Handle,
            Flags,
            DateRead,
            DateDelivered
        }

        protected override string DataQuery
        {
            get
            {
                return @"
SELECT
    ROWID,
    address,
    date,
    text,
    flags,
    country,
    is_madrid,
    madrid_roomname,
    madrid_handle,
    madrid_flags,
    madrid_date_read,
    madrid_date_delivered
FROM
    message
";
            }
        }

        protected override TextMessage ParseItemFromDatabase(IDatabaseReader databaseReader)
        {
            long isMadridRaw = databaseReader.GetInt64((int)MadridValueIndex.IsMadrid);
            bool isMadrid = (isMadridRaw == 1);
            if (!isMadrid)
            {
                return base.ParseItemFromDatabase(databaseReader);
            }

            long messageId = databaseReader.GetInt64((int)ValueIndex.MessageId);
            long timestampMadrid = databaseReader.GetInt64((int)ValueIndex.Timestamp);
            string messageContents = databaseReader.GetString((int)ValueIndex.MessageContents);
            string madridRoomName = databaseReader.GetString((int)MadridValueIndex.MadridRoomName);
            string madridHandle = databaseReader.GetString((int)MadridValueIndex.Handle);
            long madridFlagsRaw = databaseReader.GetInt64((int)MadridValueIndex.Flags);
            long timestampMadridRead = databaseReader.GetInt64((int)MadridValueIndex.DateRead);
            long timestampMadridDelivered = databaseReader.GetInt64((int)MadridValueIndex.DateDelivered);

            TextMessageMadridFlags madridFlags;
            int madridFlagsInt = (int)(madridFlagsRaw & ((long)int.MaxValue));
            madridFlags = (TextMessageMadridFlags)madridFlagsInt;
            bool isOutgoing = ((madridFlags & TextMessageMadridFlags.UnknownFlag3) != 0) ||
                              ((madridFlags & TextMessageMadridFlags.UnknownFlag6) != 0);

            messageContents = SanitizeMessageContents(messageContents);

            long timestampToConvert = timestampMadrid;
            if (!isOutgoing && (timestampMadridRead != 0))
            {
                timestampToConvert = timestampMadridRead;
            }

            DateTime timestamp = IMessageTimeToLocalTime(timestampToConvert);

            return new TextMessage(messageId,
                                   isOutgoing,
                                   timestamp,
                                   messageContents,
                                   madridHandle,
                                   madridRoomName,
                                   null,
                                   null);
        }
    }
}

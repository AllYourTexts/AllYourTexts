using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib.DataReader
{

    [Flags]
    public enum TextMessageFlags
    {
        None = 0,
        Outgoing = 1,
        UndeterminedFlag = 2,
        SendingFailed = 32
    }

    public class TextMessageReader : TextMessageReaderBase
    {
        public TextMessageReader(string backupDataPath)
            : base(backupDataPath)
        {
        }

        protected enum ValueIndex
        {
            MessageId = 0,
            Address,
            Timestamp,
            MessageContents,
            Flags,
            Country,
            Last = Country
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
    country
FROM
    message
";
            }
        }

        protected override TextMessage ParseItemFromDatabase(IDatabaseReader databaseReader)
        {
            long messageId = databaseReader.GetInt64((int)ValueIndex.MessageId);
            if (messageId < 0)
            {
                throw new ArgumentOutOfRangeException("Message ID cannot be negative.");
            }

            string address = databaseReader.GetString((int)ValueIndex.Address);
            string country = databaseReader.GetString((int)ValueIndex.Country);
            long timestampUnix = databaseReader.GetInt64((int)ValueIndex.Timestamp);
            string messageContents = databaseReader.GetString((int)ValueIndex.MessageContents);
            long flagsRaw = databaseReader.GetInt64((int)ValueIndex.Flags);

            TextMessageFlags flags;

            //
            // Take the int part of the long (intentionally truncate the raw flags value)
            //

            int flagsInt = (int)(flagsRaw & ((long)int.MaxValue));
            flags = (TextMessageFlags)flagsInt;

            return new TextMessage(messageId,
                                   (flags & TextMessageFlags.Outgoing) != 0,
                                   UnixTimeToLocalTime(timestampUnix),
                                   messageContents,
                                   address,
                                   country);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib.DataReader
{
    public class ChatRoomInformationReader : DatabaseParserBase<ChatRoomInformation>
    {
        protected override string DataQuery
        {
            get
            {
                return @"
SELECT
    room_name,
    participants
FROM
    madrid_chat
WHERE
    room_name IS NOT NULL
";
            }
        }

        protected override string DataCountQuery
        {
            get
            {
                return @"
SELECT
    COUNT(*) 
FROM
    madrid_chat
WHERE
    room_name IS NOT NULL";
            }
        }

        private enum ValueIndex
        {
            RoomName = 0,
            Participants
        }

        protected override ChatRoomInformation ParseItemFromDatabase(IDatabaseReader databaseReader)
        {
            string roomName = databaseReader.GetString((int)ValueIndex.RoomName);
            byte[] participantsRaw = databaseReader.GetBlob((int)ValueIndex.Participants);

            string[] participants;
            try
            {
                participants = BPListParser.ParseBPListStrings(participantsRaw);
            }
            catch (FormatException ex)
            {
                List<string> bytesAsStrings = new List<string>(participantsRaw.Length);
                foreach (byte participantsRawByte in participantsRaw)
                {
                    bytesAsStrings.Add(string.Format("0x{0:X2}", participantsRawByte));
                }
                string bytesString = "{" + string.Join(", ", bytesAsStrings) + "}";

                throw new FormatException("Could not parse bpList: " + bytesString, ex);
            }

            return new ChatRoomInformation(roomName, participants);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib.DataReader
{
    public class ChatRoomInformationReaderiOS6 : DatabaseParserBase<ChatRoomInformation>
    {
        protected override string DataQuery
        {
            get
            {
                return @"
SELECT
    chat.room_name,
    handle.id
FROM
    chat,
    chat_handle_join,
    handle
WHERE
    chat.ROWID=chat_handle_join.chat_id
    AND handle.ROWID=chat_handle_join.handle_id
    AND chat.room_name IS NOT NULL;
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
    chat,
    chat_handle_join,
    handle
WHERE
    chat.ROWID=chat_handle_join.chat_id
    AND handle.ROWID=chat_handle_join.handle_id
    AND chat.room_name IS NOT NULL;
";
            }
        }

        private enum ValueIndexiOS6
        {
            RoomName = 0,
            HandleId
        }

        protected override ChatRoomInformation ParseItemFromDatabase(IDatabaseReader databaseReader)
        {
            string roomName = databaseReader.GetString((int)ValueIndexiOS6.RoomName);
            string participants = databaseReader.GetString((int)ValueIndexiOS6.HandleId);

            return new ChatRoomInformation(roomName, new string[] { participants });
        }

        protected override List<ChatRoomInformation> ParseItemsFromDatabase(IDatabaseReader databaseReader)
        {
            List<ChatRoomInformation> ungroupedChatInfoItems = base.ParseItemsFromDatabase(databaseReader);

            List<ChatRoomInformation> coalescedChatInfoItems = CoalesceChatRoomInformation(ungroupedChatInfoItems);

            return coalescedChatInfoItems;
        }

        private static List<ChatRoomInformation> CoalesceChatRoomInformation(IEnumerable<ChatRoomInformation> ungroupedChatInfoItems)
        {
            Dictionary<string, ChatRoomInformation> chatRoomInfoDictionary = new Dictionary<string, ChatRoomInformation>();

            foreach (ChatRoomInformation chatRoomInfo in ungroupedChatInfoItems)
            {
                if (chatRoomInfoDictionary.ContainsKey(chatRoomInfo.ChatId))
                {
                    ChatRoomInformation existingInfo = chatRoomInfoDictionary[chatRoomInfo.ChatId];
                    existingInfo.Participants.AddRange(chatRoomInfo.Participants);
                }
                else
                {
                    chatRoomInfoDictionary[chatRoomInfo.ChatId] = chatRoomInfo;
                }

            }

            return new List<ChatRoomInformation>(chatRoomInfoDictionary.Values);
        }
    }
}

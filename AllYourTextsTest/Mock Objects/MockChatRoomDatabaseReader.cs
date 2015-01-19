using System;
using AllYourTextsLib.DataReader;

namespace AllYourTextsTest
{

    public class ChatRoomDatabaseRow
    {
        public string RoomName { get; private set; }

        public byte[] Participants { get; private set; }

        public ChatRoomDatabaseRow(string roomName, byte[] participants)
        {
            this.RoomName = roomName;
            this.Participants = participants;
        }
    }

    class MockChatRoomDatabaseReader : MockDatabase<ChatRoomDatabaseRow>
    {

        public override string GetString(int index)
        {
            ChatRoomDatabaseRow currentRow = GetCurrentRow();

            if (index == ChatRoomInformationReader_Accessor.ValueIndex.RoomName.value__)
            {
                return currentRow.RoomName;
            }
            else
            {
                throw new ArgumentException("Invalid column number.");
            }
        }

        public override long GetInt64(int index)
        {
            throw new NotImplementedException();
        }

        public override byte[] GetBlob(int index)
        {
            ChatRoomDatabaseRow currentRow = GetCurrentRow();

            if (index == ChatRoomInformationReader_Accessor.ValueIndex.Participants.value__)
            {
                return currentRow.Participants;
            }
            else
            {
                throw new ArgumentException("Invalid column number.");
            }
        }

        public void AddRow(string roomName, byte[] participants)
        {
            AddRow(new ChatRoomDatabaseRow(roomName, participants));
        }
    }
}

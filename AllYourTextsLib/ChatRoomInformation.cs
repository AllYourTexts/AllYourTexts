using System.Collections.Generic;
using System.Linq;

namespace AllYourTextsLib
{
    public class ChatRoomInformation
    {
        public string ChatId { get; private set; }
        public List<string> Participants { get; private set; }

        public ChatRoomInformation(string chatId, IEnumerable<string> participants)
        {
            ChatId = chatId;
            Participants = new List<string>(participants);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals((ChatRoomInformation)obj);
        }

        public bool Equals(ChatRoomInformation other)
        {
            if (this.ChatId != other.ChatId)
            {
                return false;
            }

            if (!Enumerable.SequenceEqual(this.Participants, other.Participants))
            {
                return false;
            }

            return true;
        }
    }
}

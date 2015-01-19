using System;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib.Conversation
{
    public class ConversationStatistics : IConversationStatistics
    {
        public int MessagesSent { get; private set; }
        public int MessagesReceived { get; private set; }
        public int DayCount { get; private set; }

        public ConversationStatistics()
        {
            MessagesSent = 0;
            MessagesReceived = 0;
            DayCount = 0;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals((ConversationStatistics)obj);
        }

        public bool Equals(ConversationStatistics other)
        {
            if (MessagesSent != other.MessagesSent)
            {
                return false;
            }

            if (MessagesReceived != other.MessagesReceived)
            {
                return false;
            }

            if (DayCount != other.DayCount)
            {
                return false;
            }

            return true;
        }

        public int MessagesExchanged
        {
            get
            {
                return MessagesSent + MessagesReceived;
            }
        }

        public double MessagesPerDay
        {
            get
            {
                if (DayCount == 0)
                {
                    return 0;
                }

                return ((double)MessagesExchanged) / (DayCount);
            }
        }

        public void AddSentMessage()
        {
            MessagesSent++;
        }

        public void AddReceivedMessage()
        {
            MessagesReceived++;
        }

        public void AddDay()
        {
            DayCount++;
        }
    }
}

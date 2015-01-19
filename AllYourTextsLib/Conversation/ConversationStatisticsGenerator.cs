using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib.Conversation
{
    public class ConversationStatisticsGenerator
    {

        private ConversationStatisticsGenerator()
        {
            ;
        }

        public static IConversationStatistics CalculateStatistics(IConversation conversation)
        {
            ConversationStatistics stats = new ConversationStatistics();
            if (conversation == null)
            {
                return stats;
            }

            Dictionary<DateTime, bool> datesSeen = new Dictionary<DateTime, bool>();

            foreach (IConversationMessage message in conversation)
            {
                if (!datesSeen.ContainsKey(message.Timestamp.Date))
                {
                    datesSeen[message.Timestamp.Date] = true;
                    stats.AddDay();
                }
                
                if (message.IsOutgoing)
                {
                    stats.AddSentMessage();
                }
                else
                {
                    stats.AddReceivedMessage();
                }
            }

            return stats;
        }
    }
}

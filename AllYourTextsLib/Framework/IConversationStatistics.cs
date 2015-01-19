using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsLib.Framework
{
    public interface IConversationStatistics
    {
        int MessagesSent { get; }

        int MessagesReceived { get; }
        
        int DayCount { get; }
        
        int MessagesExchanged { get; }
        
        double MessagesPerDay { get; }
    }
}

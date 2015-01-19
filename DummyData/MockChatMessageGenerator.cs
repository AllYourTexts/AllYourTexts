using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DummyData
{
    public class MockChatMessageGenerator : MockMessageGeneratorBase
    {
        string _ChatId;

        public MockChatMessageGenerator(string chatId, int year, int month, int day, int hour, int minute, int second)
            :base(year, month, day, hour, minute, second)
        {
            _ChatId = chatId;
        }

        public void AddOutgoingMessage(double messageIntervalSeconds, string messageContents)
        {
            AddMessage(true, messageIntervalSeconds, null, messageContents, _ChatId, null);
        }

        public void AddIncomingMessage(double messageIntervalSeconds, string senderPhoneNumber, string messageContents)
        {
            AddMessage(false, messageIntervalSeconds, senderPhoneNumber, messageContents, _ChatId, null);
        }
    }
}

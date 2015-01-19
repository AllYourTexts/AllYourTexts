using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib;
using AllYourTextsLib.Framework;

namespace DummyData
{
    public abstract class MockMessageGeneratorBase
    {
        static protected int _LastMessageId = 0;
        protected DateTime _LastTimestamp;
        protected List<TextMessage> _TextMessages;

        public MockMessageGeneratorBase(int year, int month, int day, int hour, int minute, int second)
        {
            _LastTimestamp = new DateTime(year, month, day, hour, minute, second);
            _TextMessages = new List<TextMessage>();
        }

        protected void AddMessage(bool isOutgoing, double messageIntervalSeconds, string senderPhoneNumber, string messageContents, string chatId, IMessageAttachment attachment)
        {
            _LastMessageId++;

            DateTime timestamp = _LastTimestamp.AddSeconds(messageIntervalSeconds);

            TextMessage textMessage = new TextMessage(_LastMessageId, isOutgoing, timestamp, messageContents, senderPhoneNumber, chatId, null, attachment);
            _TextMessages.Add(textMessage);

            _LastTimestamp = timestamp;
        }

        public void SetCurrentTime(int year, int month, int day, int hour, int minute, int second)
        {
            _LastTimestamp = new DateTime(year, month, day, hour, minute, second);
        }

        public List<TextMessage> GetMessages()
        {
            return _TextMessages;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib;
using AllYourTextsLib.Framework;

namespace DummyData
{
    public class MockMessageGenerator : MockMessageGeneratorBase
    {
        private string _RemotePhoneNumber;

        public MockMessageGenerator(string remotePhoneNumber, int year, int month, int day, int hour, int minute, int second)
            :base(year, month, day, hour, minute, second)
        {
            _RemotePhoneNumber = remotePhoneNumber;
        }

        public void AddIncomingMessage(double messageIntervalSeconds, string messageContents)
        {
            AddMessage(false, messageIntervalSeconds, _RemotePhoneNumber, messageContents, null, null);
        }

        public void AddIncomingMessageWithAttachment(double messageIntervalSeconds, string messageContents, IMessageAttachment attachment)
        {
            AddMessage(false, messageIntervalSeconds, _RemotePhoneNumber, messageContents, null, attachment);
        }

        public void AddOutgoingMessage(double messageIntervalSeconds, string messageContents)
        {
            AddMessage(true, messageIntervalSeconds, _RemotePhoneNumber, messageContents, null, null);
        }

        public void AddOutgoingMessageWithAttachment(double messageIntervalSeconds, string messageContents, IMessageAttachment attachment)
        {
            AddMessage(true, messageIntervalSeconds, _RemotePhoneNumber, messageContents, null, attachment);
        }
    }
}

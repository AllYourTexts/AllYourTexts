using System.Collections.Generic;
using AllYourTextsLib;
using AllYourTextsLib.Conversation;
using AllYourTextsLib.Framework;
using DummyData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AllYourTextsTest
{
    
    /// <summary>
    ///This is a test class for ConversationStatisticsGeneratorTest and is intended
    ///to contain all ConversationStatisticsGeneratorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ConversationStatisticsGeneratorTest
    {
        [TestInitialize()]
        public void InitializeTest()
        {
            _attachments = new List<MessageAttachment>();
        }

        [TestMethod()]
        public void CalculateEmptyStatisticsTest()
        {
            Contact dummyContact = DummyConversationDataGenerator.GetContacts(DummyContactId.NeverTexter)[0];
            IConversation conversation = new SingleNumberConversation(dummyContact);
            IConversationStatistics statsActual = ConversationStatisticsGenerator.CalculateStatistics(conversation);

            Assert.AreEqual(0, statsActual.MessagesSent);
            Assert.AreEqual(0, statsActual.MessagesReceived);
            Assert.AreEqual(0, statsActual.MessagesExchanged);
            Assert.AreEqual(0, statsActual.DayCount);
            Assert.AreEqual(0, statsActual.MessagesPerDay);
        }

        [TestMethod()]
        public void CalculateSingleDayStatisticsTest()
        {
            DummyContactId[] contactIds = {DummyContactId.Davola};
            List<Contact> contacts = DummyConversationDataGenerator.GetContacts(contactIds);

            DummyPhoneNumberId[] messageSetIds = { DummyPhoneNumberId.DavolaCell };
            List<TextMessage> messages = DummyConversationDataGenerator.GetMessages(messageSetIds);
            List<ChatRoomInformation> chatInfoItems = new List<ChatRoomInformation>();

            ConversationManager conversationManager = new ConversationManager(contacts, messages, chatInfoItems, _attachments, null);

            Assert.AreEqual(2, conversationManager.ConversationCount);
            IConversation conversation = conversationManager.GetConversation(0);
            IConversationStatistics statsActual = ConversationStatisticsGenerator.CalculateStatistics(conversation);

            int messagesSentExpected = 4;
            int messagesReceivedExpected = messages.Count - messagesSentExpected;
            int messagesTotalExpected = messages.Count;
            Assert.AreEqual(messagesSentExpected, statsActual.MessagesSent);
            Assert.AreEqual(messagesReceivedExpected, statsActual.MessagesReceived);
            Assert.AreEqual(messagesTotalExpected, statsActual.MessagesExchanged);
            Assert.AreEqual(1, statsActual.DayCount);
        }

        [TestMethod()]
        public void CalculateMultiDayStatisticsTest()
        {
            DummyContactId[] contactIds = { DummyContactId.ReliableLarry };
            List<Contact> contacts = DummyConversationDataGenerator.GetContacts(contactIds);

            DummyPhoneNumberId[] messageSetIds = { DummyPhoneNumberId.ReliableLarryOffice };
            List<TextMessage> messages = DummyConversationDataGenerator.GetMessages(messageSetIds);
            List<ChatRoomInformation> chatInfoItems = new List<ChatRoomInformation>();

            ConversationManager conversationManager = new ConversationManager(contacts, messages, chatInfoItems, _attachments, null);

            IConversation conversation = conversationManager.GetConversation(0);
            IConversationStatistics statsActual = ConversationStatisticsGenerator.CalculateStatistics(conversation);

            int messagesSentExpected = 5;
            int messagesReceivedExpected = messages.Count - messagesSentExpected;
            int messagesTotalExpected = messages.Count;
            Assert.AreEqual(messagesSentExpected, statsActual.MessagesSent);
            Assert.AreEqual(messagesReceivedExpected, statsActual.MessagesReceived);
            Assert.AreEqual(messagesTotalExpected, statsActual.MessagesExchanged);
            Assert.AreEqual(5, statsActual.DayCount);
        }

        [TestMethod()]
        public void CalculateAggregateConversationStatisticsTest()
        {
            IConversation[] conversations =
                {
                    DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.TonyWolfCell),
                    DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.VictoriaWolfCell),
                };
            AggregateConversation aggregate = new AggregateConversation(conversations);
            IConversationStatistics statsActual = ConversationStatisticsGenerator.CalculateStatistics(aggregate);

            int messagesSentExpected = 31;
            int messagesReceivedExpected = aggregate.MessageCount - messagesSentExpected;
            int messagesTotalExpected = aggregate.MessageCount;
            int daysTotalExpected = 7;
            Assert.AreEqual(messagesSentExpected, statsActual.MessagesSent);
            Assert.AreEqual(messagesReceivedExpected, statsActual.MessagesReceived);
            Assert.AreEqual(messagesTotalExpected, statsActual.MessagesExchanged);
            Assert.AreEqual(daysTotalExpected, statsActual.DayCount);
        }

        private IEnumerable<MessageAttachment> _attachments;
    }
}

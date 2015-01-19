using AllYourTextsLib;
using AllYourTextsLib.Conversation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AllYourTextsLib.Framework;
using System.Collections.Generic;
using DummyData;

namespace AllYourTextsTest
{
    
    
    /// <summary>
    ///This is a test class for AggregateConversationTest and is intended
    ///to contain all AggregateConversationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AggregateConversationTest
    {

        [TestMethod()]
        public void EmptyTest()
        {
            MockLoadingProgressCallback progressCallback = new MockLoadingProgressCallback();

            DummyContactId[] dummyContactIds = { };
            DummyPhoneNumberId[] DummyPhoneNumberIds = { };

            ConversationManager conversationManager = DummyConversationDataGenerator.GetConversationManager(dummyContactIds, DummyPhoneNumberIds, progressCallback);
            AggregateConversation aggregateConversation = new AggregateConversation(new List<IConversation>());

            Assert.AreEqual(0, aggregateConversation.MessageCount);
        }

        [TestMethod()]
        public void MessageCountTest()
        {
            DummyContactId[] dummyContactIds = { DummyContactId.Davola, DummyContactId.Obama, DummyContactId.NeverTexter };
            DummyPhoneNumberId[] DummyPhoneNumberIds = { DummyPhoneNumberId.DavolaCell, DummyPhoneNumberId.DavolaiPhone, DummyPhoneNumberId.ObamaCell, DummyPhoneNumberId.UnknownLawnmower };

            ConversationManager conversationManager = DummyConversationDataGenerator.GetConversationManager(dummyContactIds, DummyPhoneNumberIds, null);
            AggregateConversation aggregateConversation = new AggregateConversation(conversationManager);

            int expectedMessageCount = 0;
            foreach (DummyPhoneNumberId setId in DummyPhoneNumberIds)
            {
                expectedMessageCount += DummyConversationDataGenerator.GetMessageCount(setId);
            }

            Assert.AreEqual(expectedMessageCount, aggregateConversation.MessageCount);
        }
    }
}

using AllYourTextsLib;
using AllYourTextsLib.Conversation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AllYourTextsLib.Framework;

namespace AllYourTextsTest
{
    
    
    /// <summary>
    ///This is a test class for TextConversationStatisticsTest and is intended
    ///to contain all TextConversationStatisticsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TextConversationStatisticsTest
    {

        public static void VerifyStatisticsAllZero(IConversationStatistics stats)
        {
            Assert.AreEqual(0, stats.DayCount);
            Assert.AreEqual(0, stats.MessagesExchanged);
            Assert.AreEqual(0, stats.MessagesPerDay);
            Assert.AreEqual(0, stats.MessagesReceived);
            Assert.AreEqual(0, stats.MessagesSent);
        }

        [TestMethod()]
        public void EmptyTest()
        {
            ConversationStatistics emptyStatistics = new ConversationStatistics();
            VerifyStatisticsAllZero(emptyStatistics);
        }
    }
}

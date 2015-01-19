using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using AllYourTextsLib.Framework;
using AllYourTextsLib;
using AllYourTextsLib.Conversation;

namespace AllYourTextsTest
{
    
    
    /// <summary>
    ///This is a test class for ConversationComparerTest and is intended
    ///to contain all ConversationComparerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ConversationComparerTest
    {

        private static int ReduceToUnitValue(int value)
        {
            if (value < 0)
            {
                return -1;
            }
            else if (value > 0)
            {
                return 1;
            }

            return 0;
        }

        private static void VerifyOrderingMatches(IConversation[] conversations, Comparison<IConversation> comparer)
        {
            for (int i = 0; i < conversations.Length; i++)
            {
                for (int j = 0; j < conversations.Length; j++)
                {
                    IConversation a = conversations[i];
                    IConversation b = conversations[j];
                    int compareResultExpected = ReduceToUnitValue(i - j);
                    int compareResultActual = ReduceToUnitValue(comparer(a, b));
                    if (compareResultExpected != compareResultActual)
                    {
                        Assert.AreEqual(compareResultExpected,
                                        compareResultActual,
                                        "Sort order did not match expected {{{0}, {1}}} (i = {2}, j = {3})",
                                        new object[] { a, b, i, j });
                    }
                }
            }
        }

        /// <summary>
        ///A test for AlphabeticalByContact
        ///</summary>
        [TestMethod()]
        public void AlphabeticalByContactTest()
        {
            string lowNumber = "111-111-1111";
            string midNumber = "555-555-5555";
            string highNumber = "999-999-9999";

            Contact contactA = new MockContact("Abraham", "Abramowitz", new PhoneNumber(lowNumber));
            Contact contactB = new MockContact("Ace", "Aarville", new PhoneNumber(midNumber));
            Contact contactC = new MockContact("Zed", "Ziffel", new PhoneNumber(highNumber));

            IConversation[] alphabeticallySortedConversations = 
                {
                    new MockEmptyConversation("Aaron", null, null, lowNumber),
                    new MockEmptyConversation("Aaron", null, "Aaronsen", lowNumber),
                    new MockEmptyConversation("Aaron", "Abbot", "Aaronsen", lowNumber),
                    new MockEmptyConversation("Aaron", "Zukros", "Aaronsen", lowNumber),
                    new MockEmptyConversation("Aaron", null, "Zubitz", lowNumber),
                    new MockEmptyConversation("Aaron", "Abbot", "Zubitz", lowNumber),
                    new MockEmptyConversation("Aaron", "Zukros", "Zubitz", lowNumber),
                    new MockEmptyConversation("Aaron", "Zukros", "Zubitz", highNumber),
                    new MockEmptyConversation("Billy", null, null, lowNumber),
                    new MockEmptyConversation(null, null, "McCheese", lowNumber),
                    new MockEmptyConversation(null, null, "McCheese", highNumber),
                    new MockEmptyConversation(null, "Morgan", "Davies", highNumber),
                    new MockEmptyConversation(null, "Myxmaster", null, lowNumber),
                    new MockEmptyConversation(null, null, "Norrenson", lowNumber),
                    new MockEmptyConversation("Otter", null, null, lowNumber),
                    new MockEmptyConversation("Zippy", null, "Aaronsen", lowNumber),
                    new MockEmptyConversation("Zippy", "Abbot", "Aaronsen", lowNumber),
                    new MockEmptyConversation("Zippy", "Zukros", "Aaronsen", lowNumber),
                    new MockEmptyConversation("Zippy", null, "Zokowski", lowNumber),
                    new MockEmptyConversation("Zippy", "Zukros", "Zokowski", lowNumber),
                    new MockEmptyConversation("Zippy", "Zukros", "Zokowski", highNumber),
                    new MockEmptyConversation(lowNumber),
                    new MockEmptyConversation(highNumber),
                    new MockEmptyConversation(new IContact[] { contactA, contactB }),
                    new MockEmptyConversation(new IContact[] { contactA, contactB, contactC }),
                    new MockEmptyConversation(new IContact[] { contactA, contactC }),
                    new MockEmptyConversation(new IContact[] { contactB, contactC }),
                };

            VerifyOrderingMatches(alphabeticallySortedConversations, ConversationComparer.AlphabeticalByContact);
        }

        [TestMethod()]
        public void ByTotalMessagesTest()
        {
            IConversation[] messageSortedConversations = 
                {
                    new MockCountedConversation(900),
                    new MockCountedConversation(600),
                    new MockCountedConversation(50),
                    new MockCountedConversation(1),
                    new MockCountedConversation(0),
                    null
                };

            VerifyOrderingMatches(messageSortedConversations, ConversationComparer.DescendingByTotalMessages);
        }
    }
}

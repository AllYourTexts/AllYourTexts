using System;
using AllYourTextsLib;
using AllYourTextsLib.DataReader;
using AllYourTextsLib.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AllYourTextsTest
{
    
    
    /// <summary>
    ///This is a test class for TextMessageReader2Test and is intended
    ///to contain all TextMessageReader2Test Unit Tests
    ///</summary>
    [TestClass()]
    public class TextMessageReader2Test
    {

        public static TextMessage SingleMessageFromDatabase(IDatabaseReader databaseReader)
        {
            TextMessageReader2 messageReader = new TextMessageReader2(null);
            messageReader.ParseDatabase(databaseReader);
            Assert.AreEqual(1, messageReader.ItemCountEstimate);
            return TextMessageReaderTest.SingleMessageFromReader(messageReader);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void ReadNullMadridMessageFromDatabaseTest()
        {
            const string messageContentsExpected = null;
            const string phoneNumberValueExpected = "+19245558928";

            MockTextDatabaseReader2 mockDatabase = new MockTextDatabaseReader2();
            mockDatabase.AddMadridRow(5,
                                      347059294,
                                      347059298,
                                      0,
                                      messageContentsExpected,
                                      null,
                                      phoneNumberValueExpected,
                                      12289);

            TextMessage messageActual = SingleMessageFromDatabase(mockDatabase);

            Assert.AreEqual(phoneNumberValueExpected, messageActual.Address);
            Assert.AreEqual(messageContentsExpected, messageActual.MessageContents);
            Assert.IsFalse(messageActual.IsOutgoing);

            DateTime timestampExpected = new DateTime(2011, 12, 30, 16, 21, 38);
            Assert.AreEqual(timestampExpected, messageActual.Timestamp);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void ReadEmptyMadridMessageFromDatabaseTest()
        {
            const string messageContentsExpected = "";
            const string phoneNumberValueExpected = "+19245558928";

            MockTextDatabaseReader2 mockDatabase = new MockTextDatabaseReader2();
            mockDatabase.AddMadridRow(9,
                                      347059294,
                                      347059298,
                                      0,
                                      messageContentsExpected,
                                      null,
                                      phoneNumberValueExpected,
                                      12289);

            TextMessage messageActual = SingleMessageFromDatabase(mockDatabase);

            Assert.AreEqual(phoneNumberValueExpected, messageActual.Address);
            Assert.IsNull(messageActual.MessageContents);
            Assert.IsFalse(messageActual.IsOutgoing);

            DateTime timestampExpected = new DateTime(2011, 12, 30, 16, 21, 38);
            Assert.AreEqual(timestampExpected, messageActual.Timestamp);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void ReadMadridMessageFromDatabaseTest()
        {
            const string messageContentsExpected = "What's up. The iMessage just came out!";
            const string phoneNumberValueExpected = "+19245558928";

            MockTextDatabaseReader2 mockDatabase = new MockTextDatabaseReader2();
            mockDatabase.AddMadridRow(5,
                                      347059294,
                                      347059298,
                                      0,
                                      messageContentsExpected,
                                      null,
                                      phoneNumberValueExpected,
                                      12289);

            TextMessage messageActual = SingleMessageFromDatabase(mockDatabase);

            Assert.AreEqual(phoneNumberValueExpected, messageActual.Address);
            Assert.AreEqual(messageContentsExpected, messageActual.MessageContents);
            Assert.IsFalse(messageActual.IsOutgoing);
            Assert.IsNull(messageActual.ChatId);

            DateTime timestampExpected = new DateTime(2011, 12, 30, 16, 21, 38);
            Assert.AreEqual(timestampExpected, messageActual.Timestamp);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void ReadChatMessageFromDatabaseTest()
        {
            const string messageContentsExpected = "Hey, what a great chat room!";
            const string roomNameExpected = "chat901258305184729544";

            MockTextDatabaseReader2 mockDatabase = new MockTextDatabaseReader2();
            mockDatabase.AddMadridRow(5,
                                      347059428,
                                      0,
                                      0,
                                      messageContentsExpected,
                                      roomNameExpected,
                                      null,
                                      12289);

            TextMessage messageActual = SingleMessageFromDatabase(mockDatabase);

            Assert.IsNull(messageActual.Address);
            Assert.AreEqual(messageContentsExpected, messageActual.MessageContents);
            Assert.IsFalse(messageActual.IsOutgoing);
            Assert.AreEqual(roomNameExpected, messageActual.ChatId);

            DateTime timestampExpected = new DateTime(2011, 12, 30, 16, 23, 48);
            Assert.AreEqual(timestampExpected, messageActual.Timestamp);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void ReadMadridMmsMessageFromDatabaseTest()
        {

            //
            // Madrid MMS messages contain a leading unicode object replacement character as
            // placeholder for the image.
            //

            const string messageContentsInput = "\uFFFCCheck out this awesome pic";
            string messageContentsExpected = messageContentsInput.Substring(1);
            const string phoneNumberValueExpected = "+13545552344";

            MockTextDatabaseReader2 mockDatabase = new MockTextDatabaseReader2();
            mockDatabase.AddMadridRow(6,
                                      347059294,
                                      0,
                                      347059296,
                                      messageContentsInput,
                                      null,
                                      phoneNumberValueExpected,
                                      36869);

            TextMessage messageActual = SingleMessageFromDatabase(mockDatabase);

            Assert.AreEqual(phoneNumberValueExpected, messageActual.Address);
            Assert.AreEqual(messageContentsExpected, messageActual.MessageContents);
            Assert.IsTrue(messageActual.IsOutgoing);

            DateTime timestampExpected = new DateTime(2011, 12, 30, 16, 21, 34);
            Assert.AreEqual(timestampExpected, messageActual.Timestamp);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void ReadNonMadridMessageFromDatabaseTest()
        {
            const string messageContentsExpected = "Yo what up li'l mama. It's ya boy yunky.";
            const string phoneNumberValueExpected = "212-555-5688";
            bool isOutgoingExpected = true;
            long flagsExpected = TextMessageReaderTest.MessageFlagsAsLong(isOutgoingExpected);

            MockTextDatabaseReader2 mockDatabase = new MockTextDatabaseReader2();
            mockDatabase.AddNonMadridRow(9,
                                         phoneNumberValueExpected,
                                         1325519337,
                                         messageContentsExpected,
                                         flagsExpected,
                                         CountryCallingCodeFinder.CountryAbbreviationUnitedStates);

            TextMessage messageActual = SingleMessageFromDatabase(mockDatabase);

            Assert.AreEqual(phoneNumberValueExpected, messageActual.Address);
            Assert.AreEqual(messageContentsExpected, messageActual.MessageContents);
            Assert.AreEqual(isOutgoingExpected, messageActual.IsOutgoing);

            DateTime timestampExpected = new DateTime(2012, 1, 2, 10, 48, 57);
            Assert.AreEqual(timestampExpected, messageActual.Timestamp);
        }
    }
}

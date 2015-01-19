using System;
using System.Collections.Generic;
using AllYourTextsLib;
using AllYourTextsLib.DataReader;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AllYourTextsTest
{
    
    /// <summary>
    ///This is a test class for TextMessageReaderTest and is intended
    ///to contain all TextMessageReaderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TextMessageReaderTest
    {

        public static TextMessage SingleMessageFromReader(TextMessageReader messageReader)
        {
            List<TextMessage> messages = new List<TextMessage>(messageReader);
            Assert.AreEqual(1, messages.Count);
            return messages[0];
        }

        public TextMessage MessageFromDatabaseRow(long messageId, string address, long timestamp, string messageContents, long flags, string country)
        {
            MockTextDatabaseReader mockTextDatabase = new MockTextDatabaseReader();
            mockTextDatabase.AddRow(messageId, address, timestamp, messageContents, flags, country);
            TextMessageReader mockTextMessageReader = new TextMessageReader(null);
            mockTextMessageReader.ParseDatabase(mockTextDatabase);
            return SingleMessageFromReader(mockTextMessageReader);
        }

        public void VerifyMessageRowMatches(long messageId, bool isOutgoing, DateTime timestamp, string messageContents, string address)
        {
            TextMessage messageExpected = new TextMessage(messageId, isOutgoing, timestamp, messageContents, address, CountryCallingCodeFinder.CountryAbbreviationUnitedStates);
            long flagsExpected = MessageFlagsAsLong(messageExpected.IsOutgoing);
            TextMessage messageActual = MessageFromDatabaseRow(messageId,
                                                               messageExpected.Address,
                                                               LocalTimeToUnixTime(messageExpected.Timestamp),
                                                               messageExpected.MessageContents,
                                                               flagsExpected,
                                                               CountryCallingCodeFinder.CountryAbbreviationUnitedStates);
            Assert.AreEqual(messageExpected, messageActual);
        }

        [TestMethod()]
        public void StandardMessagesTest()
        {
            VerifyMessageRowMatches(1, true, new DateTime(2008, 4, 27, 15, 36, 30), "Hey, Jordy. What's good?", "1234567890");
            VerifyMessageRowMatches(2, true, new DateTime(2009, 10, 15, 6, 32, 15), "Reception's really bad!", "90723476");
            VerifyMessageRowMatches(3, false, new DateTime(2011, 2, 12, 12, 16, 35), "I'm living life and lovin dives.", "8239828839");
            VerifyMessageRowMatches(4, false, new DateTime(2012, 1, 28, 11, 38, 15), "England is great! You should come over!", "+44 20 7840 0889");
            VerifyMessageRowMatches(5, false, new DateTime(2012, 1, 28, 11, 43, 18), "No! Come to Germany instead!", "+49 30 22440");
            VerifyMessageRowMatches(6, false, new DateTime(2012, 1, 28, 12, 45, 47), "Ay! Scotland is the best!", "0131 229 7899");
        }

        [TestMethod()]
        public void UndefinedFlagTest()
        {
            TextMessage messageExpected = new TextMessage(6, true, new DateTime(2010, 5, 15, 8, 9, 59), "This is a flag you've never seen before!", "9879992762", CountryCallingCodeFinder.CountryAbbreviationUnitedStates);
            long flagsExpected = MessageFlagsAsLong(messageExpected.IsOutgoing);
            long flagsInput = flagsExpected + 16;
            TextMessage messageActual = MessageFromDatabaseRow(messageExpected.MessageId,
                                                               messageExpected.Address,
                                                               LocalTimeToUnixTime(messageExpected.Timestamp),
                                                               messageExpected.MessageContents,
                                                               flagsInput,
                                                               messageExpected.Country);
            Assert.AreEqual(messageExpected, messageActual);
        }

        [TestMethod()]
        public void OverflowFlagTest()
        {
            TextMessage messageExpected = new TextMessage(87, true, new DateTime(2010, 12, 15, 8, 9, 59), "This is an overflowing flag!", "9827746655", CountryCallingCodeFinder.CountryAbbreviationUnitedStates);
            long flagsExpected = MessageFlagsAsLong(messageExpected.IsOutgoing);
            long flagsInput = long.MaxValue;
            TextMessage messageActual = MessageFromDatabaseRow(messageExpected.MessageId,
                                                               messageExpected.Address,
                                                               LocalTimeToUnixTime(messageExpected.Timestamp),
                                                               messageExpected.MessageContents,
                                                               flagsInput,
                                                               CountryCallingCodeFinder.CountryAbbreviationUnitedStates);
            Assert.AreEqual(messageExpected, messageActual);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NegativeMessageIdTest()
        {
            TextMessage messageActual = MessageFromDatabaseRow(-15,
                                                               "4825556145",
                                                               0,
                                                               "This is a negative message id!",
                                                               0,
                                                               CountryCallingCodeFinder.CountryAbbreviationUnitedStates);
        }

        [TestMethod()]
        public void MinTimestampTest()
        {
            TextMessage messageActual = MessageFromDatabaseRow(1,
                                                               "9827746655",
                                                               long.MinValue,
                                                               "This is the minimum timestamp!",
                                                               0,
                                                               CountryCallingCodeFinder.CountryAbbreviationUnitedStates);
            Assert.AreEqual(TextMessageReader_Accessor.UtcTimeToLocalTime(TextMessageReader_Accessor._Epoch), messageActual.Timestamp);
        }

        [TestMethod()]
        public void MaxTimestampTest()
        {
            TextMessage messageActual = MessageFromDatabaseRow(5,
                                                               "829-092-2387",
                                                               long.MaxValue,
                                                               "This is the max timestamp!",
                                                               0,
                                                               CountryCallingCodeFinder.CountryAbbreviationUnitedStates);
            Assert.AreEqual(DateTime.MaxValue, messageActual.Timestamp);
        }


        [TestMethod()]
        public void NullMessageTest()
        {
            TextMessage messageActual = MessageFromDatabaseRow(6,
                                                               "",
                                                               0,
                                                               null,
                                                               0,
                                                               CountryCallingCodeFinder.CountryAbbreviationUnitedStates);
            Assert.IsNull(messageActual.Address);
            Assert.AreEqual(TextMessageReader_Accessor.UtcTimeToLocalTime(TextMessageReader_Accessor._Epoch), messageActual.Timestamp);
            Assert.IsNull(messageActual.MessageContents);
            Assert.IsFalse(messageActual.IsOutgoing);
        }

        [TestMethod()]
        public void EmptyTest()
        {
            TextMessage messageActual = MessageFromDatabaseRow(7,
                                                               "",
                                                               0,
                                                               "",
                                                               0,
                                                               CountryCallingCodeFinder.CountryAbbreviationUnitedStates);
            Assert.IsNull(messageActual.Address);
            Assert.AreEqual(TextMessageReader_Accessor.UtcTimeToLocalTime(TextMessageReader_Accessor._Epoch), messageActual.Timestamp);
            Assert.IsNull(messageActual.MessageContents);
            Assert.IsFalse(messageActual.IsOutgoing);
            Assert.IsNull(messageActual.ChatId);
        }

        private long LocalTimeToUnixTime(DateTime localTime)
        {
            DateTime utcTime = localTime.ToUniversalTime();
            long unixTime = (long)(new TimeSpan(utcTime.Ticks - TextMessageReader_Accessor._Epoch.Ticks)).TotalSeconds;
            return unixTime;
        }

        public static long MessageFlagsAsLong(bool isOutgoing)
        {
            long flags = 0;

            //
            // The undetermined flag seems to always be present, so we add it no matter what.
            //

            flags += (long)TextMessageFlags.UndeterminedFlag;

            if (isOutgoing)
            {
                flags += (long)TextMessageFlags.Outgoing;
            }

            return flags;
        }

        private void VerifySanitizeMessageMatchesExpected(string unsanitizedInput, string sanitizedExpected)
        {
            string sanitizedActual = TextMessageReader_Accessor.SanitizeMessageContents(unsanitizedInput);
            Assert.AreEqual(sanitizedExpected, sanitizedActual);
        }

        [TestMethod()]
        public void SanitizeMessageTest()
        {
            VerifySanitizeMessageMatchesExpected(null, null);
            VerifySanitizeMessageMatchesExpected("", "");
            VerifySanitizeMessageMatchesExpected('\uFFFC' + "", ""); // object replacement character only should be replaced with empty string
            VerifySanitizeMessageMatchesExpected('\u00A0' + "", ""); // line feed character only should be replaced with empty string
            VerifySanitizeMessageMatchesExpected(string.Format("{0}{1}", '\uFFFC', '\u00A0'), ""); // object replacement character + line feed should be replaced with empty string
            VerifySanitizeMessageMatchesExpected('\uFFFC' + "hey, what's up?", "hey, what's up?"); // object replacement character should be removed from the beginning
            VerifySanitizeMessageMatchesExpected("greetings, I am quite well" + '\uFFFC', "greetings, I am quite well" + '\uFFFC'); // object replacement character should not be removed from the end
            VerifySanitizeMessageMatchesExpected('\u00A0' + "i like donuts", "i like donuts"); // line feed should be removed from beginning
            VerifySanitizeMessageMatchesExpected("today is a great day" + '\u00A0', "today is a great day"); // line feed should be removed from end
            VerifySanitizeMessageMatchesExpected('\u00A0' + "cats reign supreme" + '\u00A0', "cats reign supreme"); // line feed should be removed from beginning and end
            VerifySanitizeMessageMatchesExpected('\uFFFC' + "here, have a pickle" + '\u00A0', "here, have a pickle"); // object replacement character should be removed from the beginning, line feed removed from end
        }
    }
}

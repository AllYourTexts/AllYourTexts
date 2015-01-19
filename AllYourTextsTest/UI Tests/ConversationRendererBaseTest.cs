using AllYourTextsUi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AllYourTextsLib.Framework;
using AllYourTextsLib;
using AllYourTextsUi.Framework;

namespace AllYourTextsTest
{
    
    
    /// <summary>
    ///This is a test class for ConversationRendererBaseTest and is intended
    ///to contain all ConversationRendererBaseTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ConversationRendererBaseTest
    {
        private void VerifyMessageDisplayName(IConversationMessage message, string senderDisplayNameExpected)
        {
            string senderDisplayNameActual = ConversationRendererBase_Accessor.GetSenderDisplayName(message);
            Assert.AreEqual(senderDisplayNameExpected, senderDisplayNameActual);
        }

        /// <summary>
        ///A test for GetSenderDisplayName
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void GetSenderDisplayNameTest()
        {
            IConversationMessage outgoingMessage = new TextMessage(105, true, new DateTime(2009, 3, 5, 16, 17, 15), "Hey, wassup?", "212-555-1234", CountryCallingCodeFinder.CountryAbbreviationUnitedStates);
            VerifyMessageDisplayName(outgoingMessage, ConversationRendererBase_Accessor._localName);

            IConversationMessage incomingKnownContactMessage = new TextMessage(106, false, new DateTime(2009, 3, 5, 16, 17, 16), "Not much, and you?", "212-555-1234", CountryCallingCodeFinder.CountryAbbreviationUnitedStates);
            incomingKnownContactMessage.Contact = new MockContact("Joe", "Billichuck");
            VerifyMessageDisplayName(incomingKnownContactMessage, "Joe Billichuck");

            incomingKnownContactMessage.Contact = new MockContact("Sarah", "Michelle", "Gellar");
            VerifyMessageDisplayName(incomingKnownContactMessage, "Sarah Michelle Gellar");

            incomingKnownContactMessage.Contact = new MockContact("Sting", null);
            VerifyMessageDisplayName(incomingKnownContactMessage, "Sting");

            incomingKnownContactMessage.Contact = new MockContact(null, "Madonna");
            VerifyMessageDisplayName(incomingKnownContactMessage, "Madonna");

            incomingKnownContactMessage.Contact = new MockContact(null, null);
            VerifyMessageDisplayName(incomingKnownContactMessage, "Unknown Sender");

            incomingKnownContactMessage.Contact = new MockContact("", "");
            VerifyMessageDisplayName(incomingKnownContactMessage, "Unknown Sender");
        }

        private void VerifyTimeFormatMatches(DateTime timeInput, TimeDisplayFormat formatInput, string formattedTimeExpected)
        {
            string formattedTimeActual = ConversationRendererBase_Accessor.FormatTimeForConversation(timeInput, formatInput);

            Assert.AreEqual(formattedTimeExpected, formattedTimeActual);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void FormatTimeForConversationTest()
        {
            DateTime morningTime = new DateTime(2011, 1, 1, 6, 15, 47);
            VerifyTimeFormatMatches(morningTime, TimeDisplayFormat.HideTime, null);
            VerifyTimeFormatMatches(morningTime, TimeDisplayFormat.HourMin24h, "6:15");
            VerifyTimeFormatMatches(morningTime, TimeDisplayFormat.HourMinAmPm, "6:15 AM");
            VerifyTimeFormatMatches(morningTime, TimeDisplayFormat.HourMinSec24h, "6:15:47");
            VerifyTimeFormatMatches(morningTime, TimeDisplayFormat.HourMinSecAmPm, "6:15:47 AM");

            DateTime eveningTime = new DateTime(2011, 1, 1, 23, 59, 59);
            VerifyTimeFormatMatches(eveningTime, TimeDisplayFormat.HideTime, null);
            VerifyTimeFormatMatches(eveningTime, TimeDisplayFormat.HourMin24h, "23:59");
            VerifyTimeFormatMatches(eveningTime, TimeDisplayFormat.HourMinAmPm, "11:59 PM");
            VerifyTimeFormatMatches(eveningTime, TimeDisplayFormat.HourMinSec24h, "23:59:59");
            VerifyTimeFormatMatches(eveningTime, TimeDisplayFormat.HourMinSecAmPm, "11:59:59 PM");
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        [ExpectedException(typeof(ArgumentException))]
        public void FormatInvalidTimeForConversationTest()
        {
            DateTime arbitraryTime = new DateTime(2011, 5, 12, 23, 6, 9);
            VerifyTimeFormatMatches(arbitraryTime, TimeDisplayFormat.Unknown, "shouldn't return a value");
        }
    }
}

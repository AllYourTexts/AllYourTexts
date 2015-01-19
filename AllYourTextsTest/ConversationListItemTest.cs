using AllYourTextsUi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AllYourTextsLib.Framework;
using DummyData;
using AllYourTextsLib;

namespace AllYourTextsTest
{
    
    
    /// <summary>
    ///This is a test class for ConversationListItemTest and is intended
    ///to contain all ConversationListItemTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ConversationListItemTest
    {

        private void VerifyConversationDescription(IConversation conversation, string descriptionExpected)
        {
            ConversationListItem listItem = new ConversationListItem(conversation);
            string descriptionActual = listItem.GetDescription(conversation);
            Assert.AreEqual(descriptionExpected, descriptionActual);
        }

        private void VerifyConversationDescription(DummyPhoneNumberId phoneNumberId, string descriptionExpected)
        {
            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(phoneNumberId);
            VerifyConversationDescription(conversation, descriptionExpected);
        }

        private void VerifyMergedConversationDescription(DummyContactId contactId, string descriptionExpected)
        {
            IConversation conversation = DummyConversationDataGenerator.GetMergedConversation(contactId);
            VerifyConversationDescription(conversation, descriptionExpected);
        }

        private void VerifyChatConversationDescription(DummyChatRoomId chatRoomId, string descriptionExpected)
        {
            IConversation conversation = DummyConversationDataGenerator.GetChatConversation(chatRoomId);
            VerifyConversationDescription(conversation, descriptionExpected);
        }

        /// <summary>
        ///A test for GetDescription
        ///</summary>
        [TestMethod()]
        public void GetDescriptionTest()
        {
            VerifyConversationDescription(DummyPhoneNumberId.ObamaCell, "Barack Obama (" + '\u200E' + "202-555-1600" + '\u202C' + ")");
            VerifyConversationDescription(DummyPhoneNumberId.UnknownEagle, "Unknown Sender (" + '\u200E' + "827-555-0972" + '\u202C' + ")");
            VerifyMergedConversationDescription(DummyContactId.Davola, "Joe Davola (2 numbers)");
            VerifyChatConversationDescription(DummyChatRoomId.ChatRoomA, "Group Chat (Anthony W., Barack O.)");
            VerifyChatConversationDescription(DummyChatRoomId.ChatRoomC, "Group Chat (Anthony W., Barack O., Harry L., ...)");

            //
            // This is actually not correct because the last name should be left of the first name because Hebrew is RTL, but good enough for right now.
            //

            VerifyConversationDescription(DummyPhoneNumberId.IsraeliDanCell, "מיכה" + " " + "פול" + " (" + '\u200E' + "9720523115679" + '\u202C' + ")");
        }
    }
}

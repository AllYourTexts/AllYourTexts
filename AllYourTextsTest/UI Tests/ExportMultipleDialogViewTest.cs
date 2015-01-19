using AllYourTextsUi;
using AllYourTextsUi.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AllYourTextsUi.Exporting;
using DummyData;
using AllYourTextsLib.Framework;
using AllYourTextsUi.Models;
using Moq;

namespace AllYourTextsTest
{
    [TestClass()]
    public class ExportMultipleDialogModelTest
    {
        private void VerifyExportErrorString(IConversation conversation, Exception ex, string descriptionExpected)
        {
            ExportError exportError = new ExportError(conversation, ex);
            var mockDescriptionHelper = new Mock<IConversationDescriptionHelper>();

            mockDescriptionHelper.Setup(x => x.GetDescription(It.IsAny<IConversation>()))
                .Returns("<some conversation>");

            ExportErrorFormatter errorFormatter = new ExportErrorFormatter(mockDescriptionHelper.Object);
            string descriptionActual = errorFormatter.Format(exportError);
            Assert.AreEqual(descriptionExpected, descriptionActual);
        }
        private void VerifyExportErrorString(DummyPhoneNumberId phoneNumberId, Exception ex, string descriptionExpected)
        {
            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(phoneNumberId);
            VerifyExportErrorString(conversation, ex, descriptionExpected);
        }

        private void VerifyExportErrorString(DummyContactId contactId, Exception ex, string descriptionExpected)
        {
            IConversation conversation = DummyConversationDataGenerator.GetMergedConversation(contactId);
            VerifyExportErrorString(conversation, ex, descriptionExpected);
        }

        private void VerifyExportErrorString(DummyChatRoomId chatRoomId, Exception ex, string descriptionExpected)
        {
            IConversation conversation = DummyConversationDataGenerator.GetChatConversation(chatRoomId);
            VerifyExportErrorString(conversation, ex, descriptionExpected);
        }

        /// <summary>
        ///A test for GetDescription
        ///</summary>
        [TestMethod()]
        public void ExportErrorToStringTest()
        {
            VerifyExportErrorString(DummyPhoneNumberId.ObamaCell,
                                    new ArgumentException("Not Barack!"),
                                    "Error in <some conversation>: (System.ArgumentException) Not Barack!");
            VerifyExportErrorString(DummyPhoneNumberId.UnknownEagle,
                                    new DivideByZeroException("Can't divide by zero"),
                                    "Error in <some conversation>: (System.DivideByZeroException) Can't divide by zero");
            VerifyExportErrorString(DummyContactId.Davola,
                                    new Exception("unspecified error"),
                                    "Error in <some conversation>: (System.Exception) unspecified error");
            VerifyExportErrorString(DummyChatRoomId.ChatRoomA,
                                    new Exception("unspecified error"),
                                    "Error in <some conversation>: (System.Exception) unspecified error");
        }
    }
}

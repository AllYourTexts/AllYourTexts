using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsUi.Exporting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AllYourTextsLib.Framework;
using DummyData;
using Moq;
using AllYourTextsUi.Framework;
namespace AllYourTextsUi.Exporting.Tests
{
    [TestClass()]
    public class ExportedFilenameGeneratorTests
    {
        private void VerifyConversationFilenameStartsWith(IConversation conversation, string conversationDescription, string filenamePrefixExpected)
        {
            var mockDescriptionHelper = new Mock<IConversationDescriptionHelper>();
            mockDescriptionHelper.Setup(x => x.GetDescription(It.IsAny<IConversation>()))
                .Returns(conversationDescription);

            var mockClock = new Mock<IClock>();
            mockClock.Setup(x => x.CurrentTime())
                .Returns(new DateTime(2012, 10, 27));

            ExportedFilenameGenerator filenameGenerator = new ExportedFilenameGenerator(mockDescriptionHelper.Object, mockClock.Object);
            string filenameSuggestionActual = filenameGenerator.CreateExportFilenameSuggestion(conversation);
            Assert.AreEqual(filenamePrefixExpected, filenameSuggestionActual);
        }

        [TestMethod()]
        public void CreateExportFilenameSuggestionSingleTest()
        {
            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.ObamaCell);
            VerifyConversationFilenameStartsWith(conversation, "Barack Obama (202-555-1600)", "iPhone Text History - Barack Obama (202-555-1600) - 2012-10-27");
        }

        [TestMethod()]
        public void CreateExportFilenameSuggestionInvalidCharsTest()
        {
            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.CrackerJackOffice);
            VerifyConversationFilenameStartsWith(conversation, "Cracker_Jack (*9977)", "iPhone Text History - Cracker_Jack (_9977) - 2012-10-27");
        }

        [TestMethod()]
        public void CreateExportFolderNameSuggestionTest()
        {
            var mockDescriptionHelper = new Mock<IConversationDescriptionHelper>();
            mockDescriptionHelper.Setup(x => x.GetDescription(It.IsAny<IConversation>()))
                .Returns("<some conversation>");

            var mockClock = new Mock<IClock>();
            mockClock.Setup(x => x.CurrentTime())
                .Returns(new DateTime(2012, 10, 27));
            
            ExportedFilenameGenerator filenameGenerator = new ExportedFilenameGenerator(mockDescriptionHelper.Object, mockClock.Object);
            Assert.AreEqual("iPhone Text History Backup - 2012-10-27", filenameGenerator.CreateExportFolderNameSuggestion());
        }
    }
}

using AllYourTextsUi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DummyData;
using AllYourTextsLib.Framework;
using AllYourTextsLib;
using AllYourTextsLib.Conversation;
using System.IO;
using AllYourTextsTest.Mock_Objects;
using Moq;
using AllYourTextsUi.Exporting;
using System.Collections.Generic;

namespace AllYourTextsTest
{
    
    /// <summary>
    ///This is a test class for ConversationExporterHtmlTest and is intended
    ///to contain all ConversationExporterHtmlTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ConversationExporterHtmlTest
    {
        private void VerifyTitleMatchesExpected(IConversation conversation, string titleExpected)
        {
            ConversationExporterHtml_Accessor target = new ConversationExporterHtml_Accessor(new MockFileSystem());
            string titleActual = target.CreateTitle(conversation);
            Assert.AreEqual(titleExpected, titleActual);
        }

        private void VerifyTitleMatchesExpected(DummyPhoneNumberId phoneNumberId, string titleExpected)
        {
            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(phoneNumberId);
            VerifyTitleMatchesExpected(conversation, titleExpected);
        }

        private void VerifyTitleMatchesExpected(DummyChatRoomId chatRoomId, string titleExpected)
        {
            IConversation conversation = DummyConversationDataGenerator.GetChatConversation(chatRoomId);
            VerifyTitleMatchesExpected(conversation, titleExpected);
        }

        /// <summary>
        ///A test for CreateTitle
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void CreateTitleTest()
        {
            VerifyTitleMatchesExpected(DummyPhoneNumberId.ObamaCell, "Barack Obama - Conversation History");
            VerifyTitleMatchesExpected(DummyPhoneNumberId.BobbyCssOffice, "Bo&lt;b&gt;b&lt;/b&gt;by Css - Conversation History");
            VerifyTitleMatchesExpected(DummyPhoneNumberId.UnknownEagle, "Unknown Sender - Conversation History");
            VerifyTitleMatchesExpected(DummyChatRoomId.ChatRoomA, "Group Chat - Conversation History");
        }

        private void VerifyHeaderContentTitleLineMatches(DummyPhoneNumberId phoneNumberId, string titleLineExpected)
        {
            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(phoneNumberId);
            VerifyHeaderContentTitleLineMatchesByConversation(conversation, titleLineExpected);
        }

        private void VerifyHeaderContentTitleLineMatches(DummyChatRoomId chatRoomId, string titleLineExpected)
        {
            IConversation conversation = DummyConversationDataGenerator.GetChatConversation(chatRoomId);
            VerifyHeaderContentTitleLineMatchesByConversation(conversation, titleLineExpected);
        }

        private void VerifyHeaderContentTitleLineMatchesByConversation(IConversation conversation, string titleLineExpected)
        {
            ConversationExporterHtml_Accessor exporter = new ConversationExporterHtml_Accessor(new MockFileSystem());
            string titleLineActual = exporter.CreateContentHeaderTitleLine(conversation);
            Assert.AreEqual(titleLineExpected, titleLineActual);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void CreateSingleNumberExportTest()
        {
            VerifyHeaderContentTitleLineMatches(DummyPhoneNumberId.ObamaCell, "iPhone Text (SMS) Conversation History with <b>Barack Obama</b> - Phone Number: 202-555-1600.");
            VerifyHeaderContentTitleLineMatches(DummyPhoneNumberId.BobbyCssOffice, "iPhone Text (SMS) Conversation History with <b>Bo&lt;b&gt;b&lt;/b&gt;by Css</b> - Phone Number: 206-555-1974.");
            VerifyHeaderContentTitleLineMatches(DummyPhoneNumberId.UnknownEagle, "iPhone Text (SMS) Conversation History with <b>Unknown Sender</b> - Phone Number: 827-555-0972.");
            VerifyHeaderContentTitleLineMatches(DummyChatRoomId.ChatRoomB, "iPhone Group Text Chat History<br />\r\n" +
                                                                           "Participants:<br />\r\n" +
                                                                           "&nbsp;&nbsp;&nbsp;&nbsp;<b>Tony Harver</b> - 305-555-0925<br />\r\n" + 
                                                                           "&nbsp;&nbsp;&nbsp;&nbsp;<b>Victoria Harver</b> - 305-555-7260");
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void CreateMultiNumberExportTest()
        {
            IConversation conversationCell = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.DavolaCell);
            IConversation conversationiPhone = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.DavolaiPhone);
            IConversation conversationMerged = new MergedConversation(conversationCell, conversationiPhone);
            VerifyHeaderContentTitleLineMatchesByConversation(conversationMerged, "iPhone Text (SMS) Conversation History with <b>Joe Davola</b> - Phone Numbers: 212-555-8728, 646-555-9189.");
        }


        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void ExportAttachmentErrorStillExportsConversation()
        {
            MockFileSystem mockFileSystem = new MockFileSystem();
            ConversationExporterHtml exporter = new ConversationExporterHtml(mockFileSystem);

            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.FrankieCoolPicsCell);
            MockDisplayOptions mockDisplayOptions = new MockDisplayOptions();
            List<ExportError> exportErrors = exporter.Export(conversation, mockDisplayOptions, @"C:\fakepath\backup\exports\frankie-coolpics_9-12-2012.html");
            Assert.AreEqual(1, exportErrors.Count);
            Assert.IsTrue(mockFileSystem.FileExists(@"C:\fakepath\backup\exports\frankie-coolpics_9-12-2012.html"));
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void ImageAttachmentTest()
        {
            MockFileSystem mockFileSystem = new MockFileSystem();
            ConversationExporterHtml exporter = new ConversationExporterHtml(mockFileSystem);
            const string attachmentPathOriginal = @"C:\fakepath\backup\082308302382";

            StreamWriter sw = new StreamWriter(mockFileSystem.CreateNewFile(attachmentPathOriginal));
            sw.Write("This is attachment file data!");

            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.FrankieCoolPicsCell);
            MockDisplayOptions mockDisplayOptions = new MockDisplayOptions();
            List<ExportError> exportErrors = exporter.Export(conversation, mockDisplayOptions, @"C:\fakepath\backup\exports\frankie-coolpics_9-12-2012.html");

            Assert.AreEqual(0, exportErrors.Count);
            Assert.AreEqual(1, mockFileSystem.DirectoryCount);
            Assert.AreEqual(3, mockFileSystem.FileCount);
            Assert.IsTrue(mockFileSystem.DirectoryExists(@"C:\fakepath\backup\exports\frankie-coolpics_9-12-2012_attachments"));
            Assert.IsTrue(mockFileSystem.FileExists(@"C:\fakepath\backup\exports\frankie-coolpics_9-12-2012_attachments\IMG_0036.JPG"));
            Assert.IsTrue(mockFileSystem.FileExists(@"C:\fakepath\backup\exports\frankie-coolpics_9-12-2012.html"));
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void ImageAttachmentFileExistsTest()
        {
            MockFileSystem mockFileSystem = new MockFileSystem();
            ConversationExporterHtml exporter = new ConversationExporterHtml(mockFileSystem);
            const string attachmentPathOriginal = @"C:\fakepath\backup\082308302382";

            StreamWriter sw = new StreamWriter(mockFileSystem.CreateNewFile(attachmentPathOriginal));
            sw.Write("This is attachment file data!");

            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.FrankieCoolPicsCell);
            MockDisplayOptions mockDisplayOptions = new MockDisplayOptions();

            // Take up the desired filename so exporter is forced to create a new one.
            mockFileSystem.CreateNewFile(@"C:\fakepath\backup\exports\frankie-coolpics_9-12-2012.html");
            List<ExportError> exportErrors = exporter.Export(conversation, mockDisplayOptions, @"C:\fakepath\backup\exports\frankie-coolpics_9-12-2012.html");

            Assert.AreEqual(0, exportErrors.Count);
            Assert.AreEqual(1, mockFileSystem.DirectoryCount);
            Assert.AreEqual(4, mockFileSystem.FileCount);
            Assert.IsTrue(mockFileSystem.DirectoryExists(@"C:\fakepath\backup\exports\frankie-coolpics_9-12-2012_attachments"));
            Assert.IsTrue(mockFileSystem.FileExists(@"C:\fakepath\backup\exports\frankie-coolpics_9-12-2012_attachments\IMG_0036.JPG"));
            Assert.IsTrue(mockFileSystem.FileExists(@"C:\fakepath\backup\exports\frankie-coolpics_9-12-2012 (2).html"));
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void ImageAttachmentDirectoryExistsTest()
        {
            MockFileSystem mockFileSystem = new MockFileSystem();
            ConversationExporterHtml exporter = new ConversationExporterHtml(mockFileSystem);
            const string attachmentPathOriginal = @"C:\fakepath\backup\082308302382";

            StreamWriter sw = new StreamWriter(mockFileSystem.CreateNewFile(attachmentPathOriginal));
            sw.Write("This is attachment file data!");

            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.FrankieCoolPicsCell);
            MockDisplayOptions mockDisplayOptions = new MockDisplayOptions();

            // Take up the desired directory name so exporter is forced to create a new one.
            mockFileSystem.CreateDirectory(@"C:\fakepath\backup\exports\frankie-coolpics_9-12-2012_attachments");
            List<ExportError> exportErrors = exporter.Export(conversation, mockDisplayOptions, @"C:\fakepath\backup\exports\frankie-coolpics_9-12-2012.html");

            Assert.AreEqual(0, exportErrors.Count);
            Assert.AreEqual(2, mockFileSystem.DirectoryCount);
            Assert.AreEqual(3, mockFileSystem.FileCount);
            Assert.IsTrue(mockFileSystem.DirectoryExists(@"C:\fakepath\backup\exports\frankie-coolpics_9-12-2012_attachments (2)"));
            Assert.IsTrue(mockFileSystem.FileExists(@"C:\fakepath\backup\exports\frankie-coolpics_9-12-2012_attachments (2)\IMG_0036.JPG"));
            Assert.IsTrue(mockFileSystem.FileExists(@"C:\fakepath\backup\exports\frankie-coolpics_9-12-2012.html"));
        }

        /// <summary>
        /// Reproduce a very unusual error where exporting chat conversations with attachments failed if at
        /// least one participant had no last name.
        /// </summary>
        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void ChatConversationImageAttachmentNoLastNameTest()
        {
            MockFileSystem mockFileSystem = new MockFileSystem();
            ConversationExporterHtml exporter = new ConversationExporterHtml(mockFileSystem);
            const string attachmentPathOriginal = @"C:\fakepath\backup\082308302382";

            StreamWriter sw = new StreamWriter(mockFileSystem.CreateNewFile(attachmentPathOriginal));
            sw.Write("This is attachment file data!");

            var mockContactA = new Mock<IContact>();
            mockContactA.Setup(x => x.FirstName).Returns("James");
            mockContactA.Setup(x => x.LastName).Returns((string)null);
            var mockContactB = new Mock<IContact>();
            mockContactB.Setup(x => x.FirstName).Returns("Peter");
            mockContactB.Setup(x => x.LastName).Returns("Horn");
            ContactList contactList = new ContactList(new IContact[] { mockContactA.Object, mockContactB.Object });
            
            var mockConversation = new Mock<IConversation>();
            mockConversation.Setup(x => x.AssociatedContacts).Returns(contactList);
            
            List<IMessageAttachment> attachments = new List<IMessageAttachment>();
            attachments.Add(new MessageAttachment(1, AttachmentType.Image, attachmentPathOriginal, "IMG_0123.JPG"));

            var mockMessage = new Mock<IConversationMessage>();
            mockMessage.Setup(x => x.Attachments).Returns(attachments);
            mockMessage.Setup(x => x.HasAttachments()).Returns(true);

            List<IConversationMessage> messages = new List<IConversationMessage>(new IConversationMessage[] {mockMessage.Object});
            mockConversation.Setup(x => x.GetEnumerator()).Returns(messages.GetEnumerator());
            mockConversation.Setup(x => x.MessageCount).Returns(1);

            MockDisplayOptions mockDisplayOptions = new MockDisplayOptions();
            List<ExportError> exportErrors = exporter.Export(mockConversation.Object,
                                                             mockDisplayOptions,
                                                             @"C:\fakepath\backup\exports\dummy-chat_2014-11-02.html");

            Assert.AreEqual(1, mockFileSystem.DirectoryCount);
            Assert.AreEqual(3, mockFileSystem.FileCount);
            Assert.IsTrue(mockFileSystem.DirectoryExists(@"C:\fakepath\backup\exports\dummy-chat_2014-11-02_attachments"));
            Assert.IsTrue(mockFileSystem.FileExists(@"C:\fakepath\backup\exports\dummy-chat_2014-11-02_attachments\IMG_0123.JPG"));
            Assert.IsTrue(mockFileSystem.FileExists(@"C:\fakepath\backup\exports\dummy-chat_2014-11-02.html"));
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void VideoAttachmentTest()
        {
            MockFileSystem mockFileSystem = new MockFileSystem();
            ConversationExporterHtml exporter = new ConversationExporterHtml(mockFileSystem);

            StreamWriter sw = new StreamWriter(mockFileSystem.CreateNewFile(@"C:\fakepath\backup\056798632135464"));
            sw.Write("This is attachment file data!");

            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.JerryCoolVidsCell);
            MockDisplayOptions mockDisplayOptions = new MockDisplayOptions();
            List<ExportError> exportErrors = exporter.Export(conversation, mockDisplayOptions, @"C:\fakepath\backup\exports\Jerry-Coolvids_9-13-2012.html");

            Assert.AreEqual(0, exportErrors.Count);
            Assert.AreEqual(1, mockFileSystem.DirectoryCount);
            Assert.AreEqual(4, mockFileSystem.FileCount); // 3 exported files + dummy source attachment
            Assert.IsTrue(mockFileSystem.DirectoryExists(@"C:\fakepath\backup\exports\Jerry-Coolvids_9-13-2012_attachments"));
            Assert.IsTrue(mockFileSystem.FileExists(@"C:\fakepath\backup\exports\Jerry-Coolvids_9-13-2012_attachments\video_icon.png"));
            Assert.IsTrue(mockFileSystem.FileExists(@"C:\fakepath\backup\exports\Jerry-Coolvids_9-13-2012_attachments\VIDEO_0015.MOV"));
            Assert.IsTrue(mockFileSystem.FileExists(@"C:\fakepath\backup\exports\Jerry-Coolvids_9-13-2012.html"));
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void AudioAttachmentTest()
        {
            MockFileSystem mockFileSystem = new MockFileSystem();
            ConversationExporterHtml exporter = new ConversationExporterHtml(mockFileSystem);

            StreamWriter sw = new StreamWriter(mockFileSystem.CreateNewFile(@"C:\fakepath\backup\056798632135464"));
            sw.Write("This is mock audio data!");

            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.WolfmanJackCell);
            List<ExportError> exportErrors = exporter.Export(conversation, new MockDisplayOptions(), @"C:\fakepath\backup\exports\WolfmanJack_10-25-2014.html");

            Assert.AreEqual(0, exportErrors.Count);
            Assert.AreEqual(1, mockFileSystem.DirectoryCount);
            Assert.AreEqual(4, mockFileSystem.FileCount); // 3 exported files + dummy source attachment
            Assert.IsTrue(mockFileSystem.DirectoryExists(@"C:\fakepath\backup\exports\WolfmanJack_10-25-2014_attachments"));
            Assert.IsTrue(mockFileSystem.FileExists(@"C:\fakepath\backup\exports\WolfmanJack_10-25-2014_attachments\audio_icon.png"));
            Assert.IsTrue(mockFileSystem.FileExists(@"C:\fakepath\backup\exports\WolfmanJack_10-25-2014_attachments\wolfman_howl.amr"));
            Assert.IsTrue(mockFileSystem.FileExists(@"C:\fakepath\backup\exports\WolfmanJack_10-25-2014.html"));
        }
    }
}

using AllYourTextsUi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using AllYourTextsLib.Framework;
using DummyData;
using AllYourTextsLib;
using AllYourTextsLib.Conversation;
using System.IO;
using AllYourTextsTest.Mock_Objects;

namespace AllYourTextsTest
{

    [TestClass()]
    public class ConversationExporterBaseTest
    {
        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void CreateHeaderSoftwareLineTest()
        {
            ConversationExporterBase_Accessor exporter = new ConversationExporterPlaintext_Accessor(new MockFileSystem());
            Assert.IsTrue(exporter.CreateHeaderSoftwareLine().StartsWith("Exported by AllYourTexts v."));
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void ExportMultipleConversationsTest()
        {
            List<IConversation> conversations = new List<IConversation>();
            conversations.Add(DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.ObamaCell));
            conversations.Add(DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.HarryLooseTieCell));

            const string outputPath = "X:\\backup\\";
            MockFileSystem mockFileSystem = new MockFileSystem();
            ConversationExporterBase exporter = new ConversationExporterPlaintext(mockFileSystem);

            exporter.ExportMultipleConversations(conversations, new MockDisplayOptions(), outputPath, null);

            Assert.IsTrue(mockFileSystem.DirectoryExists(outputPath));
            Assert.AreEqual(2, mockFileSystem.FileCount);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void ExportMultipleConversationsFolderCollisionTest()
        {
            List<IConversation> conversations = new List<IConversation>();
            conversations.Add(DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.ObamaCell));
            conversations.Add(DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.HarryLooseTieCell));

            const string outputPath = "X:\\backup\\";
            MockFileSystem mockFileSystem = new MockFileSystem();
            ConversationExporterBase exporter = new ConversationExporterPlaintext(mockFileSystem);

            exporter.ExportMultipleConversations(conversations, new MockDisplayOptions(), outputPath, null);

            string createdDirectory = "";
            foreach (string directory in mockFileSystem.ListDirectories())
            {
                createdDirectory = directory;
            }

            exporter.ExportMultipleConversations(conversations, new MockDisplayOptions(), outputPath, null);

            Assert.AreEqual(2, mockFileSystem.CountDirectoryContents(createdDirectory));
            Assert.AreEqual(2, mockFileSystem.DirectoryCount);
            Assert.AreEqual(4, mockFileSystem.FileCount);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void ExportMultipleConversationsNameCollisionTest()
        {
            IConversation conversationCell = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.DavolaCell);
            IConversation conversationiPhone = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.DavolaiPhone);
            IConversation conversationMerged = new MergedConversation(conversationCell, conversationiPhone);
            List<IConversation> conversations = new List<IConversation>();
            conversations.Add(conversationMerged);

            string filename = "dummy_filename.txt";

            const string outputPath = "X:\\backup\\";
            MockFileSystem mockFileSystem = new MockFileSystem();
            mockFileSystem.CreateNewFile(Path.Combine(outputPath, filename));

            ConversationExporterBase exporter = new ConversationExporterPlaintext(mockFileSystem);

            exporter.ExportMultipleConversations(conversations, new MockDisplayOptions(), outputPath, null);
            Assert.AreEqual(2, mockFileSystem.FileCount);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void ExportMultipleConversationsEmptyConversationTest()
        {
            List<IConversation> conversations = new List<IConversation>();
            conversations.Add(DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.NeverTexterCell));

            const string outputPath = "X:\\backup\\";
            MockFileSystem mockFileSystem = new MockFileSystem();
            ConversationExporterBase exporter = new ConversationExporterPlaintext(mockFileSystem);

            exporter.ExportMultipleConversations(conversations, new MockDisplayOptions(), outputPath, null);
            Assert.AreEqual(0, mockFileSystem.FileCount);
        }
    }
}

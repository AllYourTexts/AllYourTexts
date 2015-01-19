using AllYourTextsLib.Conversation;
using AllYourTextsLib.Framework;
using AllYourTextsUi;
using DummyData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AllYourTextsUi.Exporting;
using System.IO;
using AllYourTextsTest.Mock_Objects;

namespace AllYourTextsTest
{

    [TestClass()]
    public class ConversationExporterPlaintextTest
    {

        private void VerifyConversationHeaderMatches(IConversation conversation, string headerExpected)
        {
            ConversationExporterPlaintext_Accessor exporter = new ConversationExporterPlaintext_Accessor(new MockFileSystem());
            string headerActual = exporter.CreateHeaderConversationLine(conversation);
            Assert.AreEqual(headerExpected, headerActual);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void CreateSingleNumberExportTest()
        {
            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.ObamaCell);
            VerifyConversationHeaderMatches(conversation, "iPhone Text (SMS) Conversation History with Barack Obama - Phone Number: 202-555-1600.");
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void CreateMultiNumberExportTest()
        {
            IConversation conversationCell = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.DavolaCell);
            IConversation conversationiPhone = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.DavolaiPhone);
            IConversation conversationMerged = new MergedConversation(conversationCell, conversationiPhone);
            VerifyConversationHeaderMatches(conversationMerged, "iPhone Text (SMS) Conversation History with Joe Davola - Phone Numbers: 212-555-8728, 646-555-9189.");
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void CreateUnknownSenderExportTest()
        {
            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.UnknownEagle);
            VerifyConversationHeaderMatches(conversation, "iPhone Text (SMS) Conversation History with Unknown Sender - Phone Number: 827-555-0972.");
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void CreateGroupChatExportTest()
        {
            IConversation conversation = DummyConversationDataGenerator.GetChatConversation(DummyChatRoomId.ChatRoomA);
            VerifyConversationHeaderMatches(conversation, "iPhone Group Text Chat History\r\nParticipants:\r\n\tAnthony Weiner - 212-555-8868\r\n\tBarack Obama - 202-555-1600");
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void WriteConversationContentsTest()
        {
            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.ObamaCell);

            string exportedConversationExpected = "Tuesday, Nov 4, 2008\r\n" +
                                                    "Me (\u200E10:18:05 PM\u202C): Congrats, buddy!\r\n" +
                                                    "Barack Obama (\u200E10:18:40 PM\u202C): Thanks. Couldn't have done it without you!\r\n" +
                                                    "Me (\u200E10:25:58 PM\u202C): np\r\n" +
                                                    "\r\n" +
                                                    "Sunday, May 1, 2011\r\n" +
                                                    "Me (\u200E8:47:27 AM\u202C): Yo, I think I know where Osama Bin Laden is hiding?\r\n" +
                                                    "Barack Obama (\u200E8:50:52 AM\u202C): o rly?\r\n" +
                                                    "Me (\u200E8:51:21 AM\u202C): Yeah, dude. Abottabad, Pakistan. Huge compound. Can't miss it.\r\n" +
                                                    "Barack Obama (\u200E8:51:46 AM\u202C): Sweet. I'll send some navy seals.";

            ConversationExporterPlaintext_Accessor conversationExporter = new ConversationExporterPlaintext_Accessor(new MockFileSystem());

            MemoryStream outputStream = new MemoryStream();
            StreamWriter sw = new StreamWriter(outputStream);

            conversationExporter.WriteConversationContents(sw, conversation, new MockDisplayOptions(), new AttachmentExportLocator(null));
            sw.Flush();

            outputStream.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(outputStream);

            string exportedConversationActual = sr.ReadToEnd();
            Assert.AreEqual(exportedConversationExpected, exportedConversationActual);
        }

    }
}

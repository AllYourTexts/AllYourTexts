using AllYourTextsLib;
using AllYourTextsLib.DataReader;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AllYourTextsTest
{
    
    
    /// <summary>
    ///This is a test class for ChatRoomInformationReaderTest and is intended
    ///to contain all ChatRoomInformationReaderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ChatRoomInformationReaderTest
    {

        private readonly byte[] _ParticipantsData = new byte[] {0x62, 0x70, 0x6C, 0x69, 0x73, 0x74, 0x30, 0x30, 0xA2, 0x01, 0x02, 0x5C, 0x2B, 0x31, 0x35, 0x34, 0x30, 0x32, 0x33, 0x39,
                                                                0x32, 0x33, 0x30, 0x34, 0x5C, 0x2B, 0x31, 0x32, 0x30, 0x36, 0x38, 0x31, 0x38, 0x39, 0x32, 0x38, 0x38, 0x08, 0x0B, 0x18,
                                                                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00,
                                                                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x25};
        /// <summary>
        ///A test for ParseItemFromDatabase
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void ParseItemFromDatabaseTest()
        {
            MockChatRoomDatabaseReader mockDatabaseReader = new MockChatRoomDatabaseReader();
            mockDatabaseReader.AddRow("chat901258305184729544", _ParticipantsData);
            ChatRoomInformationReader_Accessor target = new ChatRoomInformationReader_Accessor();
            target.ParseDatabase(mockDatabaseReader);

            ChatRoomInformation expected = new ChatRoomInformation("chat901258305184729544", new string[] { "+15402392304", "+12068189288" });
            ChatRoomInformation actual = target.ParseItemFromDatabase(mockDatabaseReader);
            Assert.AreEqual(expected, actual);
        }
    }
}

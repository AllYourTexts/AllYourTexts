using System.Collections.Generic;
using System.Linq;
using AllYourTextsLib;
using AllYourTextsLib.DataReader;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AllYourTextsTest
{
    
    /// <summary>
    ///This is a test class for ChatRoomInformationReaderiOS6Test and is intended
    ///to contain all ChatRoomInformationReaderiOS6Test Unit Tests
    ///</summary>
    [TestClass()]
    public class ChatRoomInformationReaderiOS6Test
    {

        /// <summary>
        ///A test for CoalesceChatRoomInformation
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void CoalesceChatRoomInformationTest()
        {
            ChatRoomInformation[] ungrouped = 
                {
                    new ChatRoomInformation("chat895403", new string[] { "+1-914-555-3802" } ),
                    new ChatRoomInformation("chat895403", new string[] { "+1-767-555-1286" } ),
                    new ChatRoomInformation("chat895403", new string[] { "+1-148-555-5677" } ),
                    new ChatRoomInformation("chat658971", new string[] { "+1-954-555-3276" } ),
                    new ChatRoomInformation("chat658971", new string[] { "+1-212-555-1010" } )
                };
            ChatRoomInformation[] groupedExpected =
                {
                    new ChatRoomInformation("chat895403", new string[] { "+1-914-555-3802", "+1-767-555-1286", "+1-148-555-5677" } ),
                    new ChatRoomInformation("chat658971", new string[] { "+1-954-555-3276", "+1-212-555-1010" } )
                };

            List<ChatRoomInformation> groupedActual = ChatRoomInformationReaderiOS6_Accessor.CoalesceChatRoomInformation(ungrouped);

            Assert.IsTrue(Enumerable.SequenceEqual(groupedExpected, groupedActual));
        }
    }
}

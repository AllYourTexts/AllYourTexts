using AllYourTextsUi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AllYourTextsUi.Framework;
using AllYourTextsLib.Framework;
using DummyData;
using AllYourTextsTest.Mock_Objects;

namespace AllYourTextsTest
{
    
    
    /// <summary>
    ///This is a test class for ConversationRendererPlaintextTest and is intended
    ///to contain all ConversationRendererPlaintextTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ConversationRendererPlaintextTest
    {

        private void VerifyRenderedConversationMatchesExpected(DummyPhoneNumberId phoneNumberId, string renderedConversationExpected)
        {
            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(phoneNumberId);

            ConversationRendererPlaintext renderer = new ConversationRendererPlaintext(new MockDisplayOptions(), conversation);

            string renderedConversationActual = renderer.RenderMessagesAsString(ConversationRendererBase.RenderAllMessages);
            Assert.AreEqual(renderedConversationExpected, renderedConversationActual);
        }

        /// <summary>
        ///A test for RenderMessages
        ///</summary>
        [TestMethod()]
        public void RenderMessagesStandardTest()
        {
            string renderedConversationExpected = "Tuesday, Nov 4, 2008\r\n" +
                                                    "Me (\u200E10:18:05 PM\u202C): Congrats, buddy!\r\n" +
                                                    "Barack Obama (\u200E10:18:40 PM\u202C): Thanks. Couldn't have done it without you!\r\n" +
                                                    "Me (\u200E10:25:58 PM\u202C): np\r\n" +
                                                    "\r\n" +
                                                    "Sunday, May 1, 2011\r\n" +
                                                    "Me (\u200E8:47:27 AM\u202C): Yo, I think I know where Osama Bin Laden is hiding?\r\n" +
                                                    "Barack Obama (\u200E8:50:52 AM\u202C): o rly?\r\n" +
                                                    "Me (\u200E8:51:21 AM\u202C): Yeah, dude. Abottabad, Pakistan. Huge compound. Can't miss it.\r\n" +
                                                    "Barack Obama (\u200E8:51:46 AM\u202C): Sweet. I'll send some navy seals.";

            VerifyRenderedConversationMatchesExpected(DummyPhoneNumberId.ObamaCell, renderedConversationExpected);
        }

        [TestMethod()]
        public void RenderMessagesEmptyTest()
        {
            string renderedConversationExpected = ConversationRendererPlaintext_Accessor._noConversationMessage;

            VerifyRenderedConversationMatchesExpected(DummyPhoneNumberId.NeverTexterCell, renderedConversationExpected);
        }

        [TestMethod()]
        public void RenderMessagesSingleParagraphTest()
        {
            string renderedConversationExpected = "Monday, Oct 17, 2011\r\n" +
                                                    "Me (\u200E3:45:40 PM\u202C): Whatup Cracker\\Jack? Love the new number.\r\n" +
                                                    "Cracker\\Jack (\u200E3:45:43 PM\u202C): Thanks, dog!";

            VerifyRenderedConversationMatchesExpected(DummyPhoneNumberId.CrackerJackOffice, renderedConversationExpected);
        }
    }
}

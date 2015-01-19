using System;
using AllYourTextsLib;
using AllYourTextsLib.Conversation;
using AllYourTextsLib.Framework;
using DummyData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AllYourTextsTest
{

    [TestClass()]
    public class ConversationManagerTest
    {

        public static void VerifySuccessfulCompletion(MockLoadingProgressCallback progressCallback)
        {
            Assert.AreEqual(false, progressCallback.IsCanceled);
            Assert.AreEqual(true, progressCallback.IsEnded);
            Assert.AreEqual(3, progressCallback.PhaseHistory.Count);
            Assert.AreEqual(0, progressCallback.Minimum);
            Assert.AreEqual(progressCallback.Maximum, progressCallback.LastIncrementedPosition);
            Assert.AreEqual(LoadingPhase.ReadingContacts, progressCallback.PhaseHistory[0]);
            Assert.AreEqual(LoadingPhase.ReadingChatInformation, progressCallback.PhaseHistory[1]);
            Assert.AreEqual(LoadingPhase.ReadingMessages, progressCallback.PhaseHistory[2]);
        }

        private ConversationManager CreateConversationManager(DummyContactId[] contactIds, DummyPhoneNumberId[] messageSetIds, DummyChatRoomId[] chatRoomIds, ILoadingProgressCallback progressCallback)
        {
            if (progressCallback != null)
            {
                int messageCount = DummyConversationDataGenerator.GetMessageCount(messageSetIds);
                int workEstimate = ConversationManager.GetWorkEstimate(contactIds.Length, messageCount, chatRoomIds.Length, 0);
                progressCallback.Begin(workEstimate);
            }

            ConversationManager conversationManager = DummyConversationDataGenerator.GetConversationManager(contactIds, messageSetIds, chatRoomIds, progressCallback);

            if (progressCallback != null)
            {
                progressCallback.End();
            }

            return conversationManager;
        }

        private ConversationManager CreateConversationManager(DummyContactId[] contactIds, DummyPhoneNumberId[] messageSetIds, ILoadingProgressCallback progressCallback)
        {
            DummyChatRoomId[] chatRoomIds = { };
            return CreateConversationManager(contactIds, messageSetIds, chatRoomIds, progressCallback);
        }

        [TestMethod()]
        public void EmptyTest()
        {
            MockLoadingProgressCallback progressCallback = new MockLoadingProgressCallback();

            DummyContactId[] dummyContactIds = { };
            DummyPhoneNumberId[] DummyPhoneNumberIds = { };

            ConversationManager conversationManager = CreateConversationManager(dummyContactIds, DummyPhoneNumberIds, progressCallback);

            Assert.AreEqual(0, conversationManager.ConversationCount);

            Assert.AreEqual(0, progressCallback.Maximum);
            VerifySuccessfulCompletion(progressCallback);
        }

        [TestMethod()]
        public void SingleContactTest()
        {
            MockLoadingProgressCallback progressCallback = new MockLoadingProgressCallback();

            DummyContactId[] dummyContactIds = { DummyContactId.Davola };
            DummyPhoneNumberId[] DummyPhoneNumberIds = { DummyPhoneNumberId.DavolaCell, DummyPhoneNumberId.DavolaiPhone };

            ConversationManager conversationManager = CreateConversationManager(dummyContactIds, DummyPhoneNumberIds, progressCallback);

            Assert.AreEqual(2, conversationManager.ConversationCount);

            Assert.AreEqual(18, progressCallback.Maximum);
            VerifySuccessfulCompletion(progressCallback);
        }

        [TestMethod()]
        public void ConversationWithExactDuplicatesTest()
        {
            DummyContactId[] dummyContactIds = { DummyContactId.Davola, DummyContactId.Davola };
            DummyPhoneNumberId[] DummyPhoneNumberIds = { DummyPhoneNumberId.DavolaCell, DummyPhoneNumberId.DavolaiPhone };

            MockLoadingProgressCallback progressCallbackUnmerged = new MockLoadingProgressCallback();
            ConversationManager conversationManagerUnmerged = CreateConversationManager(dummyContactIds, DummyPhoneNumberIds, progressCallbackUnmerged);

            Assert.AreEqual(2, conversationManagerUnmerged.ConversationCount);
            VerifySuccessfulCompletion(progressCallbackUnmerged);
        }

        [TestMethod()]
        public void ConversationWithNumberDuplicatesTest()
        {
            DummyContactId[] dummyContactIds = { DummyContactId.Davola, DummyContactId.DavolaNumberDuplicate };
            DummyPhoneNumberId[] DummyPhoneNumberIds = { DummyPhoneNumberId.DavolaCell, DummyPhoneNumberId.DavolaiPhone };

            MockLoadingProgressCallback progressCallbackUnmerged = new MockLoadingProgressCallback();
            ConversationManager conversationManagerUnmerged = CreateConversationManager(dummyContactIds, DummyPhoneNumberIds, progressCallbackUnmerged);

            Assert.AreEqual(2, conversationManagerUnmerged.ConversationCount);
            VerifySuccessfulCompletion(progressCallbackUnmerged);

            IContact contactCellExpected = DummyConversationDataGenerator.GetContact(DummyPhoneNumberId.DavolaCell);
            IConversation conversationCellActual = conversationManagerUnmerged.GetConversation(0);
            Assert.AreEqual(1, conversationCellActual.AssociatedContacts.Count);
            Assert.AreEqual(contactCellExpected, conversationCellActual.AssociatedContacts[0]);

            IContact contactiPhoneExpected = DummyConversationDataGenerator.GetContact(DummyPhoneNumberId.DavolaiPhone);
            IConversation conversationiPhoneActual = conversationManagerUnmerged.GetConversation(1);
            Assert.AreEqual(1, conversationiPhoneActual.AssociatedContacts.Count);
            Assert.AreEqual(contactiPhoneExpected, conversationiPhoneActual.AssociatedContacts[0]);
        }

        [TestMethod()]
        public void UnmergedMultipleConversationTest()
        {
            MockLoadingProgressCallback progressCallback = new MockLoadingProgressCallback();

            DummyContactId[] dummyContactIds = { DummyContactId.Davola, DummyContactId.Obama };
            DummyPhoneNumberId[] DummyPhoneNumberIds = { DummyPhoneNumberId.DavolaCell, DummyPhoneNumberId.DavolaiPhone, DummyPhoneNumberId.ObamaCell, DummyPhoneNumberId.UnknownLawnmower };

            ConversationManager conversationManager = CreateConversationManager(dummyContactIds, DummyPhoneNumberIds, progressCallback);

            Assert.AreEqual(4, conversationManager.ConversationCount);

            Assert.AreEqual(36, progressCallback.Maximum);
            VerifySuccessfulCompletion(progressCallback);
        }

        [TestMethod()]
        public void FindConversationTest()
        {
            MockLoadingProgressCallback progressCallback = new MockLoadingProgressCallback();

            DummyContactId[] dummyContactIds = { DummyContactId.Davola, DummyContactId.Obama };
            DummyPhoneNumberId[] DummyPhoneNumberIds = { DummyPhoneNumberId.DavolaCell, DummyPhoneNumberId.DavolaiPhone, DummyPhoneNumberId.ObamaCell, DummyPhoneNumberId.UnknownLawnmower };

            ConversationManager conversationManager = CreateConversationManager(dummyContactIds, DummyPhoneNumberIds, progressCallback);
            VerifySuccessfulCompletion(progressCallback);

            int conversationIndexExpected = 1;
            IConversation conversationToFind = conversationManager.GetConversation(conversationIndexExpected);
            int conversationIndexActual = conversationManager.FindConversationIndex(conversationToFind);
            Assert.AreEqual(conversationIndexExpected, conversationIndexActual);

            IConversation missingConversation = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.ReliableLarryOffice);
            int missingConversationIndexActual = conversationManager.FindConversationIndex(missingConversation);
            Assert.AreEqual(-1, missingConversationIndexActual);
        }

        private void CancelConversationLoad(LoadingPhase loadingPhaseToCancel)
        {
            DummyContactId[] dummyContactIds = { DummyContactId.Davola, DummyContactId.Obama };
            DummyPhoneNumberId[] DummyPhoneNumberIds = { DummyPhoneNumberId.DavolaCell, DummyPhoneNumberId.DavolaiPhone, DummyPhoneNumberId.ObamaCell, DummyPhoneNumberId.UnknownLawnmower };

            MockLoadingProgressCallbackCancellable progressCallback = new MockLoadingProgressCallbackCancellable(loadingPhaseToCancel);
            ConversationManager conversationManager = CreateConversationManager(dummyContactIds, DummyPhoneNumberIds, progressCallback);
        }

        [TestMethod()]
        [ExpectedException(typeof(OperationCanceledException))]
        public void CanceledConversationDuringContactLoadTest()
        {
            CancelConversationLoad(LoadingPhase.ReadingContacts);
        }

        [TestMethod()]
        [ExpectedException(typeof(OperationCanceledException))]
        public void CanceledConversationDuringMessageLoadTest()
        {
            CancelConversationLoad(LoadingPhase.ReadingMessages);
        }

        [TestMethod()]
        public void EmptyUnmergedConversationTest()
        {
            MockLoadingProgressCallback progressCallback = new MockLoadingProgressCallback();

            DummyContactId[] dummyContactIds = { DummyContactId.NeverTexter };
            DummyPhoneNumberId[] DummyPhoneNumberIds = {  };

            ConversationManager conversationManager = CreateConversationManager(dummyContactIds, DummyPhoneNumberIds, progressCallback);

            Assert.AreEqual(2, conversationManager.ConversationCount);

            Assert.AreEqual(1, progressCallback.Maximum);
            VerifySuccessfulCompletion(progressCallback);
        }

        [TestMethod()]
        public void UnknownConversationTest()
        {
            MockLoadingProgressCallback progressCallback = new MockLoadingProgressCallback();

            DummyContactId[] dummyContactIds = { };
            DummyPhoneNumberId[] DummyPhoneNumberIds = { DummyPhoneNumberId.UnknownLawnmower };

            ConversationManager conversationManager = CreateConversationManager(dummyContactIds, DummyPhoneNumberIds, progressCallback);

            Assert.AreEqual(1, conversationManager.ConversationCount);
            foreach (IConversationMessage message in conversationManager.GetConversation(0))
            {
                Assert.IsNotNull(message.Contact);
                Assert.AreEqual(Contact.UnknownContactId, message.Contact.ContactId);
            }
            
            VerifySuccessfulCompletion(progressCallback);
        }

        [TestMethod()]
        public void DoubleUnknownConversationTest()
        {
            MockLoadingProgressCallback progressCallback = new MockLoadingProgressCallback();

            DummyContactId[] dummyContactIds = { };
            DummyPhoneNumberId[] DummyPhoneNumberIds = { DummyPhoneNumberId.UnknownLawnmower, DummyPhoneNumberId.UnknownEagle };

            ConversationManager conversationManager = CreateConversationManager(dummyContactIds, DummyPhoneNumberIds, progressCallback);

            Assert.AreEqual(2, conversationManager.ConversationCount);

            Assert.AreEqual(11, progressCallback.Maximum);
            VerifySuccessfulCompletion(progressCallback);
        }

        [TestMethod()]
        public void NullCallbackTest()
        {
            DummyContactId[] dummyContactIds = { DummyContactId.Davola, DummyContactId.Obama };
            DummyPhoneNumberId[] DummyPhoneNumberIds = { DummyPhoneNumberId.DavolaCell, DummyPhoneNumberId.DavolaiPhone, DummyPhoneNumberId.ObamaCell };

            ConversationManager conversationManager = CreateConversationManager(dummyContactIds, DummyPhoneNumberIds, null);

            int conversationCountExpected = DummyConversationDataGenerator.GetPhoneNumberCount(dummyContactIds);

            Assert.AreEqual(conversationCountExpected, conversationManager.ConversationCount);
        }

        [TestMethod()]
        public void SimpleChatTest()
        {
            DummyContactId[] dummyContactIds = { DummyContactId.Obama, DummyContactId.AnthonyWeiner };
            DummyPhoneNumberId[] dummyPhoneNumberIds = { };
            DummyChatRoomId[] dummyChatRoomIds = { DummyChatRoomId.ChatRoomA };

            ConversationManager conversationManager = CreateConversationManager(dummyContactIds, dummyPhoneNumberIds, dummyChatRoomIds, null);

            IConversation chatConversation = conversationManager.GetConversation(2);
            //Assert.IsNull(chatConversation.GetMessage(0).Contact);
            Assert.IsTrue(chatConversation.GetMessage(0).IsOutgoing);

            IContact contactExpected;
            IContact contactActual;

            contactExpected = DummyConversationDataGenerator.GetContacts(DummyContactId.Obama)[0];
            contactActual = chatConversation.GetMessage(1).Contact;
            Assert.AreEqual(contactExpected, chatConversation.GetMessage(1).Contact);
            Assert.IsFalse(chatConversation.GetMessage(1).IsOutgoing);

            IContact anthonyWeinerContact = DummyConversationDataGenerator.GetContacts(DummyContactId.AnthonyWeiner)[0];
            Assert.AreEqual(anthonyWeinerContact, chatConversation.GetMessage(2).Contact);
            Assert.IsFalse(chatConversation.GetMessage(2).IsOutgoing);
        }

        [TestMethod()]
        public void KnownUnknownChatTest()
        {
            IConversation conversation = DummyConversationDataGenerator.GetChatConversation(DummyChatRoomId.ChatRoomD);
            Assert.AreEqual(2, conversation.AssociatedContacts.Count);
            Assert.IsNotNull(conversation.AssociatedContacts[0]);
            Assert.IsNotNull(conversation.AssociatedContacts[1]);
        }
    }
}

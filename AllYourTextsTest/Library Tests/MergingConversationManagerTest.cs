using AllYourTextsLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using DummyData;
using AllYourTextsLib.Framework;
using AllYourTextsLib.Conversation;

namespace AllYourTextsTest
{
    
    /// <summary>
    ///This is a test class for MergingConversationManagerTest and is intended
    ///to contain all MergingConversationManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MergingConversationManagerTest
    {

        private MergingConversationManager CreateMergingConversationManager(DummyContactId[] contactIds, DummyPhoneNumberId[] messageSetIds, ILoadingProgressCallback progressCallback)
        {
            if (progressCallback != null)
            {
                int workEstimate = ConversationManager.GetWorkEstimate(contactIds.Length, messageSetIds.Length, 0, 0) +
                                   MergingConversationManager.GetWorkEstimateByContacts(contactIds.Length);
                progressCallback.Begin(workEstimate);
            }

            ConversationManager conversationManager = DummyConversationDataGenerator.GetConversationManager(contactIds, messageSetIds, progressCallback);

            if (progressCallback != null)
            {
                progressCallback.UpdateRemaining(MergingConversationManager.GetWorkEstimate(conversationManager.ConversationCount));
            }

            MergingConversationManager mergingConversationManager = new MergingConversationManager(conversationManager, progressCallback);

            if (progressCallback != null)
            {
                progressCallback.End();
            }

            return mergingConversationManager;
        }

        [TestMethod()]
        public void EmptyMergedConversationTest()
        {
            MockLoadingProgressCallback progressCallback = new MockLoadingProgressCallback();

            DummyContactId[] dummyContactIds = { DummyContactId.NeverTexter };
            DummyPhoneNumberId[] DummyPhoneNumberIds = { };

            MergingConversationManager mergingConversationManager = CreateMergingConversationManager(dummyContactIds, DummyPhoneNumberIds, progressCallback);

            Assert.AreEqual(1, mergingConversationManager.ConversationCount);

            int expectedProgressMax = (DummyConversationDataGenerator.GetPhoneNumberCount(dummyContactIds) * 2) + 
                                      DummyConversationDataGenerator.GetMessageCount(DummyPhoneNumberIds);
            Assert.AreEqual(expectedProgressMax, progressCallback.Maximum);
            ConversationManagerTest.VerifySuccessfulCompletion(progressCallback);
        }

        [TestMethod()]
        public void MergedConversationStandardTest()
        {
            MockLoadingProgressCallback progressCallback = new MockLoadingProgressCallback();

            DummyContactId[] dummyContactIds = { DummyContactId.Davola };
            DummyPhoneNumberId[] DummyPhoneNumberIds = { DummyPhoneNumberId.DavolaCell, DummyPhoneNumberId.DavolaiPhone };

            MergingConversationManager mergingConversationManager = CreateMergingConversationManager(dummyContactIds, DummyPhoneNumberIds, progressCallback);

            Assert.AreEqual(1, mergingConversationManager.ConversationCount);
            ConversationManagerTest.VerifySuccessfulCompletion(progressCallback);
        }

        [TestMethod()]
        public void MergedConversationWithExactDuplicatesTest()
        {
            MockLoadingProgressCallback progressCallback = new MockLoadingProgressCallback();

            DummyContactId[] dummyContactIds = { DummyContactId.Davola, DummyContactId.Davola };
            DummyPhoneNumberId[] DummyPhoneNumberIds = { DummyPhoneNumberId.DavolaCell, DummyPhoneNumberId.DavolaiPhone };

            MergingConversationManager mergingConversationManager = CreateMergingConversationManager(dummyContactIds, DummyPhoneNumberIds, progressCallback);

            Assert.AreEqual(1, mergingConversationManager.ConversationCount);
            ConversationManagerTest.VerifySuccessfulCompletion(progressCallback);
        }

        [TestMethod()]
        public void ConversationWithNumberDuplicatesTest()
        {
            MockLoadingProgressCallback progressCallback = new MockLoadingProgressCallback();

            DummyContactId[] dummyContactIds = { DummyContactId.Davola, DummyContactId.DavolaNumberDuplicate };
            DummyPhoneNumberId[] DummyPhoneNumberIds = { DummyPhoneNumberId.DavolaCell, DummyPhoneNumberId.DavolaiPhone };

            MergingConversationManager conversationManagerMerged = CreateMergingConversationManager(dummyContactIds, DummyPhoneNumberIds, progressCallback);

            Assert.AreEqual(1, conversationManagerMerged.ConversationCount);

            IConversation conversation = conversationManagerMerged.GetConversation(0);
            Assert.AreEqual(1, conversation.AssociatedContacts.Count);
            Assert.AreEqual((long)DummyContactId.Davola, conversation.AssociatedContacts[0].ContactId);

            ConversationManagerTest.VerifySuccessfulCompletion(progressCallback);
        }

        [TestMethod()]
        public void MergingNullCallbackTest()
        {
            DummyContactId[] dummyContactIds = { DummyContactId.Davola, DummyContactId.Obama };
            DummyPhoneNumberId[] DummyPhoneNumberIds = { DummyPhoneNumberId.DavolaCell, DummyPhoneNumberId.DavolaiPhone, DummyPhoneNumberId.ObamaCell };

            MergingConversationManager conversationManagerMerged = CreateMergingConversationManager(dummyContactIds, DummyPhoneNumberIds, null);

            Assert.AreEqual(2, conversationManagerMerged.ConversationCount);
        }

        [TestMethod()]
        public void MultipleUnknownContactTestTest()
        {
            DummyContactId[] dummyContactIds = { };
            DummyPhoneNumberId[] DummyPhoneNumberIds = { DummyPhoneNumberId.UnknownEagle, DummyPhoneNumberId.UnknownGrahamBell };

            MergingConversationManager conversationManagerMerged = CreateMergingConversationManager(dummyContactIds, DummyPhoneNumberIds, null);

            Assert.AreEqual(2, conversationManagerMerged.ConversationCount);
        }
    }
}

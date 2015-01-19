using System.Collections.Generic;
using AllYourTextsLib;
using AllYourTextsLib.Conversation;
using AllYourTextsLib.Framework;
using AllYourTextsUi;
using AllYourTextsUi.Framework;
using DummyData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AllYourTextsTest.Mock_Objects;

namespace AllYourTextsTest
{

    [TestClass()]
    public class ConversationWindowTest
    {

        private IConversationManager GetConversationManager(IDisplayOptions diplayOptions)
        {
            IConversationManager conversationManager;

            DummyContactId[] contactIds = { DummyContactId.Davola, DummyContactId.Obama, DummyContactId.NeverTexter, DummyContactId.ReliableLarry };
            DummyPhoneNumberId[] messageSetIds = {DummyPhoneNumberId.DavolaCell,
                                                 DummyPhoneNumberId.DavolaiPhone,
                                                 DummyPhoneNumberId.ObamaCell,
                                                 DummyPhoneNumberId.ReliableLarryOffice,
                                                 DummyPhoneNumberId.UnknownLawnmower,
                                                 DummyPhoneNumberId.UnknownEagle};
            List<Contact> contacts = DummyConversationDataGenerator.GetContacts(contactIds);
            List<ChatRoomInformation> chatInfoItems = new List<ChatRoomInformation>();
            List<TextMessage> messages = DummyConversationDataGenerator.GetMessages(messageSetIds);
            List<MessageAttachment> attachments = new List<MessageAttachment>();

            conversationManager = new ConversationManager(contacts, messages, chatInfoItems, attachments, null);
            if (diplayOptions.MergeContacts)
            {
                conversationManager = new MergingConversationManager(conversationManager, null);
            }

            return conversationManager;
        }

        private ConversationWindowModel GetPopulatedConversationWindowModel()
        {
            IDisplayOptions displayOptions = new MockDisplayOptions();
            IPhoneSelectOptions phoneSelectOptions = new MockPhoneSelectOptions();
            ConversationWindowModel model = new ConversationWindowModel(displayOptions, phoneSelectOptions);
            IConversationManager conversationManager = GetConversationManager(displayOptions);
            model.ConversationManager = conversationManager;

            return model;
        }

        private ConversationWindowModel GetEmptyPopulatedConversationWindowModel()
        {
            IDisplayOptions displayOptions = new MockDisplayOptions();
            IPhoneSelectOptions phoneSelectOptions = new MockPhoneSelectOptions();
            ConversationWindowModel model = new ConversationWindowModel(displayOptions, phoneSelectOptions);
            model.ConversationManager = DummyConversationDataGenerator.GetConversationManagerEmpty();

            return model;
        }

        private void VerifyDefaultConversationModelSettings(IConversationWindowModel model)
        {
            Assert.AreEqual(MockDisplayOptions.HideEmptyConversationsDefault, model.DisplayOptions.HideEmptyConversations);
            Assert.AreEqual(MockDisplayOptions.MergeContactsDefault, model.DisplayOptions.MergeContacts);
            Assert.AreEqual(MockDisplayOptions.TimeDisplayFormatDefault, model.DisplayOptions.TimeDisplayFormat);
        }

        [TestMethod()]
        public void EmptyConversationWindowModelTest()
        {
            ConversationWindowModel model = new ConversationWindowModel(new MockDisplayOptions(), new MockPhoneSelectOptions());

            VerifyDefaultConversationModelSettings(model);
            TextConversationStatisticsTest.VerifyStatisticsAllZero(model.ConversationStatistics);
        }

        private int CountListItems(IEnumerable<IConversationListItem> listItems)
        {
            return new List<IConversationListItem>(listItems).Count;
        }

        private void VerifyConversationListSortedAlphabetically(IEnumerable<IConversationListItem> conversationList)
        {
            string lastContactName = "";
            string lastNumber = "";
            bool firstElement = true;

            foreach (IConversationListItem listItem in conversationList)
            {
                IConversation conversation = listItem.Conversation;

                if (conversation.AssociatedContacts.Count != 1)
                {
                    continue;
                }

                string currentContactName = conversation.AssociatedContacts[0].DisplayName;
                string currentNumber = conversation.AssociatedContacts[0].PhoneNumbers[0].Number;

                if (!firstElement)
                {
                    firstElement = false;

                    Assert.IsTrue(string.Compare(lastContactName, currentContactName) > 0);
                    Assert.IsTrue(string.Compare(lastNumber, currentNumber) > 0);
                }

                lastContactName = currentContactName;
                lastNumber = currentNumber;
            }
        }

        [TestMethod()]
        public void EmptyPopulatedConversationWindowModelTest()
        {
            ConversationWindowModel model = GetEmptyPopulatedConversationWindowModel();

            Assert.AreEqual(0, CountListItems(model.ConversationListItems));
            Assert.IsNull(model.SelectedConversation);
            TextConversationStatisticsTest.VerifyStatisticsAllZero(model.ConversationStatistics);
        }

        [TestMethod()]
        public void PopulatedConversationWindowModelTest()
        {
            ConversationWindowModel model = GetPopulatedConversationWindowModel();
            VerifyDefaultConversationModelSettings(model);

            List<IConversationListItem> conversationListItems = new List<IConversationListItem>(model.ConversationListItems);
            VerifyConversationListSortedAlphabetically(conversationListItems);

            IConversation conversation = conversationListItems[2].Conversation;
            IConversationStatistics expectedStatistics = ConversationStatisticsGenerator.CalculateStatistics(conversation);
            model.SelectedConversation = conversation;
            Assert.AreEqual(expectedStatistics, model.ConversationStatistics);
        }
    }
}

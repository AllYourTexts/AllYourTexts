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
    public class MainWindowModelBaseTest
    {

        internal virtual IConversationManager CreateConversationManager()
        {
            DummyContactId[] dummyContactIds = { DummyContactId.Davola, DummyContactId.NeverTexter, DummyContactId.Obama, DummyContactId.ReliableLarry };
            DummyPhoneNumberId[] dummyMessageSetIds = { DummyPhoneNumberId.DavolaCell, DummyPhoneNumberId.DavolaiPhone, DummyPhoneNumberId.NeverTexterCell, DummyPhoneNumberId.NeverTexterHome,
                                                         DummyPhoneNumberId.ObamaCell, DummyPhoneNumberId.ReliableLarryOffice };

            return DummyConversationDataGenerator.GetConversationManager(dummyContactIds, dummyMessageSetIds, null);
        }

        internal virtual MainWindowModelBase_Accessor CreateConversationWindowModel()
        {
            return new ConversationWindowModel_Accessor(new MockDisplayOptions(), new MockPhoneSelectOptions());
        }

        internal virtual MainWindowModelBase_Accessor CreateGraphWindowModel()
        {
            return new GraphWindowModel_Accessor(new MockDisplayOptions(), new MockPhoneSelectOptions());
        }

        internal virtual IEnumerable<IMainWindowModel_Accessor> GetImplementations()
        {
            MainWindowModelBase_Accessor[] implementations = { CreateConversationWindowModel(), CreateGraphWindowModel() };

            return implementations;
        }

        [TestMethod()]
        public void EmptyTest()
        {
            foreach (MainWindowModelBase_Accessor model in GetImplementations())
            {
                List<IConversationListItem> conversationListItems = new List<IConversationListItem>(model.ConversationListItems);
                Assert.AreEqual(0, conversationListItems.Count);

                Assert.IsNull(model.SelectedConversation);
                Assert.IsNull(model.ConversationManager);

                TextConversationStatisticsTest.VerifyStatisticsAllZero(model.ConversationStatistics);
            }
        }

        [TestMethod()]
        public void SwitchConversationTest()
        {
            foreach (MainWindowModelBase_Accessor model in GetImplementations())
            {
                model.ConversationManager = CreateConversationManager();
                List<IConversationListItem> conversationListItems = new List<IConversationListItem>(model.ConversationListItems);

                Assert.AreEqual(model.DefaultConversation, model.SelectedConversation);

                IConversation conversation2 = conversationListItems[2].Conversation;
                IConversationStatistics stats2 = ConversationStatisticsGenerator.CalculateStatistics(conversation2);
                IConversation conversation4 = conversationListItems[3].Conversation;
                IConversationStatistics stats3 = ConversationStatisticsGenerator.CalculateStatistics(conversation4);

                model.SelectedConversation = conversation2;
                Assert.AreEqual(conversation2, model.SelectedConversation);
                Assert.AreEqual(stats2, model.ConversationStatistics);

                model.SelectedConversation = conversation4;
                Assert.AreEqual(conversation4, model.SelectedConversation);
                Assert.AreEqual(stats3, model.ConversationStatistics);

                model.SelectedConversation = conversation2;
                Assert.AreEqual(conversation2, model.SelectedConversation);

                model.SelectedConversation = null;
                Assert.AreEqual(model.DefaultConversation, model.SelectedConversation);

                model.SelectedConversation = conversation2;
                Assert.AreEqual(conversation2, model.SelectedConversation);

                IConversation missingConversation = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.TonyWolfCell);
                model.SelectedConversation = missingConversation;
                Assert.AreEqual(model.DefaultConversation, model.SelectedConversation);
            }
        }

        [TestMethod()]
        public void ConversationOrderTest()
        {
            foreach (MainWindowModelBase_Accessor model in GetImplementations())
            {
                model.ConversationManager = CreateConversationManager();

                IConversationListItem listItemPrevious = null;

                model.DisplayOptions.ConversationSorting = ConversationSorting.AlphabeticalByContact;
                foreach (IConversationListItem listItem in model.ConversationListItems)
                {
                    if (listItem.Conversation.GetType() == typeof(AggregateConversation))
                    {
                        Assert.IsNull(listItemPrevious);
                        continue;
                    }
                    if (listItemPrevious != null)
                    {
                        Assert.IsTrue(ConversationComparer.AlphabeticalByContact(listItemPrevious.Conversation, listItem.Conversation) <= 0);
                        Assert.AreEqual(listItemPrevious.Conversation, model.PreviousConversation(listItem.Conversation));
                        Assert.AreEqual(listItem.Conversation, model.NextConversation(listItemPrevious.Conversation));
                    }

                    listItemPrevious = listItem;
                }

                listItemPrevious = null;
                model.DisplayOptions.ConversationSorting = ConversationSorting.DescendingByTotalMessages;
                foreach (IConversationListItem listItem in model.ConversationListItems)
                {
                    if (listItem.Conversation.GetType() == typeof(AggregateConversation))
                    {
                        Assert.IsNull(listItemPrevious);
                        continue;
                    }
                    if (listItemPrevious != null)
                    {
                        Assert.IsTrue(ConversationComparer.DescendingByTotalMessages(listItemPrevious.Conversation, listItem.Conversation) <= 0);
                        Assert.AreEqual(listItemPrevious.Conversation, model.PreviousConversation(listItem.Conversation));
                        Assert.AreEqual(listItem.Conversation, model.NextConversation(listItemPrevious.Conversation));
                    }

                    listItemPrevious = listItem;
                }
            }
        }

        [TestMethod()]
        public void PreviousNextConversationTest()
        {
            foreach (MainWindowModelBase_Accessor model in GetImplementations())
            {
                model.ConversationManager =  CreateConversationManager();

                Assert.IsNotNull(model.NextConversation(null));
                Assert.IsNotNull(model.PreviousConversation(null));
            }

        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DummyData;
using System.Collections.Generic;
using AllYourTextsLib.Framework;
using AllYourTextsLib;
using AllYourTextsLib.Conversation;
using AllYourTextsUi.Framework;
using AllYourTextsUi;
using AllYourTextsTest.Mock_Objects;

namespace AllYourTextsTest
{
    
    /// <summary>
    ///This is a test class for GraphWindowModelTest and is intended
    ///to contain all GraphWindowModelTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GraphWindowModelTest
    {

        private IConversationManager GetConversationManager()
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

            return conversationManager;
        }

        private GraphWindowModel_Accessor GetPopulatedGraphWindowModel()
        {
            GraphWindowModel_Accessor model = new GraphWindowModel_Accessor(new MockDisplayOptions(), new MockPhoneSelectOptions());
            model.ConversationManager = GetConversationManager();

            return model;
        }

        private GraphWindowModel_Accessor GetEmptyPopulatedGraphWindowModel()
        {
            GraphWindowModel_Accessor model = new GraphWindowModel_Accessor(new MockDisplayOptions(), new MockPhoneSelectOptions());
            model.ConversationManager = DummyConversationDataGenerator.GetConversationManagerEmpty();

            return model;
        }

        [TestMethod()]
        public void EmptyTest()
        {
            GraphWindowModel_Accessor model = new GraphWindowModel_Accessor(new MockDisplayOptions(), new MockPhoneSelectOptions());

            Assert.AreEqual(GraphWindowModel_Accessor.DefaultGraphType, model._selectedGraphType);
            Assert.AreEqual(MainWindowModelBase_Accessor.NoContactSelectedIndex, model.SelectedConversationIndex);
            Assert.IsNull(model.SelectedConversation);
            Assert.IsNull(model.DefaultConversation);
            Assert.IsNull(model._cachedCurrentGraphData);
            Assert.IsNull(model.CurrentGraphDataCollection);
            Assert.IsNotNull(model.DisplayOptions);
            Assert.IsNotNull(model.PhoneSelectOptions);
        }

        [TestMethod()]
        public void PopulatedEmptyModelTest()
        {
            GraphWindowModel_Accessor model = GetEmptyPopulatedGraphWindowModel();

            Assert.AreEqual(GraphWindowModel_Accessor.DefaultGraphType, model._selectedGraphType);
            Assert.AreEqual(GraphWindowModel_Accessor.DefaultConversationIndex, model.SelectedConversationIndex);

            int conversationListItemCountActual = new List<IConversationListItem>(model.ConversationListItems).Count;
            Assert.AreEqual(1, conversationListItemCountActual);

            Assert.IsNotNull(model.SelectedConversation);
            TextConversationStatisticsTest.VerifyStatisticsAllZero(model.ConversationStatistics);
            foreach (ITextGraphData graphData in model.CurrentGraphDataCollection)
            {
                Assert.AreEqual(0, graphData.MessagesTotal);
            }
            Assert.IsNotNull(model.DisplayOptions);
            Assert.IsNotNull(model.PhoneSelectOptions);
        }

        [TestMethod()]
        public void PopulatedModelTest()
        {
            GraphWindowModel_Accessor model = GetPopulatedGraphWindowModel();

            Assert.AreEqual(GraphWindowModel_Accessor.DefaultGraphType, model._selectedGraphType);
            Assert.AreEqual(GraphWindowModel_Accessor.DefaultConversationIndex, model.SelectedConversationIndex);
            
            IConversation conversation = model.SelectedConversation;
            Assert.IsInstanceOfType(conversation, typeof(AggregateConversation));
            IConversationStatistics statsExpected = ConversationStatisticsGenerator.CalculateStatistics(conversation);
            Assert.AreEqual(statsExpected, model.ConversationStatistics);
            Assert.AreEqual(conversation, model.DefaultConversation);
            Assert.IsNotNull(model.DisplayOptions);
            Assert.IsNotNull(model.PhoneSelectOptions);

            //
            // Nothing should be calculated until the graph data is demanded.
            //

            Assert.IsNull(model._cachedCurrentGraphData);
            Assert.IsNotNull(model.CurrentGraphDataCollection);
            Assert.IsNotNull(model._cachedCurrentGraphData);
        }

        [TestMethod()]
        public void SwitchGraphTypeTest()
        {
            GraphWindowModel_Accessor model = GetPopulatedGraphWindowModel();

            //
            // Populate the cache.
            //

            ITextGraphDataCollection originalGraphData = model.CurrentGraphDataCollection;
            Assert.IsNotNull(originalGraphData);
            Assert.AreEqual(originalGraphData, model._cachedCurrentGraphData);

            GraphType newGraphType = GraphType.PerMonth;
            Assert.AreNotEqual(newGraphType, model.SelectedGraphType);
            model.SelectedGraphType = newGraphType;

            Assert.IsNull(model._cachedCurrentGraphData);
            ITextGraphDataCollection newGraphData = model.CurrentGraphDataCollection;
            Assert.IsNotNull(model._cachedCurrentGraphData);
            Assert.AreNotEqual(newGraphData, originalGraphData);
        }

        [TestMethod()]
        public void SwitchConversationTest()
        {
            GraphWindowModel_Accessor model = GetPopulatedGraphWindowModel();

            //
            // Populate the cache.
            //

            ITextGraphDataCollection originalGraphData = model.CurrentGraphDataCollection;
            Assert.IsNotNull(originalGraphData);
            Assert.AreEqual(originalGraphData, model._cachedCurrentGraphData);

            IConversation newConversation = new List<IConversationListItem>(model.ConversationListItems)[2].Conversation;
            Assert.AreNotEqual(newConversation, model.SelectedGraphType);
            model.SelectedConversation = newConversation;

            Assert.IsNull(model._cachedCurrentGraphData);
            ITextGraphDataCollection newGraphData = model.CurrentGraphDataCollection;
            Assert.IsNotNull(model._cachedCurrentGraphData);
            Assert.AreNotEqual(newGraphData, originalGraphData);
        }
    }
}

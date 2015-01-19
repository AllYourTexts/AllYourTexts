using AllYourTextsLib;
using AllYourTextsLib.Conversation;
using AllYourTextsLib.Framework;
using AllYourTextsUi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace AllYourTextsTest
{
    [TestClass()]
    public class ConversationDescriptionHelperTest
    {
        private IContact _contactGeorgeJetson;
        private IContact _contactJerrySeinfeld;
        private IContact _contactBartSimpson;
        private IContact _contactChandlerBing;
        private IContact _contactShortName;
        private IContact _contactLongName;

        [TestInitialize()]
        public void TestInitialize()
        {
            _contactGeorgeJetson = new MockContact("George", "Jetson", new PhoneNumber("212-555-1234"));
            _contactJerrySeinfeld = new MockContact("Jerry", "Seinfeld", new PhoneNumber("646-555-0987"));
            _contactJerrySeinfeld.PhoneNumbers.Add(new PhoneNumber("646-555-9876"));
            _contactBartSimpson = new MockContact("Bart", "Simpson");
            _contactChandlerBing = new MockContact("Chandler", "Bing");
            _contactShortName = new MockContact("X", null);
            _contactLongName = new MockContact(new String('a', 500), "Smith", new PhoneNumber("202-555-8392"));
        }

        private void VerifyDescriptionMatchesExpected(IConversation conversation, string descriptionExpected)
        {
            ConversationDescriptionHelper descriptionHelper = new ConversationDescriptionHelper();
            string descriptionActual = descriptionHelper.GetDescription(conversation);
            Assert.AreEqual(descriptionExpected, descriptionActual);
        }

        private void VerifyDescriptionMatchesExpected(IEnumerable<IContact> associatedContacts, string descriptionExpected)
        {
            var conversation = new Mock<IConversation>();
            conversation.Setup(conv => conv.AssociatedContacts)
                .Returns(new ContactList(associatedContacts));
            VerifyDescriptionMatchesExpected(conversation.Object, descriptionExpected);
        }

        [TestMethod()]
        public void GetSingleNumberConversationDescriptionTest()
        {
            var contacts = new IContact[] { _contactGeorgeJetson };
            VerifyDescriptionMatchesExpected(contacts, "George Jetson (\u200E212-555-1234\u202C)"); // includes BOM markers
        }

        [TestMethod()]
        public void GetSingleNumberConversationDescriptionLongNameTest()
        {
            var contacts = new IContact[] { _contactLongName };
            VerifyDescriptionMatchesExpected(contacts, "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa... (\u200E202-555-8392\u202C)"); // includes BOM markers
        }

        [TestMethod()]
        public void GetSingleNumberConversationDescriptionLongNameMutliNumberTest()
        {
            _contactLongName.PhoneNumbers.Add(new PhoneNumber("202-555-8393"));
            var contacts = new IContact[] { _contactLongName };
            VerifyDescriptionMatchesExpected(contacts, "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa... (2 numbers)");
        }

        [TestMethod()]
        public void GetMultiNumberConversationDescriptionTest()
        {
            var contacts = new IContact[] { _contactJerrySeinfeld };
            VerifyDescriptionMatchesExpected(contacts, "Jerry Seinfeld (2 numbers)");
        }

        [TestMethod()]
        public void GetGroupChatDescriptionTest()
        {
            var contacts = new IContact[] { 
                _contactGeorgeJetson,
                _contactJerrySeinfeld,
                _contactBartSimpson,
                _contactChandlerBing
            };
            VerifyDescriptionMatchesExpected(contacts, "Group Chat (George J., Jerry S., Bart S., ...)");
        }

        [TestMethod()]
        public void GetGroupChatDescriptionShortNameTest()
        {
            var contacts = new IContact[] { _contactShortName, _contactShortName };
            VerifyDescriptionMatchesExpected(contacts, "Group Chat (X, X)");
        }

        [TestMethod()]
        public void GetGroupChatDescriptionLongNameShortNameTest()
        {
            var contacts = new IContact[] { _contactLongName, _contactShortName };
            VerifyDescriptionMatchesExpected(contacts, "Group Chat (aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa..., ...)");
        }

        [TestMethod()]
        public void GetGroupChatDescriptionShortNameLongNameTest()
        {
            var contacts = new IContact[] { _contactShortName, _contactLongName };
            VerifyDescriptionMatchesExpected(contacts, "Group Chat (X, ...)");
        }
    }
}

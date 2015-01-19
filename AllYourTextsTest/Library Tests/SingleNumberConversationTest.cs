using System;
using AllYourTextsLib;
using AllYourTextsLib.Conversation;
using AllYourTextsLib.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AllYourTextsTest
{
    
    [TestClass()]
    public class SingleNumberConversationTest
    {
        const string NumberValue1Formatted = "(832) 938-0098";
        const string NumberValue1Stripped = "8329380098";
        const string UsCountryValue = "us";

        private IPhoneNumber GetDummyPhoneNumber()
        {
            return new PhoneNumber(NumberValue1Formatted);
        }

        private IContact GetDummyContact()
        {
            Contact contact = new Contact(MockContact.MockContactId, "George", null, "Cauldron", GetDummyPhoneNumber());

            return contact;
        }

        private TextMessage GetMessage1()
        {
            return new TextMessage(15, false, new DateTime(2010, 11, 16, 18, 34, 45), "Hey, it's George!", NumberValue1Stripped, UsCountryValue);
        }

        private TextMessage GetMessage2()
        {
            return new TextMessage(23, true, new DateTime(2010, 11, 16, 18, 35, 23), "Hey George! How are you?", NumberValue1Formatted, UsCountryValue);
        }

        private TextMessage GetMessage3()
        {
            return new TextMessage(28, false, new DateTime(2010, 11, 16, 18, 39, 1), "I'm fantastic. And yourself?", NumberValue1Stripped, UsCountryValue);
        }

        private TextMessage GetMessage4()
        {
            return new TextMessage(15, true, new DateTime(2010, 11, 16, 19, 5, 34), "Dandy, pandy!", NumberValue1Formatted, UsCountryValue);
        }

        private TextMessage GetNullAddressMessage()
        {
            return new TextMessage(67, true, new DateTime(2010, 11, 16, 12, 15, 31), "Dandy, pandy!", null, UsCountryValue);
        }

        private TextMessage GetNullContentMessage()
        {
            return new TextMessage(89, true, new DateTime(2010, 11, 16, 17, 3, 46), null, NumberValue1Formatted, UsCountryValue);
        }

        private SingleNumberConversation GetDummyConversation()
        {
            SingleNumberConversation conversation = new SingleNumberConversation(GetDummyContact());

            conversation.AddMessage(GetMessage1());
            conversation.AddMessage(GetMessage2());
            conversation.AddMessage(GetMessage3());

            return conversation;
        }

        private void VerifyTextMessageMatchesConversationMessage(TextMessage textMessage, IConversationMessage conversationMessage)
        {
            Assert.AreEqual(textMessage.Timestamp, conversationMessage.Timestamp);
            Assert.AreEqual(textMessage.MessageContents, conversationMessage.MessageContents);
            Assert.AreEqual(textMessage.IsOutgoing, conversationMessage.IsOutgoing);
        }

        private void VerifyMessagesMatch(IConversation conversation, TextMessage[] messagesExpected)
        {
            Assert.AreEqual(messagesExpected.Length, conversation.MessageCount);

            for (int messageIndex = 0; messageIndex < conversation.MessageCount; messageIndex++)
            {
                IConversationMessage conversationMessageCurrent = conversation.GetMessage(messageIndex);
                TextMessage textMessageExpected = messagesExpected[messageIndex];
                VerifyTextMessageMatchesConversationMessage(textMessageExpected, conversationMessageCurrent);
            }
        }

        [TestMethod()]
        public void EmptyTest()
        {
            SingleNumberConversation conversation = new SingleNumberConversation(GetDummyContact());
            Assert.AreEqual(0, conversation.MessageCount);

            int messagesSeen = 0;
            foreach (IConversationMessage message in conversation)
            {
                messagesSeen++;
            }
            Assert.AreEqual(0, messagesSeen);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void EmptyMessageAccessTest()
        {
            SingleNumberConversation conversation = new SingleNumberConversation(GetDummyContact());

            conversation.GetMessage(0);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void OutOfRangeMessageAccessTest()
        {
            SingleNumberConversation conversation = GetDummyConversation();
            conversation.GetMessage(conversation.MessageCount);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NegativeMessageIndexTest()
        {
            SingleNumberConversation conversation = GetDummyConversation();
            conversation.GetMessage(-1);
        }

        [TestMethod()]
        public void StandardConversationTest()
        {
            SingleNumberConversation conversation = new SingleNumberConversation(GetDummyContact());

            conversation.AddMessage(GetMessage1());
            conversation.AddMessage(GetMessage3());
            conversation.AddMessage(GetMessage2());
            conversation.AddMessage(GetMessage4());

            TextMessage[] textMessagesExpected = { GetMessage1(), GetMessage2(), GetMessage3(), GetMessage4() };
            VerifyMessagesMatch(conversation, textMessagesExpected);
        }

        [TestMethod()]
        public void NullMessageContentsTest()
        {
            SingleNumberConversation conversation = new SingleNumberConversation(GetDummyContact());

            conversation.AddMessage(GetMessage1());
            conversation.AddMessage(GetMessage3());
            conversation.AddMessage(GetNullContentMessage());
            conversation.AddMessage(GetMessage4());

            TextMessage[] textMessagesExpected = { GetMessage1(), GetMessage3(), GetMessage4() };
            VerifyMessagesMatch(conversation, textMessagesExpected);
        }
    }
}

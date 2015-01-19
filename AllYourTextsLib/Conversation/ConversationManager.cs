using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib.Conversation
{
    public class ConversationManager : ConversationManagerBase
    {
        private Dictionary<string, IContact> _contactLookupTable;
        private Dictionary<long, TextMessage> _messageLookupTable;

        public ConversationManager(IEnumerable<IContact> contacts, IEnumerable<TextMessage> messages, IEnumerable<ChatRoomInformation> chatInfoItems, IEnumerable<MessageAttachment> attachments, ILoadingProgressCallback progressCallback)
        {
            _contactLookupTable = new Dictionary<string, IContact>();
            _messageLookupTable = new Dictionary<long, TextMessage>();
            List<IConversation> conversationList = LoadConversations(contacts, messages, chatInfoItems, attachments, progressCallback);
            SetConversationList(conversationList);
        }

        private List<IConversation> LoadConversations(IEnumerable<IContact> contacts, IEnumerable<TextMessage> messages, IEnumerable<ChatRoomInformation> chatInfoItems, IEnumerable<MessageAttachment> attachments, ILoadingProgressCallback progressCallback)
        {
            Dictionary<string, IConversation> conversationHashTable = new Dictionary<string, IConversation>();

            CheckForCancel(progressCallback);

            LoadContacts(conversationHashTable, contacts, progressCallback);

            LoadChatRoomInformation(conversationHashTable, chatInfoItems, progressCallback);

            LoadMessages(conversationHashTable, messages, progressCallback);

            LoadAttachments(attachments, progressCallback);

            CheckForCancel(progressCallback);

            List<IConversation> finalConversations = new List<IConversation>(conversationHashTable.Values);

            return finalConversations;
        }

        private void LoadContacts(Dictionary<string, IConversation> conversationHashTable, IEnumerable<IContact> contacts, ILoadingProgressCallback progressCallback)
        {
            SetProgressPhase(progressCallback, LoadingPhase.ReadingContacts);

            foreach (IContact contact in contacts)
            {
                LoadContact(conversationHashTable, contact, progressCallback);
            }
        }

        private void LoadContact(Dictionary<string, IConversation> conversationHashTable, IContact contact, ILoadingProgressCallback progressCallback)
        {
            foreach (PhoneNumber contactPhoneNumber in contact.PhoneNumbers)
            {
                CheckForCancel(progressCallback);

                string strippedNumber = PhoneNumber.Strip(contactPhoneNumber);

                //
                // If multiple contacts share a single phone number, the duplicate phone numbers are ignored and
                // only the first contact seen is associated with the number.
                //

                if (!conversationHashTable.ContainsKey(strippedNumber))
                {
                    SingleNumberConversation conversation = new SingleNumberConversation(contact);
                    conversationHashTable.Add(strippedNumber, conversation);
                }

                if (!_contactLookupTable.ContainsKey(strippedNumber))
                {
                    _contactLookupTable[strippedNumber] = contact;
                }
            }

            IncrementWorkProgress(progressCallback);
        }

        private void LoadChatRoomInformation(Dictionary<string, IConversation> conversationHashTable, IEnumerable<ChatRoomInformation> chatRoomInfoItems, ILoadingProgressCallback progressCallback)
        {
            SetProgressPhase(progressCallback, LoadingPhase.ReadingChatInformation);

            foreach (ChatRoomInformation chatRoomInfo in chatRoomInfoItems)
            {
                CheckForCancel(progressCallback);
                
                List<IContact> chatContacts = LoadChatRoomContacts(chatRoomInfo);
                conversationHashTable.Add(chatRoomInfo.ChatId, new ChatConversation(chatContacts));

                IncrementWorkProgress(progressCallback);
            }
        }

        private List<IContact> LoadChatRoomContacts(ChatRoomInformation chatRoomInfo)
        {
            List<IContact> contacts = new List<IContact>();
            foreach (string phoneNumberValue in chatRoomInfo.Participants)
            {
                IContact contact = GetContactByPhoneNumber(new PhoneNumber(phoneNumberValue));
                if (contact == null)
                {
                    contact = new Contact(Contact.UnknownContactId, null, null, null, new PhoneNumber(phoneNumberValue));
                }

                contacts.Add(contact);
            }

            return contacts;
        }

        private IContact GetContactByPhoneNumber(PhoneNumber phoneNumber)
        {
            string phoneNumberStripped = PhoneNumber.Strip(phoneNumber);

            if (string.IsNullOrEmpty(phoneNumberStripped))
            {
                return null;
            }
            
            if (_contactLookupTable.ContainsKey(phoneNumberStripped))
            {
                return _contactLookupTable[phoneNumberStripped];
            }
            else
            {
                return null;
            }
        }

        private void LoadMessages(Dictionary<string, IConversation> conversationHashTable, IEnumerable<TextMessage> messages, ILoadingProgressCallback progressCallback)
        {
            SetProgressPhase(progressCallback, LoadingPhase.ReadingMessages);
            foreach (TextMessage message in messages)
            {
                LoadMessage(conversationHashTable, message, progressCallback);
            }
        }

        private void LoadMessage(Dictionary<string, IConversation> conversationHashTable, TextMessage message, ILoadingProgressCallback progressCallback)
        {
            CheckForCancel(progressCallback);

            string conversationKey;
            PhoneNumber messagePhoneNumber = new PhoneNumber(message.Address, message.Country);
            string phoneNumberStripped = PhoneNumber.Strip(messagePhoneNumber);

            if (message.ChatId != null)
            {
                conversationKey = message.ChatId;
            }
            else
            {
                conversationKey = phoneNumberStripped;
            }

            if (string.IsNullOrEmpty(conversationKey))
            {
                return;
            }

            Contact unknownContact = new Contact(Contact.UnknownContactId, null, null, null, messagePhoneNumber);
            message.Contact = GetContactByPhoneNumber(messagePhoneNumber);
            if (message.Contact == null)
            {
                message.Contact = unknownContact;
            }
            else
            {
                //
                // Update the contact's phone number to include country information.
                //

                foreach (PhoneNumber contactPhoneNumber in message.Contact.PhoneNumbers)
                {
                    if (PhoneNumber.NumbersAreEquivalent(contactPhoneNumber, messagePhoneNumber))
                    {
                        contactPhoneNumber.Country = message.Country;
                        break;
                    }
                }
            }

            IConversation conversation;

            if (conversationHashTable.ContainsKey(conversationKey))
            {
                conversation = conversationHashTable[conversationKey];
            }
            else
            {
                conversation = new SingleNumberConversation(unknownContact);
                conversationHashTable.Add(conversationKey, conversation);
            }

            _messageLookupTable.Add(message.MessageId, message);

            conversation.AddMessage(message);

            IncrementWorkProgress(progressCallback);
        }

        private void LoadAttachments(IEnumerable<MessageAttachment> attachments, ILoadingProgressCallback progressCallback)
        {
            foreach (MessageAttachment attachment in attachments)
            {
                LoadAttachment(attachment, progressCallback);
            }
        }

        private void LoadAttachment(MessageAttachment attachment, ILoadingProgressCallback progressCallback)
        {
            CheckForCancel(progressCallback);

            try
            {
                TextMessage matchingMessage = _messageLookupTable[attachment.MessageId];
                matchingMessage.AddAttachment(attachment);
            }
            catch (KeyNotFoundException)
            {
                ;
            }

            IncrementWorkProgress(progressCallback);
        }


        public static int GetWorkEstimate(int contactCountEstimate, int messageCountEstimate, int chatInfoCountEstimate, int attachmentCountEstimate)
        {
            int workAmount = 0;

            workAmount += contactCountEstimate;

            workAmount += messageCountEstimate;

            workAmount += chatInfoCountEstimate;

            workAmount += attachmentCountEstimate;
            
            return workAmount;
        }
    }
}

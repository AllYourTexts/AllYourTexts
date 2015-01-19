using AllYourTextsLib.Framework;
using AllYourTextsUi.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsUi
{
    public class ConversationDescriptionHelper : IConversationDescriptionHelper
    {
        public string GetDescription(IConversation conversation)
        {
            if (conversation.AssociatedContacts == null)
            {
                return "All Conversations";
            }
            else if (IsSinglePartnerConversation(conversation))
            {
                return GetDescriptionSinglePartner(conversation);
            }
            else
            {
                return GetDescriptionGroupChat(conversation);
            }
        }

        private string GetDescriptionSinglePartner(IConversation conversation)
        {
            string displayName = conversation.AssociatedContacts[0].DisplayName;
            string displayNameTruncated = TruncateNameIfTooLong(displayName);
            string associatedNumberDescription = GetAssociatedNumberDescription(conversation);

            return string.Format("{0} ({1})", displayNameTruncated, associatedNumberDescription);
        }

        private string GetDescriptionGroupChat(IConversation conversation)
        {
            string chatParticipantsList = GetChatParticipantList(conversation);

            return string.Format("Group Chat ({0})", chatParticipantsList);
        }

        private bool IsGroupChatConversation(IConversation conversation)
        {
            if (conversation.AssociatedContacts.Count < 1)
            {
                throw new ArgumentException("Invalid conversation. Contains no associated contacts.");
            }
            else if (conversation.AssociatedContacts.Count == 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool IsSinglePartnerConversation(IConversation conversation)
        {
            return !IsGroupChatConversation(conversation);
        }

        private string GetAssociatedNumberDescription(IConversation conversation)
        {
            if (!IsSinglePartnerConversation(conversation))
            {
                throw new ArgumentException("Single partner conversation expected.");
            }

            IContact associatedContact = conversation.AssociatedContacts[0];
            string phoneNumberDescription;
            if (associatedContact.PhoneNumbers.Count == 1)
            {
                //
                // Add LTR markers because phone numbers should never be displayed RTL.
                //

                phoneNumberDescription = string.Format("{0}{1}{2}",
                                                       '\u200E',
                                                       PhoneNumberFormatter.FormatNumberWithDashes(associatedContact.PhoneNumbers[0]),
                                                       '\u202C');
            }
            else
            {
                phoneNumberDescription = string.Format("{0} numbers", associatedContact.PhoneNumbers.Count);
            }

            return phoneNumberDescription;
        }

        private string GetChatParticipantList(IConversation conversation)
        {
            return GetChatParticipantListFromContacts(conversation.AssociatedContacts);
        }

        private string GetChatParticipantListFromContacts(IEnumerable<IContact> contacts)
        {
            const string separator = ", ";

            StringBuilder chatParticipantsList = new StringBuilder();
            bool truncated = false;

            foreach (IContact contact in contacts)
            {
                string contactNameFormatted = CreateShortenedContactName(contact);
                int truncatedLength = chatParticipantsList.Length +
                                        contactNameFormatted.Length +
                                        separator.Length +
                                        Truncator.Length;
                if (truncatedLength > MaxChatParticipantsChars)
                {
                    chatParticipantsList.Append(Truncator);
                    truncated = true;
                    break;
                }
                chatParticipantsList.Append(contactNameFormatted);
                chatParticipantsList.Append(separator);
            }

            if (!truncated)
            {
                chatParticipantsList.Remove(chatParticipantsList.Length - 2, 2);
            }

            return chatParticipantsList.ToString();
        }

        private string CreateShortenedContactName(IContact contact)
        {
            List<string> names = new List<string>();
            foreach (string namePart in new string[] { contact.FirstName, contact.MiddleName, contact.LastName})
            {
                if (!string.IsNullOrEmpty(namePart))
                {
                    names.Add(namePart);
                }
            }
            if (names.Count == 0)
            {
                return contact.DisplayName;
            }

            string name;
            if (names.Count == 1)
            {
                name = names[0];
            }
            else // if (names.Count > 1)
            {
                char lastInitial = names[names.Count - 1][0];
                name = string.Format("{0} {1}.", names[0], lastInitial);
            }

            name = TruncateNameIfTooLong(name);
            return name;
        }

        private string TruncateNameIfTooLong(string displayName)
        {
            if (displayName.Length <= MaxFullNameChars)
            {
                return displayName;
            }

            string truncated = displayName.Substring(0, MaxFullNameChars - Truncator.Length);
            return truncated + Truncator;
        }

        private const int MaxChatParticipantsChars = 40;
        private const int MaxFullNameChars = 35;
        private const string Truncator = "...";
    }
}

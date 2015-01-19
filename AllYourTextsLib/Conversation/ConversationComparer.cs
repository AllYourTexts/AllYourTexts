using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib.Conversation
{
    public class ConversationComparer
    {
        public static int AlphabeticalByContact(IConversation conversationA, IConversation conversationB)
        {
            if ((conversationA == null) && (conversationB == null))
            {
                return 0;
            }
            else if (conversationA == null)
            {
                return -1;
            }
            else if (conversationB == null)
            {
                return 1;
            }

            int sizeCompare = CompareBasedOnContactListSize(conversationA.AssociatedContacts, conversationB.AssociatedContacts);
            if (sizeCompare != 0)
            {
                return sizeCompare;
            }

            int contactCompare = CompareContactListsAlphabetically(conversationA.AssociatedContacts, conversationB.AssociatedContacts);
            if (contactCompare != 0)
            {
                return contactCompare;
            }

            return 0;
        }

        private static int CompareContactListsAlphabetically(IContactList contactListA, IContactList contactListB)
        {
            int comparisons = Math.Min(contactListA.Count, contactListB.Count);

            for (int contactIndex = 0; contactIndex < comparisons; contactIndex++)
            {
                int contactCompare = ContactComparer.CompareAlphabetically(contactListA[contactIndex], contactListB[contactIndex]);
                if (contactCompare != 0)
                {
                    return contactCompare;
                }
            }

            return contactListA.Count - contactListB.Count;
        }

        private static int CompareBasedOnContactListSize(IContactList contactListA, IContactList contactListB)
        {
            int sizeCompare = CompareBasedOnContactListSizeImpl(contactListA, contactListB);
            if (sizeCompare != 0)
            {
                return sizeCompare;
            }
            sizeCompare = -1 * CompareBasedOnContactListSizeImpl(contactListB, contactListA);
            if (sizeCompare != 0)
            {
                return sizeCompare;
            }

            return 0;
        }

        private static int CompareBasedOnContactListSizeImpl(IContactList contactListA, IContactList contactListB)
        {
            int countA = contactListA.Count;
            int countB = contactListB.Count;

            if (countA == 0)
            {
                if (countB == 1)
                {
                    // A is unknown sender, B is single known partner

                    return 1;
                }
                else if (countB > 1)
                {
                    // A is unknown sender, B is group chat

                    return -1;
                }
            }
            else if ((countA > 1) && (countB <= 1))
            {
                // A is group chat, B is single (known/unkonwn) partner

                return 1;
            }

            return 0;
        }

        public static int DescendingByTotalMessages(IConversation conversationA, IConversation conversationB)
        {
            if ((conversationA == null) && (conversationB == null))
            {
                return 0;
            }
            else if (conversationA == null)
            {
                return 1;
            }
            else if (conversationB == null)
            {
                return -1;
            }

            return conversationB.MessageCount - conversationA.MessageCount;
        }
    }
}

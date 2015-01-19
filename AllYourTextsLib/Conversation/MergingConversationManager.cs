using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib.Conversation
{
    public class MergingConversationManager : ConversationManagerBase
    {
        public MergingConversationManager(IEnumerable<IConversation> conversationManager, ILoadingProgressCallback progressCallback)
        {
            List<IConversation> mergedConversations = CoalesceConversations(conversationManager, progressCallback);
            SetConversationList(mergedConversations);
        }

        public static int GetWorkEstimate(int conversations)
        {
            return conversations;
        }

        public static int GetWorkEstimateByContacts(int contactCountEstimate)
        {
            return contactCountEstimate;
        }

        private static List<IConversation> CoalesceConversations(IEnumerable<IConversation> unmergedConversations, ILoadingProgressCallback progressCallback)
        {
            Dictionary<long, IConversation> hashByContactId = new Dictionary<long, IConversation>();
            Dictionary<IContactList, IConversation> hashByContactList = new Dictionary<IContactList, IConversation>();
            List<IConversation> unknownContactConversations = new List<IConversation>();

            foreach (IConversation conversation in unmergedConversations)
            {
                CheckForCancel(progressCallback);

                if (conversation.AssociatedContacts.Count == 1)
                {
                    long contactId = conversation.AssociatedContacts[0].ContactId;
                    if (contactId == Contact.UnknownContactId)
                    {
                        unknownContactConversations.Add(conversation);
                    }
                    else if (hashByContactId.ContainsKey(contactId))
                    {
                        IConversation hashedConversation = hashByContactId[contactId];
                        hashByContactId.Remove(contactId);

                        IConversation mergedConversation = MergeConversations(hashedConversation, conversation);
                        hashByContactId.Add(contactId, mergedConversation);
                    }
                    else
                    {
                        hashByContactId.Add(contactId, conversation);
                    }
                }
                else if (hashByContactList.ContainsKey(conversation.AssociatedContacts))
                {
                    IConversation hashedConversation = hashByContactList[conversation.AssociatedContacts];
                    hashByContactList.Remove(conversation.AssociatedContacts);

                    IConversation mergedConversation = MergeConversations(hashedConversation, conversation);
                    hashByContactList.Add(mergedConversation.AssociatedContacts, mergedConversation);
                }
                else
                {
                    hashByContactList.Add(conversation.AssociatedContacts, conversation);
                }

                IncrementWorkProgress(progressCallback);
            }

            List<IConversation> mergedConversations = new List<IConversation>(hashByContactId.Values);
            mergedConversations.AddRange(hashByContactList.Values);
            mergedConversations.AddRange(unknownContactConversations);

            return mergedConversations;
        }

        private static IConversation MergeConversations(IConversation conversationA, IConversation conversationB)
        {
            IConversation mergedConversation = new MergedConversation(conversationA, conversationB);

            return mergedConversation;
        }
    }
}

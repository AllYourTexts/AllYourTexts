using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib.Conversation
{
    public abstract class ConversationManagerBase : IConversationManager
    {
        private List<IConversation> _conversations;

        protected void SetConversationList(List<IConversation> conversationList)
        {
            _conversations = conversationList;
        }

        public IConversation GetConversation(int conversationIndex)
        {
            if (conversationIndex < 0 || conversationIndex >= ConversationCount)
            {
                throw new ArgumentException("Invalid conversation index", "conversationIndex");
            }

            return _conversations[conversationIndex];
        }

        public int FindConversationIndex(IConversation conversation)
        {
            for (int conversationIndex = 0; conversationIndex < ConversationCount; conversationIndex++)
            {
                if (GetConversation(conversationIndex) == conversation)
                {
                    return conversationIndex;
                }
            }

            return -1;
        }

        public int ConversationCount
        {
            get
            {
                return _conversations.Count;
            }
        }

        public IEnumerator<IConversation> GetEnumerator()
        {
            for (int conversationIndex = 0; conversationIndex < ConversationCount; conversationIndex++)
            {
                yield return GetConversation(conversationIndex);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        protected void SetProgressPhase(ILoadingProgressCallback progressCallback, LoadingPhase loadingPhase)
        {
            if (progressCallback == null)
            {
                return;
            }

            progressCallback.CurrentPhase = loadingPhase;
        }

        protected static void IncrementWorkProgress(ILoadingProgressCallback progressCallback)
        {
            if (progressCallback == null)
            {
                return;
            }

            progressCallback.Increment(1);
        }

        protected static void CheckForCancel(ILoadingProgressCallback progressCallback)
        {
            if (progressCallback == null)
            {
                return;
            }

            if (progressCallback.IsCancelRequested)
            {
                progressCallback.AcceptCancel();
                throw new OperationCanceledException();
            }
        }
    }
}

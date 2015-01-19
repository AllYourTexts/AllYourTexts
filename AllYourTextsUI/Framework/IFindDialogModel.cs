using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib;

namespace AllYourTextsUi.Framework
{
    public enum SearchDirection
    {
        Up,
        Down
    }

    public interface IFindDialogModel
    {
        string Query { get; set; }

        bool CaseSensitive { get; set; }

        bool LastQuerySuccessful { get; }

        void SearchDisplayedConversation(SearchDirection searchDirection);

        void SearchAllConversations();

        event EventHandler SearchAllConversationsStarted;

        event EventHandler SearchAllConversationsCompleted;

        void CancelSearch();
    }
}

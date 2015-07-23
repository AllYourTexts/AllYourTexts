using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsUi.Framework;
using System.Windows.Controls;
using System.Windows.Documents;
using AllYourTextsLib.Framework;
using System.ComponentModel;
using System.Windows.Threading;
using AllYourTextsUi.ConversationRendering;

namespace AllYourTextsUi
{
    public class FindDialogModel : IFindDialogModel
    {
        public bool CaseSensitive { get; set; }

        public bool LastQuerySuccessful { get; private set; }

        public event EventHandler SearchAllConversationsStarted;

        public event EventHandler SearchAllConversationsCompleted;

        private string _query;

        private bool _queryChangedSinceLastSearch;

        private IConversationSearchTarget _searchTarget;

        private BackgroundWorker _searchWorker;

        private Dispatcher _dispatcher;

        private const string DefaultQuery = "";

        private const bool DefaultCaseSensitive = false;

        public FindDialogModel(IConversationSearchTarget searchTarget)
        {
            _query = null;

            _queryChangedSinceLastSearch = true;

            _searchTarget = searchTarget;

            _searchWorker = new BackgroundWorker();
            _searchWorker.WorkerReportsProgress = false;
            _searchWorker.WorkerSupportsCancellation = true;
            _searchWorker.DoWork += OnDoWork;
            _searchWorker.RunWorkerCompleted += OnWorkerCompleted;

            _dispatcher = ((RichTextBox)searchTarget.SearchTargetControl).Dispatcher;

            Query = DefaultQuery;

            CaseSensitive = DefaultCaseSensitive;

            LastQuerySuccessful = true;
        }

        public string Query
        {
            get
            {
                return _query;
            }
            set
            {
                if (_query != value)
                {
                    _query = value;
                    _queryChangedSinceLastSearch = true;
                }
            }
        }

        public void SearchDisplayedConversation(SearchDirection searchDirection)
        {
            LastQuerySuccessful = (SearchCurrentConversation(searchDirection, true) != null);

            _queryChangedSinceLastSearch = false;
        }

        public void SearchAllConversations()
        {
            if (SearchCurrentConversation(SearchDirection.Down, false) != null)
            {
                return;
            }

            if (SearchAllConversationsStarted != null)
            {
                SearchAllConversationsStarted(this, EventArgs.Empty);
            }

            _searchWorker.RunWorkerAsync(_searchTarget.CurrentConversation);
        }

        private TextRange SearchCurrentConversation(SearchDirection searchDirection, bool wrapSearch)
        {
            if (string.IsNullOrEmpty(Query))
            {
                return null;
            }

            TextPointer searchStartPointer;

            RichTextBox targetTextBox = (RichTextBox)_searchTarget.SearchTargetControl;

            if (_queryChangedSinceLastSearch)
            {
                ClearRichTextBoxSelection(targetTextBox);
            }

            if (searchDirection == SearchDirection.Down)
            {
                searchStartPointer = targetTextBox.Selection.End;
            }
            else
            {
                TextRange startToCaret = new TextRange(targetTextBox.Document.ContentStart, targetTextBox.CaretPosition);
                if (startToCaret.Text.Length == 0)
                {
                    searchStartPointer = targetTextBox.Document.ContentEnd;
                }
                else
                {
                    searchStartPointer = targetTextBox.Selection.Start;
                }
            }

            TextRange foundRange = FindWordFromPosition(searchStartPointer, Query, searchDirection);
            if (foundRange == null && wrapSearch)
            {
                if ((searchDirection == SearchDirection.Down) && (searchStartPointer != targetTextBox.Document.ContentStart))
                {
                    foundRange = FindWordFromPosition(targetTextBox.Document.ContentStart, Query, searchDirection);
                }
                else if ((searchDirection == SearchDirection.Up) && (searchStartPointer != targetTextBox.Document.ContentEnd))
                {
                    foundRange = FindWordFromPosition(targetTextBox.Document.ContentEnd, Query, searchDirection);
                }
            }

            if (foundRange != null)
            {
                targetTextBox.Focus();
                targetTextBox.Selection.Select(foundRange.Start, foundRange.End);

                return foundRange;
            }

            return null;
        }

        private TextRange FindWordFromPosition(TextPointer position, string word, SearchDirection searchDirection)
        {
            string wordToFind;

            if (!CaseSensitive)
            {
                wordToFind = word.ToLower();
            }
            else
            {
                wordToFind = word;
            }

            LogicalDirection logicalDirection = SearchDirectionToLogicalDirection(searchDirection);

            while (position != null)
            {
                if (position.Parent is ConversationContentRun)
                {
                    string textRun = position.GetTextInRun(logicalDirection);

                    int indexInRun = FindWordInString(wordToFind, textRun, searchDirection);

                    if (indexInRun >= 0)
                    {
                        int startOffset;

                        if (searchDirection == SearchDirection.Down)
                        {
                            startOffset = indexInRun;
                        }
                        else
                        {
                            startOffset = -1 * (textRun.Length - indexInRun);
                        }

                        TextPointer start = position.GetPositionAtOffset(startOffset, logicalDirection);
                        TextPointer end = start.GetPositionAtOffset(wordToFind.Length, logicalDirection);
                        return new TextRange(start, end);
                    }
                }

                position = position.GetNextContextPosition(logicalDirection);
            }

            return null;
        }

        private LogicalDirection SearchDirectionToLogicalDirection(SearchDirection searchDirection)
        {
            if (searchDirection == SearchDirection.Down)
            {
                return LogicalDirection.Forward;
            }
            else
            {
                return LogicalDirection.Backward;
            }
        }

        private int FindWordInString(string wordToFind, string toSearch, SearchDirection searchDirection)
        {
            if (!CaseSensitive)
            {
                toSearch = toSearch.ToLower();
            }

            int wordIndex;

            if (searchDirection == SearchDirection.Down)
            {
                wordIndex = toSearch.IndexOf(wordToFind);
            }
            else
            {
                wordIndex = toSearch.LastIndexOf(wordToFind);
            }

            return wordIndex;
        }

        private IConversation SearchAllNonCurrentConversations(IConversation startingConversation, SearchDirection searchDirection)
        {
            IConversation currentConversation = GetNextConversationToSearch(startingConversation, searchDirection);

            string wordToFind;

            if (!CaseSensitive)
            {
                wordToFind = Query.ToLower();
            }
            else
            {
                wordToFind = Query;
            }

            while (currentConversation != startingConversation)
            {
                if (_searchWorker.CancellationPending)
                {
                    throw new OperationCanceledException();
                }

                if (WordAppearsInConversation(wordToFind, currentConversation, searchDirection))
                {
                    return currentConversation;
                }

                currentConversation = GetNextConversationToSearch(currentConversation, searchDirection);
            }

            return null;
        }

        private IConversation GetNextConversationToSearch(IConversation currentConversation, SearchDirection searchDirection)
        {
            if (searchDirection == SearchDirection.Down)
            {
                return _searchTarget.NextConversation(currentConversation);
            }
            else
            {
                return _searchTarget.PreviousConversation(currentConversation);
            }
        }

        private bool WordAppearsInConversation(string wordToFind, IConversation conversation, SearchDirection searchDirection)
        {
            foreach (IConversationMessage message in conversation)
            {
                if (string.IsNullOrEmpty(message.MessageContents))
                {
                    continue;
                }
                if (FindWordInString(wordToFind, message.MessageContents, searchDirection) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        private void OnDoWork(object sender, DoWorkEventArgs e)
        {
            IConversation startingConversation = (IConversation)e.Argument;

            IConversation matchingConversation = null;

            try
            {
                matchingConversation = SearchAllNonCurrentConversations(startingConversation, SearchDirection.Down);
            }
            catch (OperationCanceledException)
            {
                e.Cancel = true;
                return;
            }

            if (matchingConversation != null)
            {
                if (_searchWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                _searchTarget.PrepareWaitForRenderComplete();

                _dispatcher.BeginInvoke(DispatcherPriority.SystemIdle,
                                        new RenderDelegate(RenderConversationInSearchTarget),
                                        matchingConversation,
                                        _searchTarget);

                _searchTarget.WaitForRenderComplete();

                e.Result = true;
                return;
            }

            e.Result = false;
        }

        private void ClearRichTextBoxSelection(RichTextBox richTextBoxToClear)
        {
            richTextBoxToClear.Selection.Select(richTextBoxToClear.Document.ContentStart,
                                                richTextBoxToClear.Document.ContentStart);
        }

        private delegate void RenderDelegate(IConversation conversation, IConversationSearchTarget target);

        private void RenderConversationInSearchTarget(IConversation conversation, IConversationSearchTarget target)
        {
            target.CurrentConversation = conversation;
        }

        private void OnWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                ;
            }
            else if (e.Error != null)
            {
                FailHandler.HandleUnrecoverableFailure(e.Error);
            }
            else
            {
                LastQuerySuccessful = (bool)e.Result;

                if (LastQuerySuccessful)
                {
                    SearchCurrentConversation(SearchDirection.Down, false);
                }
            }

            if (SearchAllConversationsCompleted != null)
            {
                SearchAllConversationsCompleted(this, EventArgs.Empty);
            }
        }
        
        public void CancelSearch()
        {
            _searchWorker.CancelAsync();   
        }
    }
}

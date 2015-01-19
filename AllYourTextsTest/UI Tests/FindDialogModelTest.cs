using AllYourTextsUi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Controls;
using System.Windows.Documents;
using DummyData;
using System.Collections.Generic;
using AllYourTextsLib.Framework;
using AllYourTextsUi.Framework;
using AllYourTextsUi.ConversationRendering;
using AllYourTextsTest.Mock_Objects;

namespace AllYourTextsTest
{
    
    
    /// <summary>
    ///This is a test class for FindDialogModelTest and is intended
    ///to contain all FindDialogModelTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FindDialogModelTest
    {

        private static RichTextBox GetEmptyRichTextBox()
        {
            RichTextBox emptyRichTextBox = new RichTextBox();
            emptyRichTextBox.Document.Blocks.Clear();

            return emptyRichTextBox;
        }

        private static RichTextBox GetSingleLineRichTextBox()
        {
            return CreateRichTextBoxWithContent("That dog costs $350^2 in spacebucks!!! (*$1 spacebuck = $5 US bucks*)");
        }

        private static RichTextBox GetConversationRichTextBox()
        {
            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.ObamaCell);

            ConversationRendererRichText renderer = new ConversationRendererRichText(new MockDisplayOptions(), conversation);

            IEnumerable<Paragraph> paragraphs = renderer.RenderMessagesAsParagraphs(ConversationRendererRichText.RenderAllMessages);

            RichTextBox conversationRichTextBox = new RichTextBox();
            conversationRichTextBox.Document.Blocks.Clear();
            conversationRichTextBox.Document.Blocks.AddRange(paragraphs);

            return conversationRichTextBox;
        }

        private static RichTextBox CreateRichTextBoxWithContent(string content)
        {
            RichTextBox richTextBox = new RichTextBox();
            richTextBox.Document.Blocks.Clear();

            Paragraph paragraph = new Paragraph(new ConversationContentRun(content));
            richTextBox.Document.Blocks.Add(paragraph);

            return richTextBox;
        }

        private static void GetEmptyDialogModel(out MockConversationSearchTarget searchTarget, out IFindDialogModel findDialogModel)
        {
            searchTarget = new MockConversationSearchTarget();
            searchTarget.SearchTargetControl = GetEmptyRichTextBox();
            
            findDialogModel = new FindDialogModel(searchTarget);
        }

        private static void GetSingleLineDialogModel(out MockConversationSearchTarget searchTarget, out IFindDialogModel findDialogModel)
        {
            searchTarget = new MockConversationSearchTarget();
            searchTarget.SearchTargetControl = GetSingleLineRichTextBox();
            
            findDialogModel = new FindDialogModel(searchTarget);
        }

        private static void GetConversationDialogModel(out MockConversationSearchTarget searchTarget, out IFindDialogModel findDialogModel)
        {
            searchTarget = new MockConversationSearchTarget();
            searchTarget.SearchTargetControl = GetConversationRichTextBox();

            findDialogModel = new FindDialogModel(searchTarget);
        }

        private bool WordIsFound(IFindDialogModel model, string word)
        {
            model.Query = word;
            model.SearchDisplayedConversation(SearchDirection.Down);

            return model.LastQuerySuccessful;
        }

        private bool WordIsFoundReverse(IFindDialogModel model, string word)
        {
            model.Query = word;
            model.SearchDisplayedConversation(SearchDirection.Up);

            return model.LastQuerySuccessful;
        }

        private bool WordIsFoundMatchCase(IFindDialogModel model, string word)
        {
            model.CaseSensitive = true;

            return WordIsFound(model, word);
        }

        private int WordInstancesFound(IFindDialogModel model, MockConversationSearchTarget searchTarget, string word)
        {
            int instancesFound = 0;

            int lastMatchIndex = 0;

            while (model.LastQuerySuccessful)
            {
                model.Query = word;
                model.SearchDisplayedConversation(SearchDirection.Down);

                if (!model.LastQuerySuccessful || (searchTarget.MatchIndex <= lastMatchIndex))
                {
                    break;
                }
                else
                {
                    lastMatchIndex = searchTarget.MatchIndex;
                }

                instancesFound++;
            }

            return instancesFound;
        }

        private int WordInstancesFoundMatchCase(IFindDialogModel model, MockConversationSearchTarget searchTarget, string word)
        {
            model.CaseSensitive = true;

            return WordInstancesFound(model, searchTarget, word);
        }

        private int WordInstancesFoundReverse(IFindDialogModel model, MockConversationSearchTarget searchTarget, string word)
        {
            int instancesFound = 0;

            int lastMatchIndex = int.MaxValue;

            while (model.LastQuerySuccessful)
            {
                model.Query = word;
                model.SearchDisplayedConversation(SearchDirection.Up);

                if (!model.LastQuerySuccessful || (searchTarget.MatchIndex > lastMatchIndex))
                {
                    break;
                }
                else
                {
                    lastMatchIndex = searchTarget.MatchIndex;
                }

                instancesFound++;
            }

            return instancesFound;
        }

        [TestMethod()]
        public void EmptyQueryTest()
        {
            IFindDialogModel model;
            MockConversationSearchTarget searchTarget;

            GetSingleLineDialogModel(out searchTarget, out model);
            model.Query = "";
            model.SearchDisplayedConversation(SearchDirection.Down);

            Assert.IsFalse(model.LastQuerySuccessful);
            Assert.AreEqual(-1, searchTarget.MatchIndex);
        }

        [TestMethod()]
        public void SimpleWordFindTest()
        {
            IFindDialogModel model;
            MockConversationSearchTarget searchTarget;

            GetSingleLineDialogModel(out searchTarget, out model);
            Assert.AreEqual(1, WordInstancesFound(model, searchTarget, "dog"));
            
            GetSingleLineDialogModel(out searchTarget, out model);
            Assert.AreEqual(0, WordInstancesFound(model, searchTarget, "dig"));
            
            GetSingleLineDialogModel(out searchTarget, out model);
            Assert.AreEqual(1, WordInstancesFound(model, searchTarget, "$350^2"));
            
            GetSingleLineDialogModel(out searchTarget, out model);
            Assert.AreEqual(1, WordInstancesFound(model, searchTarget, "^2"));

            GetEmptyDialogModel(out searchTarget, out model);
            Assert.AreEqual(0, WordInstancesFound(model, searchTarget, "a"));

            GetConversationDialogModel(out searchTarget, out model);
            Assert.AreEqual(1, WordInstancesFound(model, searchTarget, "compound"));

            GetConversationDialogModel(out searchTarget, out model);
            Assert.AreEqual(0, WordInstancesFound(model, searchTarget, "compount"));
        }

        [TestMethod()]
        public void CaseSensitivityTest()
        {
            IFindDialogModel model;
            MockConversationSearchTarget searchTarget;

            GetSingleLineDialogModel(out searchTarget, out model);
            Assert.AreEqual(1, WordInstancesFoundMatchCase(model, searchTarget, "dog"));

            GetSingleLineDialogModel(out searchTarget, out model);
            Assert.AreEqual(0, WordInstancesFoundMatchCase(model, searchTarget, "DOG"));

            GetSingleLineDialogModel(out searchTarget, out model);
            Assert.AreEqual(1, WordInstancesFound(model, searchTarget, "DOG"));
        }

        [TestMethod()]
        public void ReverseSearchTest()
        {
            IFindDialogModel model;
            MockConversationSearchTarget searchTarget;

            GetSingleLineDialogModel(out searchTarget, out model);
            Assert.AreEqual(3, WordInstancesFoundReverse(model, searchTarget, "buck"));

            GetSingleLineDialogModel(out searchTarget, out model);
            Assert.AreEqual(0, WordInstancesFoundReverse(model, searchTarget, "dig"));
        }

        [TestMethod()]
        public void ForwardReverseSearchTest()
        {
            IFindDialogModel model;
            MockConversationSearchTarget searchTarget;

            GetSingleLineDialogModel(out searchTarget, out model);
            model.Query = "buck";

            const int SearchCount = 3;
            int[] forwardMatchIndices = new int[SearchCount];

            for (int i = 0; i < SearchCount; i++)
            {
                model.SearchDisplayedConversation(SearchDirection.Down);
                forwardMatchIndices[i] = searchTarget.MatchIndex;
            }

            int[] reverseMatchIndices = new int[SearchCount];
            for (int i = 0; i < SearchCount; i++)
            {
                model.SearchDisplayedConversation(SearchDirection.Up);
                reverseMatchIndices[i] = searchTarget.MatchIndex;
            }

            Assert.AreEqual(forwardMatchIndices[0], reverseMatchIndices[1]);
            Assert.AreEqual(forwardMatchIndices[1], reverseMatchIndices[0]);
            Assert.AreEqual(forwardMatchIndices[2], reverseMatchIndices[2]);
        }

        private int FindWordInStringHelper(string toFind, string target, SearchDirection direction, bool isCaseSensitive)
        {
            MockConversationSearchTarget searchTarget = new MockConversationSearchTarget();
            searchTarget.SearchTargetControl = CreateRichTextBoxWithContent(target);

            FindDialogModel_Accessor model = new FindDialogModel_Accessor(searchTarget);

            model.CaseSensitive = isCaseSensitive;

            return model.FindWordInString(toFind, target, direction);
        }

        [TestMethod()]
        public void FindWordInStringTest()
        {
            Assert.AreEqual(23, FindWordInStringHelper("xyz", "abcdefghijklmnopqrstuvwxyz", SearchDirection.Down, false));
            Assert.AreEqual(23, FindWordInStringHelper("xyz", "abcdefghijklmnopqrstuvwxyz", SearchDirection.Up, false));
            Assert.AreEqual(23, FindWordInStringHelper("xyz", "abcdefghijklmnopqrstuvwxyz", SearchDirection.Down, true));

            Assert.AreEqual(0, FindWordInStringHelper("the", "the eagle flies at noon", SearchDirection.Down, false));
            Assert.AreEqual(0, FindWordInStringHelper("the", "the eagle flies at noon", SearchDirection.Up, false));

            Assert.AreEqual(4, FindWordInStringHelper("head", "two heads are better than one head", SearchDirection.Down, false));
            Assert.AreEqual(30, FindWordInStringHelper("head", "two heads are better than one head", SearchDirection.Up, false));

            Assert.AreEqual(-1, FindWordInStringHelper("monsters", "under your bed", SearchDirection.Down, false));

            Assert.AreEqual(4, FindWordInStringHelper("CAT", "the CAT in the hat", SearchDirection.Down, true));
            Assert.AreEqual(-1, FindWordInStringHelper("cat", "the CAT in the hat", SearchDirection.Down, true));
            Assert.AreEqual(-1, FindWordInStringHelper("CAT", "the cat in the hat", SearchDirection.Down, true));
        }
    }
}

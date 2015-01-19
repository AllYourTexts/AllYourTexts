using System.Collections.Generic;
using System.Windows.Documents;
using AllYourTextsLib.Framework;
using AllYourTextsUi;
using AllYourTextsUi.Framework;
using DummyData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AllYourTextsUi.ConversationRendering;
using AllYourTextsTest.Mock_Objects;

namespace AllYourTextsTest
{
    
    
    /// <summary>
    ///This is a test class for ConversationRendererTest and is intended
    ///to contain all ConversationRendererTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ConversationRendererRichTextTest
    {

        private void VerifyRunsEqual(Run runExpected, Run runActual)
        {
            Assert.AreEqual(runExpected.Text, runActual.Text);
        }

        private void VerifySpansEqual(Span spanExpected, Span spanActual)
        {
            VerifyInlineCollectionsMatch(spanExpected.Inlines, spanActual.Inlines);
        }

        private void VerifyInlineCollectionsMatch(InlineCollection inlineCollectionExpected, InlineCollection inlineCollectionActual)
        {
            VerifyInlineListsMatch(new List<Inline>(inlineCollectionExpected), new List<Inline>(inlineCollectionActual));
        }

        private void VerifyInlineListsMatch(List<Inline> inlineListExpected, List<Inline> inlineListActual)
        {
            Assert.AreEqual(inlineListExpected.Count, inlineListActual.Count);

            int inlineCount = inlineListExpected.Count;
            List<Inline> inlinesExpected = new List<Inline>(inlineListExpected);
            List<Inline> inlinesActual = new List<Inline>(inlineListActual);

            for (int i = 0; i < inlineCount; i++)
            {
                Inline inlineExpected = inlinesExpected[i];
                Inline inlineActual = inlinesActual[i];

                Assert.IsInstanceOfType(inlineActual, inlineExpected.GetType());

                if (inlineExpected is Run)
                {
                    VerifyRunsEqual((Run)inlineExpected, (Run)inlineActual);
                }
                else if (inlineExpected is Span)
                {
                    VerifySpansEqual((Span)inlineExpected, (Span)inlineActual);
                }
            }
        }

        private void VerifyParagraphsEqual(Paragraph paragraphExpected, Paragraph paragraphActual)
        {
            VerifyInlineCollectionsMatch(paragraphExpected.Inlines, paragraphActual.Inlines);
        }

        private void VerifyParagraphListsEqual(List<Paragraph> paragraphsExpected, List<Paragraph> paragraphsActual)
        {
            Assert.AreEqual(paragraphsExpected.Count, paragraphsActual.Count);

            for (int i = 0; i < paragraphsActual.Count; i++)
            {
                Paragraph paragraphExpected = paragraphsExpected[i];
                Paragraph paragraphActual = paragraphsActual[i];

                VerifyParagraphsEqual(paragraphExpected, paragraphActual);
            }
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void NoMessageConversationTest()
        {
            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.NeverTexterCell);
            IDisplayOptions displayOptions = new MockDisplayOptions();

            ConversationRendererRichText renderer = new ConversationRendererRichText(displayOptions, conversation);

            List<Paragraph> paragraphsActual = renderer.RenderMessagesAsParagraphs(ConversationRendererRichText.RenderAllMessages);
            Assert.AreEqual(1, paragraphsActual.Count);

            Paragraph paragraphExpected = new Paragraph(new Italic(new Run(ConversationRendererRichText_Accessor._noConversationMessage)));
            VerifyParagraphsEqual(paragraphExpected, paragraphsActual[0]);

            //
            // Calling RenderMessages after messages have been rendered should return empty list.
            //

            paragraphsActual = renderer.RenderMessagesAsParagraphs(ConversationRendererRichText.RenderAllMessages);
            Assert.AreEqual(0, paragraphsActual.Count);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void SingleMessageConversationTest()
        {
            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.UnknownEagle);
            IDisplayOptions displayOptions = new MockDisplayOptions();

            ConversationRendererRichText_Accessor renderer = new ConversationRendererRichText_Accessor(displayOptions, conversation);

            List<Paragraph> paragraphsActual = renderer.RenderMessagesAsParagraphs(ConversationRendererRichText.RenderAllMessages);
            Assert.AreEqual(1, paragraphsActual.Count);

            IConversationMessage message = conversation.GetMessage(0);

            Paragraph paragraphExpected = new Paragraph();
            paragraphExpected.Inlines.Add(ConversationRendererRichText_Accessor.DateAsInline(message.Timestamp));
            paragraphExpected.Inlines.Add(new LineBreak());
            paragraphExpected.Inlines.AddRange(renderer.ConversationMessageToInlines(message));

            VerifyParagraphsEqual(paragraphExpected, paragraphsActual[0]);

            paragraphsActual = renderer.RenderMessagesAsParagraphs(ConversationRendererRichText.RenderAllMessages);
            Assert.AreEqual(0, paragraphsActual.Count);
        }

        private IConversation GetMultiDayConversation()
        {
            return DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.HarryLooseTieCell);
        }

        private List<Paragraph> GetMultiDayConversationRenderingExpected(IConversation conversation, ConversationRendererRichText_Accessor renderer)
        {
            List<IConversationMessage> messages = new List<IConversationMessage>(conversation);
            List<Paragraph> paragraphsExpected = new List<Paragraph>();

            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(ConversationRendererRichText_Accessor.DateAsInline(messages[0].Timestamp));
            paragraph.Inlines.Add(new LineBreak());
            paragraph.Inlines.AddRange(renderer.ConversationMessageToInlines(messages[0]));
            paragraph.Inlines.Add(new LineBreak());
            paragraph.Inlines.AddRange(renderer.ConversationMessageToInlines(messages[1]));
            paragraphsExpected.Add(paragraph);

            paragraph = new Paragraph();
            paragraph.Inlines.Add(new LineBreak());
            paragraph.Inlines.Add(ConversationRendererRichText_Accessor.DateAsInline(messages[2].Timestamp));
            paragraph.Inlines.Add(new LineBreak());
            paragraph.Inlines.AddRange(renderer.ConversationMessageToInlines(messages[2]));
            paragraph.Inlines.Add(new LineBreak());
            paragraph.Inlines.AddRange(renderer.ConversationMessageToInlines(messages[3]));
            paragraphsExpected.Add(paragraph);

            return paragraphsExpected;
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void MultiDayConversationTest()
        {
            IConversation conversation = GetMultiDayConversation();
            IDisplayOptions displayOptions = new MockDisplayOptions();

            ConversationRendererRichText_Accessor renderer = new ConversationRendererRichText_Accessor(displayOptions, conversation);

            List<Paragraph> paragraphsActual = renderer.RenderMessagesAsParagraphs(ConversationRendererRichText.RenderAllMessages);

            List<Paragraph> paragraphsExpected = GetMultiDayConversationRenderingExpected(conversation, renderer);
            
            VerifyParagraphListsEqual(paragraphsExpected, paragraphsActual);

            paragraphsActual = renderer.RenderMessagesAsParagraphs(ConversationRendererRichText.RenderAllMessages);
            Assert.AreEqual(0, paragraphsActual.Count);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void RenderIncrementallyTest()
        {
            IConversation conversation = GetMultiDayConversation();
            IDisplayOptions displayOptions = new MockDisplayOptions();

            ConversationRendererRichText_Accessor renderer = new ConversationRendererRichText_Accessor(displayOptions, conversation);

            List<Paragraph> paragraphsActual = new List<Paragraph>();

            const int MessageRenderSize = 1;

            List<Paragraph> paragraphsCurrent;
            while (renderer.HasUnprocessedMessages)
            {
                paragraphsCurrent = renderer.RenderMessagesAsParagraphs(MessageRenderSize);
                paragraphsActual.AddRange(paragraphsCurrent);
            }

            List<Paragraph> paragraphsExpected = GetMultiDayConversationRenderingExpected(conversation, renderer);

            VerifyParagraphListsEqual(paragraphsExpected, paragraphsActual);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void RenderSingleMessage24HourTimeTest()
        {
            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.UnknownEagle);
            IConversationMessage message = conversation.GetMessage(0);
            MockDisplayOptions displayOptions = new MockDisplayOptions();

            displayOptions.TimeDisplayFormat = TimeDisplayFormat.HourMinSec24h;

            ConversationRendererRichText_Accessor renderer = new ConversationRendererRichText_Accessor(displayOptions, conversation);

            List<Inline> inlineListExpected = new List<Inline>();
            Span messagePrefix = new Span();
            messagePrefix.Inlines.Add(new Bold(new Run("Unknown Sender")));
            messagePrefix.Inlines.Add(new Run(" (\u200E20:38:17\u202C)"));
            messagePrefix.Inlines.Add(new Run(": "));
            inlineListExpected.Add(messagePrefix);
            inlineListExpected.Add(new ConversationContentRun(message.MessageContents));

            List<Inline> inlineListActual = new List<Inline>();
            inlineListActual.AddRange(renderer.ConversationMessageToInlines(message));

            VerifyInlineListsMatch(inlineListExpected, inlineListActual);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void RenderSingleMessageHideTimeTest()
        {
            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.UnknownEagle);
            IConversationMessage message = conversation.GetMessage(0);
            MockDisplayOptions displayOptions = new MockDisplayOptions();

            displayOptions.TimeDisplayFormat = TimeDisplayFormat.HideTime;

            ConversationRendererRichText_Accessor renderer = new ConversationRendererRichText_Accessor(displayOptions, conversation);

            List<Inline> inlineListExpected = new List<Inline>();
            Span messagePrefix = new Span();
            messagePrefix.Inlines.Add(new Bold(new Run("Unknown Sender")));
            messagePrefix.Inlines.Add(new Run(": "));
            inlineListExpected.Add(messagePrefix);
            inlineListExpected.Add(new ConversationContentRun(message.MessageContents));

            List<Inline> inlineListActual = new List<Inline>();
            inlineListActual.AddRange(renderer.ConversationMessageToInlines(message));

            VerifyInlineListsMatch(inlineListExpected, inlineListActual);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void GetSenderColorMergedConversationTest()
        {
            IConversation conversation = DummyConversationDataGenerator.GetMergedConversation(DummyContactId.Davola);
            IConversationMessage cellIncomingMessage = conversation.GetMessage(0);
            IConversationMessage iPhoneIncomingMessage = conversation.GetMessage(11);
            MockDisplayOptions displayOptions = new MockDisplayOptions();

            ConversationRendererRichText_Accessor renderer = new ConversationRendererRichText_Accessor(displayOptions, conversation);
            System.Windows.Media.Color cellColor = renderer.GetSenderDisplayColor(cellIncomingMessage);
            System.Windows.Media.Color iPhoneColor = renderer.GetSenderDisplayColor(iPhoneIncomingMessage);

            Assert.AreEqual(cellColor, iPhoneColor);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void GetSenderColorUnknownContactTest()
        {
            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.UnknownEagle);
            IConversationMessage incomingMessage = conversation.GetMessage(0);
            MockDisplayOptions displayOptions = new MockDisplayOptions();

            ConversationRendererRichText_Accessor renderer = new ConversationRendererRichText_Accessor(displayOptions, conversation);
            System.Windows.Media.Color incomingMessageColor = renderer.GetSenderDisplayColor(incomingMessage);

            Assert.IsNotNull(incomingMessageColor);
        }
    }
}

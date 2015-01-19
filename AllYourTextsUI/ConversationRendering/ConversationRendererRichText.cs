using System;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Windows.Media;
using AllYourTextsLib.Framework;
using AllYourTextsUi.Controls;
using AllYourTextsUi.Framework;
using System.Windows.Controls;
using System.Windows;
using AllYourTextsUi.Models;
using AllYourTextsUi.ConversationRendering;

namespace AllYourTextsUi
{
    public class ConversationRendererRichText : ConversationRendererBase
    {
        private Paragraph _currentParagraph;

        private List<Paragraph> _paragraphs;

        private IFileSystem _fileSystem;

        public ConversationRendererRichText(IDisplayOptionsReadOnly displayOptions, IConversation conversation)
            :base(displayOptions, conversation)
        {
            _currentParagraph = null;

            _fileSystem = new OsFileSystem();
        }

        public override string RenderMessagesAsString(int messageCount)
        {
            throw new NotImplementedException(); // Temporary method while refactoring ConversationRenderer classes
        }

        public override List<Paragraph> RenderMessagesAsParagraphs(int messageCount)
        {
            RenderMessagesImpl(messageCount);
            return _paragraphs;
        }

        protected override void ClearRenderingBuffer()
        {
            _paragraphs = new List<Paragraph>();
        }

        protected override void StartNewParagraph()
        {
            MoveCurrentParagraphToBuffer();

            _currentParagraph = new Paragraph();

            if (!IsParagraphBufferEmpty())
            {
                AddLineBreak();
            }
        }

        protected override void MoveCurrentParagraphToBuffer()
        {
            if (_currentParagraph != null)
            {
                _paragraphs.Add(_currentParagraph);
            }
        }

        private bool IsParagraphBufferEmpty()
        {
            return (_paragraphs.Count == 0);
        }

        protected override void AddDateLine(DateTime date)
        {
            _currentParagraph.Inlines.Add(DateAsInline(date));
        }

        protected override void AddMessageLine(IConversationMessage message)
        {
            IEnumerable<Inline> messageInlines = ConversationMessageToInlines(message);
            _currentParagraph.Inlines.AddRange(messageInlines);
        }

        protected override void AddLineBreak()
        {
            _currentParagraph.Inlines.Add(new LineBreak());
        }

        protected override void AddEmptyConversationMessage()
        {
            List<Paragraph> emptyParagraphList = new List<Paragraph>();

            Italic noConversationItalic = new Italic(new Run(_noConversationMessage));
            emptyParagraphList.Add(new Paragraph(noConversationItalic));

            _paragraphs = emptyParagraphList;
        }

        private static Inline DateAsInline(DateTime date)
        {
            string dateString = FormatDateForConversation(date);

            return new Italic(new Bold(new Run(dateString)));
        }

        private IEnumerable<Inline> ConversationMessageToInlines(IConversationMessage message)
        {
            string senderNameString;
            string timestampString;
            string messageContents = message.MessageContents;

            List<Inline> messageInlines = new List<Inline>();
            SolidColorBrush senderBrush;

            Span messagePrefixSpan = new Span();

            senderNameString = GetSenderDisplayName(message);
            senderBrush = GetSenderBrush(message);

            Bold senderName = new Bold(new Run(senderNameString));
            messagePrefixSpan.Foreground = senderBrush;
            messagePrefixSpan.Inlines.Add(senderName);

            timestampString = FormatTimeForConversation(message.Timestamp);
            if (timestampString != null)
            {
                string formattedTimestamp = string.Format(" (" + '\u200E' + "{0}" + '\u202C' + ")", timestampString);
                Run timestampRun = new Run(formattedTimestamp);
                messagePrefixSpan.Inlines.Add(timestampRun);
            }

            messagePrefixSpan.Inlines.Add(new Run(": "));

            messageInlines.Add(messagePrefixSpan);

            messageInlines.Add(new ConversationContentRun(messageContents));

            return messageInlines;
        }

        protected override void AddAttachment(IMessageAttachment messageAttachment)
        {
            UIElement attachmentElement;
            AttachmentModel attachmentModel = new AttachmentModel(_fileSystem, messageAttachment);
            switch (messageAttachment.Type)
            {
                case AttachmentType.Image:
                    attachmentElement = new AttachmentImage(attachmentModel);
                    break;
                case AttachmentType.Video:
                    attachmentElement = new AttachmentVideo(attachmentModel);
                    break;
                case AttachmentType.Audio:
                    attachmentElement = new AttachmentAudio(attachmentModel);
                    break;
                default:
                    throw new ArgumentException("Unrecognized attachment type: " + messageAttachment.Type.ToString());
            }

            _currentParagraph.Inlines.Add(attachmentElement);
        }

        private SolidColorBrush GetSenderBrush(IConversationMessage message)
        {
            return new SolidColorBrush(GetSenderDisplayColor(message));
        }
    }
}

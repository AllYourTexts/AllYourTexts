using System;
using System.Text;
using System.Windows.Media;
using AllYourTextsLib.Framework;
using AllYourTextsUi.Framework;
using AllYourTextsUi.Exporting;
using System.Collections.Generic;

namespace AllYourTextsUi
{
    public class ConversationRendererHtml : ConversationRendererBase
    {
        private StringBuilder _htmlBuffer;

        private IAttachmentExportLocator _attachmentExportLocator;

        public ConversationRendererHtml(IDisplayOptionsReadOnly displayOptions, IConversation conversation, IAttachmentExportLocator attachmentExportLocator)
            :base(displayOptions, conversation)
        {
            _attachmentExportLocator = attachmentExportLocator;
        }

        public override string RenderMessagesAsString(int messageCount)
        {
            RenderMessagesImpl(messageCount);
            return _htmlBuffer.ToString();
        }

        public override List<System.Windows.Documents.Paragraph> RenderMessagesAsParagraphs(int messageCount)
        {
            throw new NotImplementedException();
        }

        protected override void ClearRenderingBuffer()
        {
            _htmlBuffer = new StringBuilder();
        }

        protected override void StartNewParagraph()
        {
            if (_htmlBuffer.Length == 0)
            {
                _htmlBuffer.AppendLine("<p>");
            }
            else
            {
                _htmlBuffer.AppendLine();
                _htmlBuffer.AppendLine("</p>");
                _htmlBuffer.AppendLine();
                _htmlBuffer.AppendLine("<p>");
            }
        }

        protected override void MoveCurrentParagraphToBuffer()
        {
            _htmlBuffer.AppendLine();
            _htmlBuffer.AppendLine("</p>");
        }

        protected override void AddDateLine(DateTime date)
        {
            string format = "<span class=\"date\">{0}</span>";
            _htmlBuffer.AppendFormat(format, FormatDateForConversation(date));
        }

        protected override void AddMessageLine(IConversationMessage message)
        {
            const string MessageLineFormat = "<span style=\"color:{0};\"><span class=\"senderName\">{1}</span> <span class=\"timestamp\">(<span dir=\"ltr\" lang=\"en\">{2}</span>)</span>: </span>{3}";

            string prefixColor;
            string contactName;

            prefixColor = GetMessagePrefixColor(message);

            contactName = EscapeHtml(GetSenderDisplayName(message));

            string timestamp = FormatTimeForConversation(message.Timestamp);
            string messageContents = EscapeHtml(message.MessageContents);

            _htmlBuffer.AppendFormat(MessageLineFormat, prefixColor, contactName, timestamp, messageContents);
        }

        private string EscapeHtml(string unescapedHtml)
        {
            return System.Security.SecurityElement.Escape(unescapedHtml);
        }

        protected override void AddLineBreak()
        {
            _htmlBuffer.AppendLine("<br />");
        }

        protected override void AddEmptyConversationMessage()
        {
            _htmlBuffer.Append("<p>" + _noConversationMessage + "</p>");
        }

        private string GetMessagePrefixColor(IConversationMessage message)
        {
            Color displayColor = GetSenderDisplayColor(message);

            return ColorToCssAttribute(displayColor);
        }

        private string ColorToCssAttribute(Color color)
        {
            string cssFormat = "rgb({0},{1},{2})";

            return string.Format(cssFormat, color.R, color.G, color.B);
        }

        protected override void AddAttachment(IMessageAttachment messageAttachment)
        {
            if (messageAttachment.Type == AttachmentType.Image)
            {
                AddImageAttachment(messageAttachment);
            }
            else if (messageAttachment.Type == AttachmentType.Video)
            {
                AddVideoAttachment(messageAttachment);
            }
        }

        private void AddImageAttachment(IMessageAttachment imageAttachment)
        {
            string relativeFilename = GetRelativeFilenameForAttachment(imageAttachment);
            _htmlBuffer.AppendFormat("<a href=\"{0}\" target=\"_blank\"><img class=\"attachmentImage\" src=\"{0}\" /></a>", relativeFilename);
        }

        private void AddVideoAttachment(IMessageAttachment videoAttachment)
        {
            string attachmentRelativeFilename = GetRelativeFilenameForAttachment(videoAttachment);
            string videoIconRelativeFilename = GetRelativeFilenameForFile(ConversationExporterHtml.VideoIconOutputFilename);
            _htmlBuffer.AppendFormat("<a href=\"{0}\" target=\"_blank\"><img class=\"attachmentVideo\" src=\"{1}\" /></a>\r\n", attachmentRelativeFilename, videoIconRelativeFilename);
        }

        private string GetRelativeFilenameForAttachment(IMessageAttachment attachment)
        {
            return _attachmentExportLocator.GetAttachmentExportRelativePath(attachment);
        }

        private string GetRelativeFilenameForFile(string originalFilename)
        {
            return _attachmentExportLocator.GetFileExportRelativePath(originalFilename);
        }
    }
}

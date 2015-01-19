using AllYourTextsLib.Framework;
using AllYourTextsUi.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AllYourTextsUi
{
    public class ConversationRendererPlaintext : ConversationRendererBase
    {
        StringBuilder _renderBuffer;

        public ConversationRendererPlaintext(IDisplayOptionsReadOnly displayOptions, IConversation conversation)
            :base(displayOptions, conversation)
        {
            ;
        }

        public override string RenderMessagesAsString(int messageCount)
        {
            RenderMessagesImpl(messageCount);
            return _renderBuffer.ToString();
        }

        public override List<System.Windows.Documents.Paragraph> RenderMessagesAsParagraphs(int messageCount)
        {
            throw new NotImplementedException();
        }

        protected override void ClearRenderingBuffer()
        {
            _renderBuffer = new StringBuilder();
        }

        protected override void StartNewParagraph()
        {
            if (_renderBuffer.Length > 0)
            {
                _renderBuffer.AppendLine();
                _renderBuffer.AppendLine();
            }
        }

        protected override void MoveCurrentParagraphToBuffer()
        {
            ;
        }

        protected override void AddDateLine(DateTime date)
        {
            _renderBuffer.Append(FormatDateForConversation(date));
        }

        protected override void AddMessageLine(IConversationMessage message)
        {
            string messageLineFormat = "{0} (" + '\u200E' + "{1}" + '\u202C' + "): {2}";

            string contactName = GetSenderDisplayName(message);

            string timestamp = FormatTimeForConversation(message.Timestamp);
            string messageContents = message.MessageContents;

            _renderBuffer.AppendFormat(messageLineFormat, contactName, timestamp, messageContents);
        }

        protected override void AddLineBreak()
        {
            _renderBuffer.AppendLine();
        }

        protected override void AddEmptyConversationMessage()
        {
            _renderBuffer.Append(_noConversationMessage);
        }

        protected override void AddAttachment(IMessageAttachment messageAttachment)
        {
            //
            // Plaintext renderer ignores message attachments.
            //
        }
    }
}

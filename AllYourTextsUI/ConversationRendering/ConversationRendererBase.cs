using System;
using System.Collections.Generic;
using System.Windows.Media;
using AllYourTextsLib.Framework;
using AllYourTextsUi.Framework;

namespace AllYourTextsUi
{
    public abstract class ConversationRendererBase : IConversationRenderer
    {
        private IDisplayOptionsReadOnly _displayOptions;

        protected string _remoteName;

        private const string _localName = "Me";

        private IConversation _conversation;

        private DateTime _lastMessageDate;

        private int _lastMessageReadIndex;

        private bool _renderedEmptyMessage;

        private List<IContact> _remoteContacts;

        private Color _localSenderColor;

        private Color[] _remoteSenderColors;

        protected const string _noConversationMessage = "No messages for this contact.";

        public const int RenderAllMessages = -1;

        public ConversationRendererBase(IDisplayOptionsReadOnly displayOptions, IConversation conversation)
        {
            _displayOptions = displayOptions;

            _conversation = conversation;

            _lastMessageDate = DateTime.MinValue;

            _lastMessageReadIndex = -1;

            _renderedEmptyMessage = false;

            _remoteContacts = new List<IContact>();

            _localSenderColor = Color.FromRgb(210, 0, 0);

            _remoteSenderColors = new Color[6];
            _remoteSenderColors[0] = Color.FromRgb(0, 0, 210);
            _remoteSenderColors[1] = Color.FromRgb(114, 159, 0);
            _remoteSenderColors[2] = Color.FromRgb(186, 124, 30);
            _remoteSenderColors[3] = Color.FromRgb(0, 210, 210);
            _remoteSenderColors[4] = Color.FromRgb(143, 2, 128);
            _remoteSenderColors[5] = Color.FromRgb(52, 143, 156);
            
            ClearRenderingBuffer();
        }

        public bool HasUnprocessedMessages
        {
            get
            {
                if (_conversation == null)
                {
                    return false;
                }

                if ((_conversation.MessageCount - (_lastMessageReadIndex + 1)) <= 0)
                {
                    return false;
                }

                return true;
            }
        }

        private bool IsEmptyConversation()
        {
            if ((_conversation != null) && (_conversation.MessageCount == 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void RenderMessagesImpl(int messageCount)
        {
            if (!_renderedEmptyMessage && IsEmptyConversation())
            {
                AddEmptyConversationMessage();

                _renderedEmptyMessage = true;

                return;
            }
            else if ((_conversation == null) || (_lastMessageReadIndex + 1 >= _conversation.MessageCount))
            {
                ClearRenderingBuffer();
                return;
            }

            ClearRenderingBuffer();

            int messageEndIndex;
            if (messageCount == RenderAllMessages)
            {
                messageEndIndex = _conversation.MessageCount;
            }
            else
            {
                messageEndIndex = Math.Min(_conversation.MessageCount, (_lastMessageReadIndex + 1) + messageCount);
            }

            for (int messageIndex = _lastMessageReadIndex + 1; messageIndex < messageEndIndex; messageIndex++)
            {
                ProcessMessage(messageIndex);
            }

            if (messageEndIndex == _conversation.MessageCount)
            {
                MoveCurrentParagraphToBuffer();
            }
        }

        private void ProcessMessage(int messageIndex)
        {
            IConversationMessage message = _conversation.GetMessage(messageIndex);

            if (message.Timestamp.Date != _lastMessageDate)
            {
                StartNewParagraph();

                AddDateLine(message.Timestamp);

                _lastMessageDate = message.Timestamp.Date;
            }

            AddLineBreak();

            AddMessageLine(message);

            if (_displayOptions.LoadMmsAttachments && message.HasAttachments())
            {
                foreach (IMessageAttachment attachment in message.Attachments)
                {
                    AddLineBreak();

                    try
                    {
                        AddAttachment(attachment);
                    }
                    catch
                    {
                        // Ignore attachment errors
                    }
                }
            }

            _lastMessageReadIndex = messageIndex;
        }

        protected static string GetSenderDisplayName(IConversationMessage message)
        {
            if (message.IsOutgoing)
            {
                return _localName;
            }
            else
            {
                return message.Contact.DisplayName;
            }
        }

        protected Color GetSenderDisplayColor(IConversationMessage message)
        {
            if (message.IsOutgoing)
            {
                return _localSenderColor;
            }

            int brushIndex;

            int phoneNumberIndex = _remoteContacts.IndexOf(message.Contact);

            if (phoneNumberIndex < 0)
            {
                _remoteContacts.Add(message.Contact);
                brushIndex = (_remoteContacts.Count - 1) % _remoteSenderColors.Length;
            }
            else
            {
                brushIndex = phoneNumberIndex % _remoteSenderColors.Length;
            }

            return _remoteSenderColors[brushIndex];
        }

        protected string FormatTimeForConversation(DateTime time)
        {
            return FormatTimeForConversation(time, _displayOptions.TimeDisplayFormat);
        }

        private static string FormatTimeForConversation(DateTime time, TimeDisplayFormat displayFormat)
        {
            string formatString;

            switch (displayFormat)
            {
                case TimeDisplayFormat.HideTime:
                    return null;
                case TimeDisplayFormat.HourMin24h:
                    formatString = "{0:H:mm}";
                    break;
                case TimeDisplayFormat.HourMinAmPm:
                    formatString = "{0:h:mm tt}";
                    break;
                case TimeDisplayFormat.HourMinSec24h:
                    formatString = "{0:H:mm:ss}";
                    break;
                case TimeDisplayFormat.HourMinSecAmPm:
                    formatString = "{0:h:mm:ss tt}";
                    break;
                default:
                    throw new ArgumentException("Invalid display format type", "displayFormat");
            }

            return string.Format(formatString, time);
        }

        protected static string FormatDateForConversation(DateTime date)
        {
            return string.Format("{0:dddd, MMM d, yyyy}", date);
        }

        protected abstract void ClearRenderingBuffer();

        protected abstract void StartNewParagraph();

        protected abstract void MoveCurrentParagraphToBuffer();

        protected abstract void AddDateLine(DateTime date);

        protected abstract void AddMessageLine(IConversationMessage message);

        protected abstract void AddAttachment(IMessageAttachment messageAttachment);

        protected abstract void AddLineBreak();

        protected abstract void AddEmptyConversationMessage();

        public abstract string RenderMessagesAsString(int messageCount);

        public abstract List<System.Windows.Documents.Paragraph> RenderMessagesAsParagraphs(int messageCount);
    }
}

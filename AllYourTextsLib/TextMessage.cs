using System;
using System.Text;
using AllYourTextsLib.Framework;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace AllYourTextsLib
{
    public class TextMessage : IConversationMessage
    {
        public long MessageId { get; private set; }
        public string Address { get; private set; }
        public string Country { get; private set; }
        public bool IsOutgoing { get; private set; }
        public string MessageContents { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string ChatId { get; private set; }
        public List<IMessageAttachment> Attachments { get; private set; }
        public IContact Contact { get; set; }

        public TextMessage(long messageId, bool isOutgoing, DateTime timestamp, string messageContents, string address, string chatId, string country, IMessageAttachment messageAttachment)
        {
            MessageId = messageId;
            IsOutgoing = isOutgoing;
            Timestamp = timestamp;
            Attachments = new List<IMessageAttachment>();
            if (messageAttachment != null)
            {
                AddAttachment(messageAttachment);
            }

            MessageContents = ConvertEmptyToNull(messageContents);
            Address = ConvertEmptyToNull(address);
            ChatId = ConvertEmptyToNull(chatId);
            Country = ConvertEmptyToNull(country);
        }

        public TextMessage(long messageId, bool isOutgoing, DateTime timestamp, string messageContents, string address, string country)
            :this(messageId, isOutgoing, timestamp, messageContents, address, null, country, null)
        {
            ;
        }

        public void AddAttachment(IMessageAttachment attachment)
        {
            Attachments.Add(attachment);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("TextMessage={");

            sb.AppendFormat("MessageId={0}, ", MessageId);
            sb.AppendFormat("IsOutgoing={0}, ", IsOutgoing);
            sb.AppendFormat("Timestamp={0:HH:mm:ss MM/dd/yyyy}, ", Timestamp);
            sb.AppendFormat("Address={0}, ", Address);
            sb.AppendFormat("Country={0}, ", Country);
            sb.AppendFormat("ChatId={0}, ", ChatId);
            sb.AppendFormat("Contents={0}", MessageContents);

            sb.Append("}");

            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            TextMessage message = (TextMessage)obj;
            return Equals(message);
        }

        public bool Equals(TextMessage other)
        {
            if (this.MessageId != other.MessageId)
            {
                return false;
            }

            if (this.IsOutgoing != other.IsOutgoing)
            {
                return false;
            }

            if (this.Timestamp != other.Timestamp)
            {
                return false;
            }

            if (this.MessageContents != other.MessageContents)
            {
                return false;
            }

            if (this.Address != other.Address)
            {
                return false;
            }

            if (this.Contact != other.Contact)
            {
                return false;
            }

            if (this.ChatId != other.ChatId)
            {
                return false;
            }

            if (this.Country != other.Country)
            {
                return false;
            }

            if (!this.Attachments.SequenceEqual(other.Attachments))
            {
                return false;
            }

            return true;
        }

        private string ConvertEmptyToNull(string original)
        {
            if (string.Empty.Equals(original))
            {
                return null;
            }

            return original;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            return CompareTo((TextMessage)obj);
        }

        public int CompareTo(TextMessage other)
        {
            if (this.Timestamp != other.Timestamp)
            {
                return this.Timestamp.CompareTo(other.Timestamp);
            }
            else
            {
                return this.MessageId.CompareTo(other.MessageId);
            }
        }

        public bool HasAttachments()
        {
            return (Attachments.Count > 0);
        }
    }
}

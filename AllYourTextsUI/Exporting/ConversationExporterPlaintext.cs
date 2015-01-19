using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;
using System.IO;
using System.Windows.Documents;
using AllYourTextsUi.Framework;
using System.Text.RegularExpressions;
using AllYourTextsUi.Exporting;

namespace AllYourTextsUi
{
    public class ConversationExporterPlaintext : ConversationExporterBase
    {
        public ConversationExporterPlaintext(IFileSystem exportFileSystem)
            :base(exportFileSystem)
        {
            ;
        }

        protected override string GetExportFileExtension()
        {
            return "txt";
        }

        protected override string CreateHeader(IConversation conversation)
        {
            const string starLine = "****************************************************************************************************\r\n";
            return starLine + CreateHeaderConversationLine(conversation) + "\r\n" + CreateHeaderSoftwareLine() + "\r\n" + starLine;
        }

        protected override string CreateFooter()
        {
            return "";
        }

        private string CreateHeaderConversationLine(IConversation conversation)
        {
            if (conversation.AssociatedContacts.Count == 1)
            {
                IContact associatedContact = conversation.AssociatedContacts[0];
                const string headerLineFormat = "iPhone Text (SMS) Conversation History with {0} - {1}: {2}.";
                string name = associatedContact.DisplayName;
                string phoneNumberLabel;
                if (associatedContact.PhoneNumbers.Count == 1)
                {
                    phoneNumberLabel = "Phone Number";
                }
                else
                {
                    phoneNumberLabel = "Phone Numbers";
                }
                string phoneNumbers = GetAssociatedPhoneNumbersAsString(associatedContact);

                return string.Format(headerLineFormat, name, phoneNumberLabel, phoneNumbers);
            }
            else
            {
                StringBuilder headerBuilder = new StringBuilder();
                headerBuilder.Append("iPhone Group Text Chat History");
                headerBuilder.AppendLine();
                headerBuilder.Append("Participants:");
                headerBuilder.AppendLine();
                for (int contactIndex = 0; contactIndex < conversation.AssociatedContacts.Count; contactIndex++)
                {
                    IContact contact = conversation.AssociatedContacts[contactIndex];
                    string phoneNumberList = GetAssociatedPhoneNumbersAsString(contact);
                    headerBuilder.AppendFormat("\t{0} - {1}", contact.DisplayName, phoneNumberList);
                    if (contactIndex != conversation.AssociatedContacts.Count - 1)
                    {
                        headerBuilder.AppendLine();
                    }
                }

                return headerBuilder.ToString();
            }
        }

        protected override string GetProductUrlReference()
        {
            return ProductWebSiteInfo.Url.ToString();
        }

        protected override IConversationRenderer GetRenderer(IConversation conversation, IDisplayOptionsReadOnly displayOptions, IAttachmentExportLocator attachmentExportLocator)
        {
            return new ConversationRendererPlaintext(displayOptions, conversation);
        }

        protected override List<ExportError> ExportConversationAttachments(IConversation conversation, string attachmentExportDirectory, out IAttachmentExportLocator exportLocator)
        {
            exportLocator = new AttachmentExportLocator(null);
            return new List<ExportError>();
        }
    }
}

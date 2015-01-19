using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using AllYourTextsLib;
using AllYourTextsLib.Framework;
using AllYourTextsUi.Exporting;
using AllYourTextsUi.Framework;
using AllYourTextsUi.Properties;
using System.Drawing.Imaging;
using System.Drawing;
using System;

namespace AllYourTextsUi
{
    public class ConversationExporterHtml : ConversationExporterBase
    {
        private static readonly Regex _directoryNameRegex = new Regex("[^a-zA-Z]");

        private const string AttachmentDirectorySuffix = "_attachments";

        public const string VideoIconOutputFilename = "video_icon.png";

        public const string AudioIconOutputFilename = "audio_icon.png";

        private readonly ImageFormat IconFormat = ImageFormat.Png;

        public ConversationExporterHtml(IFileSystem exportFileSystem)
            :base(exportFileSystem)
        {
            ;
        }

        protected override string GetExportFileExtension()
        {
            return "html";
        }

        protected override string CreateHeader(IConversation conversation)
        {
            StringBuilder headerBuilder = new StringBuilder();
            headerBuilder.AppendLine("<html>");
          
            headerBuilder.AppendLine("<head>");
            headerBuilder.AppendFormat("<title>{0}</title>", CreateTitle(conversation));
            headerBuilder.AppendLine();
            headerBuilder.AppendLine("</head>");

            headerBuilder.AppendLine(CreateCssSection());
            headerBuilder.AppendLine("<body>");
            headerBuilder.AppendLine(CreateContentHeader(conversation));

            return headerBuilder.ToString();
        }

        private string CreateTitle(IConversation conversation)
        {
            string title;
            const string history = "Conversation History";
            if (conversation.AssociatedContacts.Count == 0)
            {
                title = history;
            }
            else if (conversation.AssociatedContacts.Count == 1)
            {
                string escapedName = EscapeHtml(conversation.AssociatedContacts[0].DisplayName);
                title = string.Format("{0} - {1}", escapedName, history);
            }
            else
            {
                title = string.Format("Group Chat - {0}", history);
            }

            return title;
        }

        private string CreateCssSection()
        {
            return @"<style type=""text/css"">

p.contentHeader {
    font-style: italic;
}

span.date {
    font-weight: bold;
    font-style: italic;
}

span.senderName {
    font-weight: bold;
}

img.attachmentImage {
    max-width: 800px;
    max-height: 600px;
    margin: 5px 15px;
}

img.attachmentVideo {
    margin: 5px 15px;
}

</style>";
        }

        private string CreateContentHeader(IConversation conversation)
        {
            StringBuilder contentHeaderBuilder = new StringBuilder();

            contentHeaderBuilder.AppendLine("<p class=\"contentHeader\">");
            contentHeaderBuilder.Append(CreateContentHeaderTitleLine(conversation));
            contentHeaderBuilder.AppendLine("<br />");
            contentHeaderBuilder.Append(CreateHeaderSoftwareLine());
            contentHeaderBuilder.AppendLine();
            contentHeaderBuilder.AppendLine("</p>");
            contentHeaderBuilder.Append("<hr>");

            return contentHeaderBuilder.ToString();
        }

        private string CreateContentHeaderTitleLine(IConversation conversation)
        {
            if (conversation.AssociatedContacts.Count == 1)
            {
                return CreateContentHeaderTitleLineSinglePartner(conversation);
            }
            else
            {
                return CreateContentHeaderTitleLineGroupChat(conversation);
            }
        }

        private string CreateContentHeaderTitleLineSinglePartner(IConversation conversation)
        {
            const string headerLineFormat = "iPhone Text (SMS) Conversation History with <b>{0}</b> - {1}: {2}.";
            IContact associatedContact = conversation.AssociatedContacts[0];
            string name = EscapeHtml(associatedContact.DisplayName);
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

        private string CreateContentHeaderTitleLineGroupChat(IConversation conversation)
        {
            StringBuilder headerBuilder = new StringBuilder();
            headerBuilder.Append("iPhone Group Text Chat History");
            headerBuilder.AppendLine("<br />");
            headerBuilder.Append("Participants:");
            headerBuilder.AppendLine("<br />");
            for (int contactIndex = 0; contactIndex < conversation.AssociatedContacts.Count; contactIndex++)
            {
                IContact contact = conversation.AssociatedContacts[contactIndex];
                string name = EscapeHtml(contact.DisplayName);
                string phoneNumbers = GetAssociatedPhoneNumbersAsString(contact);
                headerBuilder.AppendFormat("&nbsp;&nbsp;&nbsp;&nbsp;<b>{0}</b> - {1}", name, phoneNumbers);
                if (contactIndex != conversation.AssociatedContacts.Count - 1)
                {
                    headerBuilder.AppendLine("<br />");
                }
            }

            return headerBuilder.ToString();
        }

        protected override string GetProductUrlReference()
        {
            return string.Format("<a href=\"{0}\">{1}</a>", ProductWebSiteInfo.Url.ToString(), ProductWebSiteInfo.DisplayUrl);
        }

        protected override string CreateFooter()
        {
            return "</body>\r\n</html>\r\n";
        }

        protected override IConversationRenderer GetRenderer(IConversation conversation, IDisplayOptionsReadOnly displayOptions, IAttachmentExportLocator attachmentExportLocator)
        {
            return new ConversationRendererHtml(displayOptions, conversation, attachmentExportLocator);
        }

        protected override List<ExportError> ExportConversationAttachments(IConversation conversation, string exportFilename, out IAttachmentExportLocator exportLocator)
        {
            List<ExportError> exportErrors = new List<ExportError>();
            string exportBasePath = Path.GetDirectoryName(exportFilename);
            AttachmentExportLocator attachmentExportLocator = new AttachmentExportLocator(exportBasePath);
            string attachmentDirectoryPath = null;
            bool exportedVideoAttachment = false;
            bool exportedAudioAttachment = false;

            foreach (IConversationMessage message in conversation)
            {
                if (!message.HasAttachments())
                {
                    continue;
                }

                if (attachmentDirectoryPath == null)
                {
                    string attachmentDirectoryName = GenerateAttachmentExportDirectoryName(exportFilename);
                    attachmentDirectoryPath = Path.Combine(exportBasePath, attachmentDirectoryName);
                    attachmentDirectoryPath = FileCreator.CreateNewDirectoryWithRenameAttempts(attachmentDirectoryPath, _exportFileSystem, MaxRenameAttempts);
                }

                foreach (IMessageAttachment attachment in message.Attachments)
                {
                    try
                    {
                        string exportedPath = ExportMessageAttachment(attachment, attachmentDirectoryPath);
                        attachmentExportLocator.AddAttachmentExportLocation(attachment, exportedPath);

                        if (attachment.Type == AttachmentType.Video)
                        {
                            exportedVideoAttachment = true;
                        }
                        else if (attachment.Type == AttachmentType.Audio)
                        {
                            exportedAudioAttachment = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        exportErrors.Add(new ExportError(conversation, ex));
                    }
                }
            }

            if (exportedVideoAttachment)
            {
                string videoIconPath = PlaceVideoIconFile(attachmentDirectoryPath);
                attachmentExportLocator.AddFileExportLocation(VideoIconOutputFilename, videoIconPath);
            }
            if (exportedAudioAttachment)
            {
                string audioIconPath = PlaceAudioIconFile(attachmentDirectoryPath);
                attachmentExportLocator.AddFileExportLocation(AudioIconOutputFilename, audioIconPath);
            }

            exportLocator = attachmentExportLocator;
            return exportErrors;
        }

        private string PlaceVideoIconFile(string exportDirectory)
        {
            return PlaceIconFile(exportDirectory, VideoIconOutputFilename, Resources.video_icon);
        }

        private string PlaceAudioIconFile(string exportDirectory)
        {
            return PlaceIconFile(exportDirectory, AudioIconOutputFilename, Resources.audio_icon);
        }

        private string PlaceIconFile(string exportDirectory, string outputFilename, Bitmap icon)
        {
            string iconPath = Path.Combine(exportDirectory, outputFilename);
            string finalIconPath;
            using (Stream iconDestinationStream = FileCreator.CreateNewFileWithRenameAttempts(iconPath, _exportFileSystem, MaxRenameAttempts, out finalIconPath))
            {
                icon.Save(iconDestinationStream, IconFormat);
            }

            return finalIconPath;
        }

        private string ExportMessageAttachment(IMessageAttachment attachment, string attachmentExportDirectory)
        {
            string attachmentExportPath = Path.Combine(attachmentExportDirectory, attachment.OriginalFilename);
            string createdAttachmentExportPath;
            using (Stream attachmentOutputStream = FileCreator.CreateNewFileWithRenameAttempts(attachmentExportPath, _exportFileSystem, MaxRenameAttempts, out createdAttachmentExportPath))
            {
                _exportFileSystem.CopyFile(attachment.Path, attachmentOutputStream);
            }
            return createdAttachmentExportPath;
        }

        private static string EscapeHtml(string unescaped)
        {
            return System.Security.SecurityElement.Escape(unescaped);
        }

        private string GenerateAttachmentExportDirectoryName(string exportFilename)
        {
            return Path.GetFileNameWithoutExtension(exportFilename) + "_attachments";
        }
    }
}

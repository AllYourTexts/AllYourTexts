using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using AllYourTextsUi.Framework;
using AllYourTextsLib.Framework;
using AllYourTextsUi.Exporting;
using AllYourTextsLib;

namespace AllYourTextsUi
{
    public abstract class ConversationExporterBase : IConversationExporter
    {
        protected const int MaxRenameAttempts = 100;

        protected IFileSystem _exportFileSystem;

        protected abstract string GetExportFileExtension();

        protected abstract string CreateHeader(IConversation conversation);

        protected abstract string CreateFooter();

        protected abstract IConversationRenderer GetRenderer(IConversation conversation, IDisplayOptionsReadOnly displayOptions, IAttachmentExportLocator attachmentExportLocator);

        public ConversationExporterBase(IFileSystem exportFileSystem)
        {
            _exportFileSystem = exportFileSystem;
        }

        protected string CreateHeaderSoftwareLine()
        {
            const string headerFormat = "Exported by AllYourTexts v.{0} ({1}) on {2}.";
            string versionNumber = ProductInfoIndirect.Version;
            string url = GetProductUrlReference();
            string timestamp = DateTime.Now.ToString("M/d/yyyy, h:mm:ss tt");

            string softwareLine = string.Format(headerFormat, versionNumber, url, timestamp);

            return softwareLine;
        }

        protected abstract string GetProductUrlReference();

        protected string GetAssociatedPhoneNumbersAsString(IContact contact)
        {
            List<string> phoneNumberValues = new List<string>();
            foreach (IPhoneNumber phoneNumber in contact.PhoneNumbers)
            {
                string formattedNumber = PhoneNumberFormatter.FormatNumberWithDashes(phoneNumber);
                phoneNumberValues.Add(formattedNumber);
            }

            string phoneNumbers = string.Join(", ", phoneNumberValues);

            return phoneNumbers;
        }

        public List<ExportError> Export(IConversation conversation, IDisplayOptionsReadOnly displayOptions, string exportFilename)
        {
            IAttachmentExportLocator exportLocator;
            List<ExportError> attachmentExportErrors = DoAttachmentExport(conversation, displayOptions, exportFilename, out exportLocator);
            List<ExportError> conversationTextExportErrors = DoConversationTextExport(conversation, displayOptions, exportFilename, exportLocator);

            List<ExportError> exportErrors = new List<ExportError>();
            exportErrors.AddRange(attachmentExportErrors);
            exportErrors.AddRange(conversationTextExportErrors);
            return exportErrors;
        }

        private List<ExportError> DoAttachmentExport(IConversation conversation, IDisplayOptionsReadOnly displayOptions, string exportFilename, out IAttachmentExportLocator exportLocator)
        {
            List<ExportError> exportErrors;
            if (displayOptions.LoadMmsAttachments)
            {
                exportErrors = ExportConversationAttachments(conversation, exportFilename, out exportLocator);
            }
            else
            {
                exportErrors = new List<ExportError>();
                exportLocator = new AttachmentExportLocator(null);
            }

            return exportErrors;
        }

        private List<ExportError> DoConversationTextExport(IConversation conversation, IDisplayOptionsReadOnly displayOptions, string exportFilename, IAttachmentExportLocator exportLocator)
        {
            List<ExportError> exportErrors = new List<ExportError>();
            string finalExportFilename;
            try
            {
                using (Stream exportFileStream = FileCreator.CreateNewFileWithRenameAttempts(exportFilename, _exportFileSystem, MaxRenameAttempts, out finalExportFilename))
                {
                    using (StreamWriter exportStreamWriter = new StreamWriter(exportFileStream, Encoding.UTF8))
                    {
                        WriteHeader(exportStreamWriter, conversation);
                        WriteConversationContents(exportStreamWriter, conversation, displayOptions, exportLocator);
                        WriteFooter(exportStreamWriter);
                    }
                }
            }
            catch (Exception ex)
            {
                exportErrors.Add(new ExportError(conversation, ex));
            }

            return exportErrors;
        }

        private void WriteHeader(StreamWriter writer, IConversation conversation)
        {
            writer.Write(CreateHeader(conversation));
            writer.WriteLine();
        }

        protected void WriteConversationContents(StreamWriter writer, IConversation conversation, IDisplayOptionsReadOnly displayOptions, IAttachmentExportLocator attachmentExportLocator)
        {
            IConversationRenderer renderer = GetRenderer(conversation, displayOptions, attachmentExportLocator);

            while (renderer.HasUnprocessedMessages)
            {
                const int RenderBufferSize = 5000;
                string exportableString = renderer.RenderMessagesAsString(RenderBufferSize);
                writer.Write(exportableString);
            }
        }

        private void WriteFooter(StreamWriter writer)
        {
            writer.Write(CreateFooter());
        }

        public List<ExportError> ExportMultipleConversations(IEnumerable<IConversation> conversations, IDisplayOptionsReadOnly displayOptions, string exportPath, IProgressCallback progressCallback)
        {
            List<ExportError> exportErrors = new List<ExportError>();
            string createdFolderPath = FileCreator.CreateNewDirectoryWithRenameAttempts(exportPath, _exportFileSystem, MaxRenameAttempts);

            ExportedFilenameGenerator filenameGenerator = new ExportedFilenameGenerator();

            foreach (IConversation conversation in conversations)
            {
                CheckForCancel(progressCallback);

                //
                // Don't export empty conversations
                //

                if (conversation.MessageCount == 0)
                {
                    IncrementProgress(progressCallback);
                    continue;
                }

                try
                {
                    string filename = filenameGenerator.CreateExportFilenameSuggestion(conversation) + "." + GetExportFileExtension();
                    string exportFilename = Path.Combine(createdFolderPath, filename);

                    List<ExportError> currentErrors = Export(conversation, displayOptions, exportFilename);
                    exportErrors.AddRange(currentErrors);
                }
                catch (Exception ex)
                {
                    exportErrors.Add(new ExportError(conversation, ex));
                }

                IncrementProgress(progressCallback);
            }

            return exportErrors;
        }

        private void CheckForCancel(IProgressCallback progressCallback)
        {
            if (progressCallback == null)
            {
                return;
            }

            if (progressCallback.IsCancelRequested)
            {
                progressCallback.AcceptCancel();
                throw new OperationCanceledException();
            }
        }

        private void IncrementProgress(IProgressCallback progressCallback)
        {
            if (progressCallback != null)
            {
                progressCallback.Increment(1);
            }
        }

        protected abstract List<ExportError> ExportConversationAttachments(IConversation conversation, string exportFilename, out IAttachmentExportLocator exportLocator);
    }
}

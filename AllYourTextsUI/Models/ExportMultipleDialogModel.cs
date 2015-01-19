using AllYourTextsLib.Framework;
using AllYourTextsUi.Exporting;
using AllYourTextsUi.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsUi.Models
{
    public class ExportMultipleDialogModel
    {

        public enum ExportFormat
        {
            Unknown = 0,
            Html,
            Plaintext
        }

        public int ConversationCount
        {
            get
            {
                return _conversationManager.ConversationCount;
            }
        }

        public bool ExportSucceeded
        {
            get
            {
                return (_exportErrors.Count == 0);
            }
        }

        public string ErrorMessage
        {
            get
            {
                List<string> errorMessages = new List<string>();
                foreach (ExportError exportError in _exportErrors)
                {
                    errorMessages.Add(_exportErrorFormatter.Format(exportError));
                }

                return string.Join("\r\n", errorMessages);
            }
        }

        public ExportMultipleDialogModel(IConversationManager conversationManager, IDisplayOptionsReadOnly displayOptions, IFileSystem exportFileSystem,
            ExportErrorFormatter exportErrorFormatter)
        {
            _conversationManager = conversationManager;
            _displayOptions = displayOptions;
            _exportFileSystem = exportFileSystem;
            _exportErrorFormatter = exportErrorFormatter;
        }

        public void ExportConversations(ExportFormat exportFormat, string exportPath, IProgressCallback progressCallback)
        {
            IConversationExporter conversationExporter;
            switch (exportFormat)
            {
                case ExportFormat.Html:
                    conversationExporter = new ConversationExporterHtml(_exportFileSystem);
                    break;
                case ExportFormat.Plaintext:
                    conversationExporter = new ConversationExporterPlaintext(_exportFileSystem);
                    break;
                default:
                    throw new ArgumentException("Unrecognized export format.");
            }

            _exportErrors = conversationExporter.ExportMultipleConversations(_conversationManager, _displayOptions, exportPath, progressCallback);
        }

        private IConversationManager _conversationManager;

        private IDisplayOptionsReadOnly _displayOptions;

        private IFileSystem _exportFileSystem;

        private ExportErrorFormatter _exportErrorFormatter;

        private List<ExportError> _exportErrors;
    }
}

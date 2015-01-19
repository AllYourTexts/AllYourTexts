using AllYourTextsLib;
using AllYourTextsLib.Framework;
using AllYourTextsUi.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AllYourTextsUi.Exporting
{
    public class ExportedFilenameGenerator
    {
        public ExportedFilenameGenerator()
            :this(new ConversationDescriptionHelper(), new Clock())
        {
            ;
        }
        public ExportedFilenameGenerator(IConversationDescriptionHelper descriptionHelper, IClock clock)
        {
            _descriptionHelper = descriptionHelper;
            _clock = clock;
        }

        public string CreateExportFilenameSuggestion(IConversation conversation)
        {
            string filenameFormat = "{0} - {1} - {2}";

            IContact contact = conversation.AssociatedContacts[0];

            string description = _descriptionHelper.GetDescription(conversation);

            string suggestedFilename = string.Format(filenameFormat, FilenamePrefix, description, GetCurrentDatestamp());

            return ReplaceIllegalFilenameCharacters(suggestedFilename);
        }

        public string CreateExportFolderNameSuggestion()
        {
            string filenameFormat = "{0} Backup - {1}";

            return string.Format(filenameFormat, FilenamePrefix, GetCurrentDatestamp());
        }

        private string ReplaceIllegalFilenameCharacters(string originalFilename)
        {
            string invalidPathCharactersPattern = "[" + Regex.Escape(new string(Path.GetInvalidFileNameChars())) + "]";
            return Regex.Replace(originalFilename, invalidPathCharactersPattern, "_");
        }

        private string GetCurrentDatestamp()
        {
            return _clock.CurrentTime().ToString("yyyy-MM-dd");
        }

        private const string FilenamePrefix = "iPhone Text History";

        private IConversationDescriptionHelper _descriptionHelper;
        private IClock _clock;
    }
}

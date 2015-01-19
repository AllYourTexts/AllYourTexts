using AllYourTextsLib.Framework;
using AllYourTextsUi.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsUi.Exporting
{
    public class ExportErrorFormatter
    {
        public ExportErrorFormatter()
            :this(new ConversationDescriptionHelper())
        {
            ;
        }
        public ExportErrorFormatter(IConversationDescriptionHelper descriptionHelper)
        {
            _descriptionHelper = descriptionHelper;
        }

        public string Format(ExportError exportError)
        {
            string errorHeader = CreateErrorMessagePrefix(exportError.Conversation);
            return string.Format("{0}: ({1}) {2}", errorHeader, exportError.Error.GetType().ToString(), exportError.Error.Message);
        }

        private string CreateErrorMessagePrefix(IConversation conversation)
        {
            return string.Format("Error in {0}", _descriptionHelper.GetDescription(conversation));
        }

        private IConversationDescriptionHelper _descriptionHelper;
    }
}

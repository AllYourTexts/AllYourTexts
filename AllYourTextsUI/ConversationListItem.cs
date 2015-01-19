using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsUi.Framework;
using AllYourTextsLib.Framework;

namespace AllYourTextsUi
{
    public class ConversationListItem : IConversationListItem
    {
        public string Description { get; private set; }

        public IConversation Conversation { get; private set; }

        private IConversationDescriptionHelper _descriptionHelper;

        public ConversationListItem(IConversation conversation)
        {
            _descriptionHelper = new ConversationDescriptionHelper();

            this.Description = GetDescription(conversation);

            this.Conversation = conversation;
        }

        public string GetDescription(IConversation conversation)
        {
            return _descriptionHelper.GetDescription(conversation);
        }
    }
}

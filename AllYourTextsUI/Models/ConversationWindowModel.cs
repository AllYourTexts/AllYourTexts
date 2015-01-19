using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsUi.Framework;
using AllYourTextsLib.Framework;

namespace AllYourTextsUi
{
    public class ConversationWindowModel : MainWindowModelBase, IConversationWindowModel
    {
        public ConversationWindowModel(IDisplayOptions displayOptions, IPhoneSelectOptions phoneSelectOptions)
            :base(displayOptions, phoneSelectOptions)
        {
            _selectedConversationIndex = NoContactSelectedIndex;
        }

        public override IConversation SelectedConversation
        {
            get
            {
                if (_selectedConversationIndex == NoContactSelectedIndex)
                {
                    return DefaultConversation;
                }
                else
                {
                    return ConversationManager.GetConversation(_selectedConversationIndex);
                }
            }
            set
            {
                int conversationIndex = ConversationManager.FindConversationIndex(value);
                if (conversationIndex == -1)
                {
                    _selectedConversationIndex = NoContactSelectedIndex;
                }
                else
                {
                    _selectedConversationIndex = conversationIndex;
                }
            }
        }

        protected override IConversation DefaultConversation
        {
            get
            {
                return null;
            }
        }
    }
}

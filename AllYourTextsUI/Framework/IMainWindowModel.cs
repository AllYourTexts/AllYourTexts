using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;

namespace AllYourTextsUi.Framework
{
    public interface IMainWindowModel
    {
        IDisplayOptions DisplayOptions { get; }

        IPhoneSelectOptions PhoneSelectOptions { get; }

        IEnumerable<IConversationListItem> ConversationListItems { get; }

        IConversation SelectedConversation { get; set; }

        IConversation PreviousConversation(IConversation conversation);

        IConversation NextConversation(IConversation conversation);

        IConversationStatistics ConversationStatistics { get; }

        IConversationManager ConversationManager { get; set; }

        IPhoneDeviceInfo DeviceInfo { get; set; }
    }
}

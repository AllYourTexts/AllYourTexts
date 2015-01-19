using AllYourTextsLib.Framework;
using System;

namespace AllYourTextsUi.Framework
{
    public interface IConversationDescriptionHelper
    {
        string GetDescription(IConversation conversation);
    }
}

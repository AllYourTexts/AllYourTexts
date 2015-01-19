using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;
using System.Threading;

namespace AllYourTextsUi.Framework
{
    public interface IConversationSearchTarget
    {
        IConversation PreviousConversation(IConversation conversation);

        IConversation NextConversation(IConversation conversation);

        IConversation CurrentConversation { get; set; }

        object SearchTargetControl { get; }

        void PrepareWaitForRenderComplete();

        void WaitForRenderComplete();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;
using AllYourTextsUi.Framework;
using System.Windows.Documents;
using System.Windows.Controls;

namespace AllYourTextsTest
{
    public class MockConversationSearchTarget : IConversationSearchTarget
    {

        public IConversation PreviousConversation(IConversation conversation)
        {
            throw new NotImplementedException();
        }

        public IConversation NextConversation(IConversation conversation)
        {
            throw new NotImplementedException();
        }

        public IConversation CurrentConversation
        {
            get { throw new NotImplementedException(); }
        }

        public object SearchTargetControl { get; set; }

        IConversation IConversationSearchTarget.CurrentConversation
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void PrepareWaitForRenderComplete()
        {
            throw new NotImplementedException();
        }

        public void WaitForRenderComplete()
        {
            throw new NotImplementedException();
        }

        public int MatchIndex
        {
            get
            {
                if (SearchTargetControl == null)
                {
                    return -1;
                }

                RichTextBox rtb = (RichTextBox)SearchTargetControl;

                if (rtb.Selection.IsEmpty)
                {
                    return -1;
                }

                return (new TextRange(rtb.Document.ContentStart, rtb.Selection.Start)).Text.Length;
            }
        }
    }
}

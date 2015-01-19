using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace AllYourTextsUi.ConversationRendering
{
    public class ConversationContentRun : Run
    {
        private string _contents;

        public ConversationContentRun(string contents)
            : base(contents)
        {
            _contents = contents;
        }

        public override string ToString()
        {
            return "ConversationContentRun=" + _contents;
        }
    }
}

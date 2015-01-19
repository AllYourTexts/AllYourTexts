using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsUi.Framework
{
    public enum OptionsCategory
    {
        Unknown,
        General,
        ConversationView
    }

    public interface IOptionsDialogModel
    {
        OptionsCategory SelectedOptionsCategory { get; set; }

        bool MergeContacts { get; set; }

        bool HideEmptyContacts { get; set; }

        TimeDisplayFormat TimeDisplay { get; set; }

        void CommitOptions();
    }
}

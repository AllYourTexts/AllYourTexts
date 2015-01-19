using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsUi.Framework
{

    public delegate void PhoneSelectOptionsPropertyChangedEventHandler(object sender, EventArgs e);

    public interface IPhoneSelectOptionsReadOnly
    {
        bool PromptForPhoneChoice { get; }

        string PhoneDataPath { get; }

        bool WarnAboutMoreRecentSync { get; }

        event PhoneSelectOptionsPropertyChangedEventHandler PhoneDataPathPropertyChanged;
    }

    public interface IPhoneSelectOptions : IPhoneSelectOptionsReadOnly
    {
        new bool PromptForPhoneChoice { get; set; }

        new string PhoneDataPath { get;  set; }

        new bool WarnAboutMoreRecentSync { get; set; }

        void Save();
    }
}

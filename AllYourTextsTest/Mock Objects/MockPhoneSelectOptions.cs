using AllYourTextsUi.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsTest.Mock_Objects
{
    public class MockPhoneSelectOptions : IPhoneSelectOptions
    {
        public bool PromptForPhoneChoice { get; set; }

        public string PhoneDataPath { get; set; }

        public bool WarnAboutMoreRecentSync { get; set; }

        public void Save()
        {
            ;
        }

#pragma warning disable 0067
        public event PhoneSelectOptionsPropertyChangedEventHandler PhoneDataPathPropertyChanged;
#pragma warning restore 0067
    }
}

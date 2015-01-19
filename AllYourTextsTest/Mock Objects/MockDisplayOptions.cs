using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsUi.Framework;

namespace AllYourTextsTest.Mock_Objects
{
    public class MockDisplayOptions : IDisplayOptions
    {

#pragma warning disable 0067
        public event DisplayOptionsPropertyChangedEventHandler HideEmptyConversationsPropertyChanged;

        public event DisplayOptionsPropertyChangedEventHandler MergeContactsPropertyChanged;

        public event DisplayOptionsPropertyChangedEventHandler LoadMmsAttachmentsPropertyChanged;

        public event DisplayOptionsPropertyChangedEventHandler TimeDisplayFormatPropertyChanged;

        public event DisplayOptionsPropertyChangedEventHandler ConversationSortingPropertyChanged;
#pragma warning restore 0067

        public const bool HideEmptyConversationsDefault = true;

        public const bool MergeContactsDefault = false;

        public const bool LoadMmsAttachmentsDefault = true;

        public const TimeDisplayFormat TimeDisplayFormatDefault = TimeDisplayFormat.HourMinSecAmPm;

        public const ConversationSorting ConversationSortingDefault = ConversationSorting.AlphabeticalByContact;

        public bool HideEmptyConversations { get; set; }

        public bool MergeContacts { get; set; }

        public bool LoadMmsAttachments { get; set; }

        public TimeDisplayFormat TimeDisplayFormat { get; set; }

        public ConversationSorting ConversationSorting { get; set; }

        public bool PromptForSyncTroubleshooting { get; set; }

        public MockDisplayOptions()
        {
            HideEmptyConversations = HideEmptyConversationsDefault;

            MergeContacts = MergeContactsDefault;

            LoadMmsAttachments = LoadMmsAttachmentsDefault;

            TimeDisplayFormat = TimeDisplayFormatDefault;

            ConversationSorting = ConversationSortingDefault;
        }
        public void Save()
        {
            ;
        }
    }
}

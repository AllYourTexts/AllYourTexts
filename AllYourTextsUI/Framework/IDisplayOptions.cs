using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsUi.Framework
{

    public enum TimeDisplayFormat
    {
        Unknown = 0,
        HideTime,
        HourMinSecAmPm,
        HourMinAmPm,
        HourMinSec24h,
        HourMin24h
    }

    public enum ConversationSorting
    {
        Unknown = 0,
        AlphabeticalByContact,
        DescendingByTotalMessages
    }

    public delegate void DisplayOptionsPropertyChangedEventHandler(object sender, EventArgs e);

    public interface IDisplayOptionsReadOnly
    {
        bool HideEmptyConversations { get; }

        bool MergeContacts { get; }

        bool LoadMmsAttachments { get; }

        bool PromptForSyncTroubleshooting { get; }

        TimeDisplayFormat TimeDisplayFormat { get; }

        ConversationSorting ConversationSorting { get; }

        event DisplayOptionsPropertyChangedEventHandler HideEmptyConversationsPropertyChanged;

        event DisplayOptionsPropertyChangedEventHandler MergeContactsPropertyChanged;

        event DisplayOptionsPropertyChangedEventHandler LoadMmsAttachmentsPropertyChanged;

        event DisplayOptionsPropertyChangedEventHandler TimeDisplayFormatPropertyChanged;

        event DisplayOptionsPropertyChangedEventHandler ConversationSortingPropertyChanged;
    }

    public interface IDisplayOptions : IDisplayOptionsReadOnly
    {
        new bool HideEmptyConversations { get; set; }

        new bool MergeContacts { get; set; }

        new bool LoadMmsAttachments { get; set; }

        new bool PromptForSyncTroubleshooting { get; set; }

        new TimeDisplayFormat TimeDisplayFormat { get; set; }

        new ConversationSorting ConversationSorting { get; set; }

        void Save();
    }
}

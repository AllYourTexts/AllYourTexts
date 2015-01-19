using System;
using System.ComponentModel;
using System.Configuration;
using AllYourTextsUi.Framework;

namespace AllYourTextsUi
{

    internal sealed partial class DisplayOptions : IDisplayOptions, IPhoneSelectOptions
    {
        public event DisplayOptionsPropertyChangedEventHandler HideEmptyConversationsPropertyChanged;

        public event DisplayOptionsPropertyChangedEventHandler MergeContactsPropertyChanged;

        public event DisplayOptionsPropertyChangedEventHandler LoadMmsAttachmentsPropertyChanged;

        public event DisplayOptionsPropertyChangedEventHandler TimeDisplayFormatPropertyChanged;

        public event DisplayOptionsPropertyChangedEventHandler ConversationSortingPropertyChanged;

        public event PhoneSelectOptionsPropertyChangedEventHandler PhoneDataPathPropertyChanged;

        private string SettingChanged;

        public DisplayOptions()
        {
            SettingChanged = null;
            this.SettingChanging += this.SettingChangingEventHandler;
            this.PropertyChanged += this.PropertyChangedEventHandler;
        }

        private void SettingChangingEventHandler(object sender, SettingChangingEventArgs e)
        {
            SettingChanged = e.SettingName;
        }

        private void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e)
        {
            if (SettingChanged != e.PropertyName)
            {
                SettingChanged = null;
                return;
            }
            switch (e.PropertyName)
            {
                case "HideEmptyConversations":
                    if (HideEmptyConversationsPropertyChanged != null)
                    {
                        HideEmptyConversationsPropertyChanged(this, EventArgs.Empty);
                    }
                    break;
                case "MergeContacts":
                    if (MergeContactsPropertyChanged != null)
                    {
                        MergeContactsPropertyChanged(this, EventArgs.Empty);
                    }
                    break;
                case "LoadMmsAttachments":
                    if (LoadMmsAttachmentsPropertyChanged != null)
                    {
                        LoadMmsAttachmentsPropertyChanged(this, EventArgs.Empty);
                    }
                    break;
                case "TimeDisplayFormat":
                    if (TimeDisplayFormatPropertyChanged != null)
                    {
                        TimeDisplayFormatPropertyChanged(this, EventArgs.Empty);
                    }
                    break;
                case "ConversationSorting":
                    if (ConversationSortingPropertyChanged != null)
                    {
                        ConversationSortingPropertyChanged(this, EventArgs.Empty);
                    }
                    break;
                case "PhoneDataPath":
                    if (PhoneDataPathPropertyChanged != null)
                    {
                        PhoneDataPathPropertyChanged(this, EventArgs.Empty);
                    }
                    break;
                case "PromptForPhoneChoice":
                    break;
                case "PromptForSyncTroubleshooting":
                    break;
                case "WarnAboutMoreRecentSync":
                    break;
                default:
                    throw new ArgumentException("Unrecognized option property type.");
            }
        }
    }
}

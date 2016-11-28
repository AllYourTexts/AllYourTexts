using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib;
using AllYourTextsLib.Conversation;
using AllYourTextsUi.Framework;
using AllYourTextsLib.Framework;
using AllYourTextsLib.DataReader;

namespace AllYourTextsUi
{
    public abstract class MainWindowModelBase : IMainWindowModel
    {
        private IConversationManager _conversationManager;

        private List<IConversation> _sortedConversationList;

        protected int _selectedConversationIndex;

        protected const int NoContactSelectedIndex = -1;

        public IDisplayOptions DisplayOptions { get; private set; }

        public IPhoneSelectOptions PhoneSelectOptions { get; private set; }

        public IPhoneDeviceInfo DeviceInfo { get; set; }

        public MainWindowModelBase(IDisplayOptions displayOptions, IPhoneSelectOptions phoneSelectOptions)
        {
            DisplayOptions = displayOptions;

            PhoneSelectOptions = phoneSelectOptions;

            DeviceInfo = null;

            ConversationManager = null;

            _sortedConversationList = null;
        }

        public abstract IConversation SelectedConversation
        {
            get;

            set;
        }

        public IConversation PreviousConversation(IConversation conversation)
        {
            int itemCount = _sortedConversationList.Count;
            if (itemCount == 0)
            {
                return null;
            }

            if (conversation == null)
            {
                return _sortedConversationList[_sortedConversationList.Count - 1];
            }

            int conversationIndex = _sortedConversationList.IndexOf(conversation);
            if (conversationIndex == -1)
            {
                return null;
            }

            int previousConversationIndex;
            if (conversationIndex == 0)
            {
                previousConversationIndex = itemCount - 1;
            }
            else
            {
                previousConversationIndex = conversationIndex - 1;
            }

            return _sortedConversationList[previousConversationIndex];
        }

        public IConversation NextConversation(IConversation conversation)
        {
            int itemCount = _sortedConversationList.Count;
            if (itemCount == 0)
            {
                return null;
            }

            if (conversation == null)
            {
                return _sortedConversationList[0];
            }

            int conversationIndex = _sortedConversationList.IndexOf(conversation);
            if (conversationIndex == -1)
            {
                return null;
            }

            int nextConversationIndex = ((conversationIndex + 1) % itemCount);

            return _sortedConversationList[nextConversationIndex];
        }

        protected abstract IConversation DefaultConversation
        {
            get;
        }

        public IConversationStatistics ConversationStatistics
        {
            get
            {
                return ConversationStatisticsGenerator.CalculateStatistics(SelectedConversation);
            }
        }

        public virtual IConversationManager ConversationManager
        {
            get
            {
                return _conversationManager;
            }
            set
            {
                _conversationManager = value;
                UpdateConversationList();
            }
        }

        public static IConversationManager LoadConversationManager(ILoadingProgressCallback progressCallback, IPhoneDeviceInfo deviceInfo, bool mergeConversations)
        {
            IConversationManager conversationManager = null;

            using (DatabaseReader contactDatabaseReader = new DatabaseReader(DatabaseFinder.FindContactDatabasePath(deviceInfo)))
            {
                ContactReader contactReader = new ContactReader();
                contactReader.ParseDatabase(contactDatabaseReader);

                using (DatabaseReader textDatabaseReader = new DatabaseReader(DatabaseFinder.FindTextMessageDatabasePath(deviceInfo)))
                {
                    TextMessageReaderBase textMessageReader;
                    IEnumerable<ChatRoomInformation> chatReader;
                    IEnumerable<MessageAttachment> attachments;
                    int chatWorkEstimate;

                    if ((deviceInfo.OsVersion == null) || (deviceInfo.OsVersion.MajorVersion >= 6))
                    {
                        try
                        { 
                            textMessageReader = new TextMessageReaderiOS6(deviceInfo.BackupPath);
                            textMessageReader.ParseDatabase(textDatabaseReader);
                            ChatRoomInformationReaderiOS6 chatRoomInfoReader = new ChatRoomInformationReaderiOS6();
                            chatRoomInfoReader.ParseDatabase(textDatabaseReader);
                            chatReader = chatRoomInfoReader;
                            attachments = new List<MessageAttachment>();
                            chatWorkEstimate = chatRoomInfoReader.ItemCountEstimate;
                        }
                        catch (DatabaseQueryException)
                        {
                            textMessageReader = new TextMessageReader(deviceInfo.BackupPath);
                            textMessageReader.ParseDatabase(textDatabaseReader);
                            chatReader = new List<ChatRoomInformation>();
                            attachments = new List<MessageAttachment>();
                            chatWorkEstimate = 0;
                        }
                    }
                    else if (deviceInfo.OsVersion.MajorVersion == 5)
                    {
                        textMessageReader = new TextMessageReader2(deviceInfo.BackupPath);
                        textMessageReader.ParseDatabase(textDatabaseReader);
                        ChatRoomInformationReader chatRoomInfoReader = new ChatRoomInformationReader();
                        chatRoomInfoReader.ParseDatabase(textDatabaseReader);
                        chatReader = chatRoomInfoReader;
                        attachments = new List<MessageAttachment>();
                        chatWorkEstimate = chatRoomInfoReader.ItemCountEstimate;
                    }
                    else
                    {
                        textMessageReader = new TextMessageReader(deviceInfo.BackupPath);
                        textMessageReader.ParseDatabase(textDatabaseReader);
                        chatReader = new List<ChatRoomInformation>();
                        attachments = new List<MessageAttachment>();
                        chatWorkEstimate = 0;
                    }

                    int workEstimate = AllYourTextsLib.Conversation.ConversationManager.GetWorkEstimate(contactReader.ItemCountEstimate,
                                                                                                        textMessageReader.ItemCountEstimate,
                                                                                                        chatWorkEstimate,
                                                                                                        0);
                    if (mergeConversations)
                    {
                        workEstimate += MergingConversationManager.GetWorkEstimateByContacts(contactReader.ItemCountEstimate);
                    }
                    progressCallback.Begin(workEstimate);

                    conversationManager = new ConversationManager(contactReader,
                                                                  textMessageReader,
                                                                  chatReader,
                                                                  attachments,
                                                                  progressCallback);
                    if (mergeConversations)
                    {
                        conversationManager = new MergingConversationManager(conversationManager, progressCallback);
                    }
                }
            }

            return conversationManager;
        }

        private void UpdateConversationList()
        {
            if (ConversationManager == null)
            {
                _sortedConversationList = null;
                return;
            }

            _sortedConversationList = new List<IConversation>(ConversationManager.ConversationCount);

            foreach (IConversation conversation in ConversationManager)
            {
                if (DisplayOptions.HideEmptyConversations && (conversation.MessageCount == 0))
                {
                    continue;
                }
                _sortedConversationList.Add(conversation);
            }

            _sortedConversationList.Sort(CompareConversations);
        }

        public virtual IEnumerable<IConversationListItem> ConversationListItems
        {
            get
            {
                UpdateConversationList();
                return GetConversationListItems();
            }
        }

        private List<IConversationListItem> GetConversationListItems()
        {
            if (_sortedConversationList == null)
            {
                return new List<IConversationListItem>();
            }

            List<IConversationListItem> listItems = new List<IConversationListItem>(_sortedConversationList.Count);
            foreach (IConversation conversation in _sortedConversationList)
            {
                listItems.Add(new ConversationListItem(conversation));
            }

            return listItems;
        }

        private int CompareConversations(IConversation conversationA, IConversation conversationB)
        {
            Comparison<IConversation> sortMechanism;

            switch (DisplayOptions.ConversationSorting)
            {
                case ConversationSorting.AlphabeticalByContact:
                    sortMechanism = ConversationComparer.AlphabeticalByContact;
                    break;
                case ConversationSorting.DescendingByTotalMessages:
                    sortMechanism = ConversationComparer.DescendingByTotalMessages;
                    break;
                default:
                    throw new ArgumentException("Unrecognized sorting mechanism.");
            }

            return sortMechanism(conversationA, conversationB);
        }
    }
}

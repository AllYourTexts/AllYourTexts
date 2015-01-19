using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AllYourTextsUi.Framework;
using AllYourTextsLib.Framework;
using AllYourTextsLib;
using AllYourTextsLib.DataReader;

namespace AllYourTextsUi
{
    public class TimeDisplayFormatListItem
    {
        public string Description { get; set; }

        public TimeDisplayFormat Format { get; set; }

        public TimeDisplayFormatListItem(string description, TimeDisplayFormat format)
        {
            this.Description = description;
            this.Format = format;
        }

        public TimeDisplayFormatListItem()
            : this(null, TimeDisplayFormat.Unknown)
        {
            ;
        }
    }

    public class ConversationSortingListItem
    {
        public string Description { get; set; }
        public ConversationSorting Sorting { get; set; }

        public ConversationSortingListItem(string description, ConversationSorting sorting)
        {
            this.Description = description;
            this.Sorting = sorting;
        }

        public ConversationSortingListItem()
            : this(null, ConversationSorting.Unknown)
        {
            ;
        }
    }

    /// <summary>
    /// Interaction logic for OptionsDialogView.xaml
    /// </summary>
    public partial class OptionsDialogView : Window
    {
        public IPhoneDeviceInfo SelectedDevice { get; set; }

        private enum OptionsCategory
        {
            Unknown = 0,
            General,
            ConversationView,
            Phone
        }

        private class OptionsCategoryData
        {
            public string Title { get; set; }
            public StackPanel OptionsPanel { get; set; }

            public OptionsCategoryData(string title, StackPanel optionsPanel)
            {
                this.Title = title;
                this.OptionsPanel = optionsPanel;
            }
        }

        private IDisplayOptions _DisplayOptions;
        private IPhoneSelectOptions _PhoneSelectOptions;
        private OptionsCategory _CurrentCategory;
        private OptionsCategoryData _GeneralOptionsData;
        private OptionsCategoryData _ConversationViewOptionsData;
        private OptionsCategoryData _PhoneOptionsData;

        public OptionsDialogView(IDisplayOptions displayOptions, IPhoneSelectOptions phoneSelectOptions)
        {
            InitializeComponent();
            _DisplayOptions = displayOptions;
            _PhoneSelectOptions = phoneSelectOptions;

            _GeneralOptionsData = new OptionsCategoryData("General", generalOptionsPanel);
            _ConversationViewOptionsData = new OptionsCategoryData("Conversation View", conversationOptionsPanel);
            _PhoneOptionsData = new OptionsCategoryData("Phone", phoneOptionsPanel);

            _CurrentCategory = OptionsCategory.General;

            Loaded += delegate
                {
                    DisplayCurrentCategory();
                    InitializeFields();
                };
        }

        private void DisplayCurrentCategory()
        {
            switch (_CurrentCategory)
            {
                case OptionsCategory.General:
                    DisplayOptions(_GeneralOptionsData);
                    break;
                case OptionsCategory.ConversationView:
                    DisplayOptions(_ConversationViewOptionsData);
                    break;
                case OptionsCategory.Phone:
                    DisplayOptions(_PhoneOptionsData);
                    break;
                default:
                    throw new ArgumentException("Unrecognized options category.");
            }
        }

        private void DisplayOptions(OptionsCategoryData optionsCategoryData)
        {
            categoyTitleLabel.Content = optionsCategoryData.Title;

            categorySpecificOptionsContainer.Children.Clear();
            categorySpecificOptionsContainer.Children.Add(optionsCategoryData.OptionsPanel);
        }

        private void InitializeFields()
        {
            IEnumerable<IPhoneDeviceInfo> devicesInfo = PhoneDeviceInfoReader.GetDevicesInfo();
            phoneListBox.AddPhoneOptions(devicesInfo);

            mergeContactsCheckbox.IsChecked = _DisplayOptions.MergeContacts;
            hideEmptyContactsCheckbox.IsChecked = _DisplayOptions.HideEmptyConversations;
            loadMmsAttachmentsCheckbox.IsChecked = _DisplayOptions.LoadMmsAttachments;
            timeFormatComboBox.SelectedValue = _DisplayOptions.TimeDisplayFormat;
            conversationSortingComboBox.SelectedValue = _DisplayOptions.ConversationSorting;
            phoneListBox.SelectedValue = SelectedDevice;
            promptForPhoneCheckBox.IsChecked = _PhoneSelectOptions.PromptForPhoneChoice;
        }

        private void categoryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = categoryListBox.SelectedIndex;

            if (generalOptionsPanel == null || conversationOptionsPanel == null)
            {
                return;
            }

            if (index == 0)
            {
                _CurrentCategory = OptionsCategory.General;
            }
            else if (index == 1)
            {
                _CurrentCategory = OptionsCategory.ConversationView;
            }
            else if (index == 2)
            {
                _CurrentCategory = OptionsCategory.Phone;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }

            DisplayCurrentCategory();
        }

        private void CommitChanges()
        {
            if (_DisplayOptions.MergeContacts != (mergeContactsCheckbox.IsChecked == true))
            {
                _DisplayOptions.MergeContacts = (mergeContactsCheckbox.IsChecked == true);
            }

            if (_DisplayOptions.HideEmptyConversations != (hideEmptyContactsCheckbox.IsChecked == true))
            {
                _DisplayOptions.HideEmptyConversations = (hideEmptyContactsCheckbox.IsChecked == true);
            }

            if (_DisplayOptions.LoadMmsAttachments != (loadMmsAttachmentsCheckbox.IsChecked == true))
            {
                _DisplayOptions.LoadMmsAttachments = (loadMmsAttachmentsCheckbox.IsChecked == true);
            }

            if (_DisplayOptions.TimeDisplayFormat != (TimeDisplayFormat)timeFormatComboBox.SelectedValue)
            {
                _DisplayOptions.TimeDisplayFormat = (TimeDisplayFormat)timeFormatComboBox.SelectedValue;
            }

            if (_DisplayOptions.ConversationSorting != (ConversationSorting)conversationSortingComboBox.SelectedValue)
            {
                _DisplayOptions.ConversationSorting = (ConversationSorting)conversationSortingComboBox.SelectedValue;
            }
            
            if ((phoneListBox.SelectedValue != null) && (_PhoneSelectOptions.PhoneDataPath != ((IPhoneDeviceInfo)phoneListBox.SelectedValue).BackupPath))
            {
                _PhoneSelectOptions.PhoneDataPath = ((IPhoneDeviceInfo)phoneListBox.SelectedValue).BackupPath;
            }

            _PhoneSelectOptions.PromptForPhoneChoice = (promptForPhoneCheckBox.IsChecked == true);

            _DisplayOptions.Save();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;

            if (phoneListBox.SelectedValue != null)
            {
                SelectedDevice = (IPhoneDeviceInfo)phoneListBox.SelectedValue;
            }

            CommitChanges();
        }
    }
}

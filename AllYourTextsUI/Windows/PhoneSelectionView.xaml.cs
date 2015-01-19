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
using AllYourTextsLib.Framework;
using AllYourTextsUi.Framework;

namespace AllYourTextsUi
{
    /// <summary>
    /// Interaction logic for PhoneSelectionView.xaml
    /// </summary>
    public partial class PhoneSelectionView : Window
    {
        public IPhoneDeviceInfo SelectedPhoneInfo { get; private set; }

        public bool AlwaysPrompt { get; private set; }

        public PhoneSelectionView(IEnumerable<IPhoneDeviceInfo> phoneDevices, IPhoneSelectOptions displayOptions)
        {
            InitializeComponent();

            SelectedPhoneInfo = null;

            AlwaysPrompt = displayOptions.PromptForPhoneChoice;

            Loaded += delegate
                {
                    PopulatePhoneListBox(phoneDevices);
                    promptForPhoneCheckBox.IsChecked = displayOptions.PromptForPhoneChoice;
                };
        }

        private void PopulatePhoneListBox(IEnumerable<IPhoneDeviceInfo> phoneDevices)
        {
            phoneListBox.AddPhoneOptions(phoneDevices);

            phoneListBox.SelectedValue = SelectedPhoneInfo;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedPhoneInfo = (IPhoneDeviceInfo)phoneListBox.SelectedValue;

            AlwaysPrompt = (promptForPhoneCheckBox.IsChecked == true);

            this.DialogResult = true;
        }

        private void phoneListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (phoneListBox.SelectedItem != null)
            {
                okButton.IsEnabled = true;
            }
            else
            {
                okButton.IsEnabled = false;
            }
        }
    }
}

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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AllYourTextsLib.Framework;

namespace AllYourTextsUi
{
    /// <summary>
    /// Interaction logic for PhoneDeviceListItem.xaml
    /// </summary>
    public partial class PhoneDeviceListItem : ListBoxItem
    {
        public IPhoneDeviceInfo DeviceInfo { get; private set; }

        public PhoneDeviceListItem()
        {
            InitializeComponent();
        }

        public PhoneDeviceListItem(IPhoneDeviceInfo deviceInfo)
            :this()
        {
            DeviceInfo = deviceInfo;

            Loaded += delegate
                {

                    if (deviceInfo.DisplayName != null)
                    {
                        const int MaxNameLength = 17;
                        string phoneNameLocal = deviceInfo.DisplayName;
                        if (phoneNameLocal.Length > MaxNameLength)
                        {
                            phoneNameLocal = phoneNameLocal.Substring(0, MaxNameLength) + "...";
                        }
                        phoneNameLabel.Content = phoneNameLocal;
                    }
                    else
                    {
                        phoneNameLabel.Content = "Unknown Phone Name";
                    }

                    if (deviceInfo.LastSync != null)
                    {
                        syncDateTextBlock.Text = string.Format("{0:MMM d, yyyy}", deviceInfo.LastSync);
                    }
                    else
                    {
                        syncDateTextBlock.Text = "Unknown";
                    }
                };
        }
    }
}

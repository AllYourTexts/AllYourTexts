using AllYourTextsLib.Framework;
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

namespace AllYourTextsUi.Controls
{
    /// <summary>
    /// Interaction logic for PhoneDeviceList.xaml
    /// </summary>
    public partial class PhoneDeviceList : ListBox
    {
        public PhoneDeviceList()
        {
            InitializeComponent();
        }

        public void AddPhoneOptions(IEnumerable<IPhoneDeviceInfo> phoneDevices)
        {
            List<IPhoneDeviceInfo> phoneDevicesLocal = new List<IPhoneDeviceInfo>(phoneDevices);
            phoneDevicesLocal.Sort(ComparePhones);

            foreach (IPhoneDeviceInfo phoneDevice in phoneDevicesLocal)
            {
                Items.Add(new PhoneDeviceListItem(phoneDevice));
            }
        }

        private int ComparePhones(IPhoneDeviceInfo phone1, IPhoneDeviceInfo phone2)
        {
            if (!phone1.LastSync.HasValue)
            {
                return 1;
            }
            if (!phone2.LastSync.HasValue)
            {
                return -1;
            }
            else
            {
                return -1 * phone1.LastSync.Value.CompareTo(phone2.LastSync.Value);
            }
        }

        
    }
}

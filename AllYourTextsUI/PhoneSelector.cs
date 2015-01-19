using AllYourTextsLib.Framework;
using AllYourTextsUi.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsUi
{
    public class PhoneSelector
    {
        List<IPhoneDeviceInfo> _devices;

        IPhoneSelectOptionsReadOnly _phoneSelectOptions;

        public PhoneSelector(IEnumerable<IPhoneDeviceInfo> devices, IPhoneSelectOptionsReadOnly phoneSelectOptions)
        {
            _devices = new List<IPhoneDeviceInfo>(devices);
            _phoneSelectOptions = phoneSelectOptions;
        }

        public IPhoneDeviceInfo AutoSelectPhoneDevice()
        {
            if (!CanAutoSelectPhoneDevice())
            {
                return null;
            }
            else if (_devices.Count == 1)
            {
                return _devices[0];
            }
            else if (!string.IsNullOrEmpty(_phoneSelectOptions.PhoneDataPath))
            {
                try
                {
                    return FindDeviceInfoWithPath(_phoneSelectOptions.PhoneDataPath);
                }
                catch (KeyNotFoundException)
                {
                    return null;
                }
            }

            return null;
        }

        public bool ShouldWarnAboutLaterSyncedPhone()
        {
            if (!_phoneSelectOptions.WarnAboutMoreRecentSync)
            {
                return false;
            }

            IPhoneDeviceInfo autoSelectedPhone = AutoSelectPhoneDevice();
            IPhoneDeviceInfo lastSyncedPhone = GetMostRecentlySyncedDeviceInfo();

            if ((autoSelectedPhone != null) && (autoSelectedPhone != lastSyncedPhone))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private IPhoneDeviceInfo GetMostRecentlySyncedDeviceInfo()
        {
            IPhoneDeviceInfo mostRecent = null;

            foreach (IPhoneDeviceInfo deviceInfo in _devices)
            {
                if ((mostRecent == null) || (deviceInfo.LastSync >= mostRecent.LastSync))
                {
                    mostRecent = deviceInfo;
                }
            }

            return mostRecent;
        }

        private IPhoneDeviceInfo FindDeviceInfoWithPath(string path)
        {
            foreach (IPhoneDeviceInfo deviceInfo in _devices)
            {
                if (string.Equals(deviceInfo.BackupPath, path))
                {
                    return deviceInfo;
                }
            }

            throw new KeyNotFoundException("Could not find device info with path " + path);
        }

        private bool CanAutoSelectPhoneDevice()
        {
            if (_devices.Count <= 1)
            {
                return true;
            }
            else if (_phoneSelectOptions.PromptForPhoneChoice)
            {
                return false;
            }
            else if (string.IsNullOrEmpty(_phoneSelectOptions.PhoneDataPath))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib
{
    public class PhoneDeviceInfo : IPhoneDeviceInfo
    {
        public PhoneDeviceInfo(string backupPath, string displayName, DateTime? lastSync, IOsVersion osVersion, Guid? deviceGuid)
        {
            BackupPath = backupPath;
            DisplayName = displayName;
            LastSync = lastSync;
            OsVersion = osVersion;
            DeviceGuid = deviceGuid;
        }

        public string BackupPath { get; private set; }

        public string DisplayName { get; private set; }

        public DateTime? LastSync { get; private set; }

        public IOsVersion OsVersion { get; private set; }

        public Guid? DeviceGuid { get; private set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");

            sb.AppendFormat("BackupPath = {0}", BackupPath);
            sb.AppendFormat(", DisplayName = {0} ", DisplayName);
            sb.AppendFormat(", LastSync = {0}", LastSync);
            sb.AppendFormat(", OsVersion = {0}", OsVersion);
            sb.AppendFormat(", GUID = {0}", DeviceGuid.ToString());

            sb.Append("}");

            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            return Equals((IPhoneDeviceInfo)obj);
        }

        public bool Equals(IPhoneDeviceInfo other)
        {
            if (this.BackupPath != other.BackupPath)
            {
                return false;
            }

            if (this.DisplayName != other.DisplayName)
            {
                return false;
            }

            if (this.LastSync != other.LastSync)
            {
                return false;
            }

            if (this.OsVersion == null)
            {
                if (other.OsVersion != null)
                {
                    return false;
                }
            }
            else if (!this.OsVersion.Equals(other.OsVersion))
            {
                return false;
            }

            if (this.DeviceGuid == null)
            {
                if (other.DeviceGuid != null)
                {
                    return false;
                }
            }
            else if (!this.DeviceGuid.Equals(other.DeviceGuid))
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

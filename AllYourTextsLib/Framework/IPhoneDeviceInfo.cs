using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsLib.Framework
{
    public interface IPhoneDeviceInfo
    {
        string BackupPath { get; }
        string DisplayName { get; }
        DateTime? LastSync { get; }
        IOsVersion OsVersion { get; }
        Guid? DeviceGuid { get; }
    }
}

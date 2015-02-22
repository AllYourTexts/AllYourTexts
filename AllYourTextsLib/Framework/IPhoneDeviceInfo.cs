using System;

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

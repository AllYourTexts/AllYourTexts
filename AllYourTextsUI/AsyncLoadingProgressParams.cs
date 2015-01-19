using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;

namespace AllYourTextsUi
{
    public class AsyncLoadingProgressParams
    {
        public ILoadingProgressCallback LoadingProgressCallback { get; private set; }

        public bool MergeConversations { get; private set; }

        public IPhoneDeviceInfo DeviceInfo { get; private set; }

        public AsyncLoadingProgressParams(ILoadingProgressCallback progressCallback, bool mergeConversations, IPhoneDeviceInfo deviceInfo)
        {
            LoadingProgressCallback = progressCallback;

            MergeConversations = mergeConversations;

            DeviceInfo = deviceInfo;
        }
    }
}

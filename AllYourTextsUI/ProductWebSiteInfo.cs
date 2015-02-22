using System;

namespace AllYourTextsUi
{
    public static class ProductWebSiteInfo
    {
        private const string ProductUrlBaseString = "http://www.allyourtexts.com/";
        private const string SyncTroubleshootingUrlString = ProductUrlBaseString + "missing-conversations/";

        public static Uri Url = new Uri(ProductUrlBaseString);

        public static Uri SyncTroubleshootingUrl = new Uri(SyncTroubleshootingUrlString);

        public const string DisplayUrl = "www.allyourtexts.com";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsUi.Framework;
using System.IO;
using AllYourTextsLib.Framework;
using AllYourTextsLib;

namespace AllYourTextsUi
{
    public class AboutBoxModel : IAboutBoxModel
    {
        public string BuildDateString { get; private set; }

        public AboutBoxModel()
        {
            BuildDateString = string.Format("{0:MMM d, yyyy}", GetBuildDate());
        }

        private DateTime GetBuildDate()
        {
            string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;
            const int PeHeaderOffset = 60;
            const int LinkerTimestampOffset = 8;
            const int PeHeaderLength = 2048;

            byte[] assemblyHeader = new byte[PeHeaderLength];
            Stream assemblyStream = null;

            try
            {
                assemblyStream = new System.IO.FileStream(filePath, FileMode.Open, FileAccess.Read);
                assemblyStream.Read(assemblyHeader, 0, PeHeaderLength);
            }
            finally
            {
                if (assemblyStream != null)
                {
                    assemblyStream.Close();
                }
            }

            int i = System.BitConverter.ToInt32(assemblyHeader, PeHeaderOffset);
            int secondsSince1970 = System.BitConverter.ToInt32(assemblyHeader, i + LinkerTimestampOffset);

            DateTime buildDate = new DateTime(1970, 1, 1, 0, 0, 0);
            buildDate = buildDate.AddSeconds(secondsSince1970);
            buildDate = buildDate.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(buildDate).Hours);

            return buildDate;
        }
    }
}

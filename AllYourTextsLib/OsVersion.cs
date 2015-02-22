using System;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib
{
    public class OsVersion : IOsVersion
    {
        public int MajorVersion { get; private set; }
        public int MinorVersion { get; private set; }
        public int RevisionNumber { get; private set; }

        public OsVersion(string versionString)
        {
            int majorVersion;
            int minorVersion;
            int revisionNumber;
            ParseVersionString(versionString, out majorVersion, out minorVersion, out revisionNumber);

            MajorVersion = majorVersion;
            MinorVersion = minorVersion;
            RevisionNumber = revisionNumber;
        }

        private static void ParseVersionString(string versionString, out int majorVersion, out int minorVersion, out int revisionNumber)
        {
            string[] versionParts = versionString.Split(new char[] { '.' });
            if (versionParts.Length < 2)
            {
                throw new ArgumentException("Version string in invalid format.");
            }

            majorVersion = int.Parse(versionParts[0]);
            minorVersion = int.Parse(versionParts[1]);

            if (versionParts.Length >= 3)
            {
                revisionNumber = int.Parse(versionParts[2]);
            }
            else
            {
                revisionNumber = 0;
            }

            if ((majorVersion < 0) || (minorVersion < 0) || (revisionNumber < 0))
            {
                throw new ArgumentException("Version string in invalid format: version number cannot be negative.");
            }
        }

        public override bool Equals(object obj)
        {
            return this.Equals((OsVersion)obj);
        }

        public bool Equals(OsVersion other)
        {
            if (this.MajorVersion != other.MajorVersion)
            {
                return false;
            }

            if (this.MinorVersion != other.MinorVersion)
            {
                return false;
            }

            if (this.RevisionNumber != other.RevisionNumber)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}", MajorVersion, MinorVersion, RevisionNumber);
        }
    }
}

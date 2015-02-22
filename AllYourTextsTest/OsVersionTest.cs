using System;
using AllYourTextsLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AllYourTextsTest
{
    
    [TestClass()]
    public class OsVersionTest
    {

        private void VerifyVersionMatchesExpected(string versionString, int majorVersionExpected, int minorVersionExpected, int revisionNumberExpected)
        {
            int majorVersionActual = 0;
            int minorVersionActual = 0;
            int revisionNumberActual = 0;
            OsVersion_Accessor.ParseVersionString(versionString, out majorVersionActual, out minorVersionActual, out revisionNumberActual);
            Assert.AreEqual(majorVersionExpected, majorVersionActual);
            Assert.AreEqual(minorVersionExpected, minorVersionActual);
            Assert.AreEqual(revisionNumberExpected, revisionNumberActual);
        }

        private void VerifyParseFails(string versionString)
        {
            Exception exceptionThrown = null;

            try
            {
                int majorVersionActual = 0;
                int minorVersionActual = 0;
                int revisionNumberActual = 0;
                OsVersion_Accessor.ParseVersionString(versionString, out majorVersionActual, out minorVersionActual, out revisionNumberActual);
            }
            catch (Exception ex)
            {
                exceptionThrown = ex;
            }

            Assert.IsNotNull(exceptionThrown);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void ParseVersionStringTest()
        {
            VerifyVersionMatchesExpected("4.2.1", 4, 2, 1);
            VerifyVersionMatchesExpected("3.0.01", 3, 0, 1);
            VerifyVersionMatchesExpected("5.1.6", 5, 1, 6);
            VerifyVersionMatchesExpected("0.0.19", 0, 0, 19);
            VerifyVersionMatchesExpected("5.4", 5, 4, 0);
            VerifyVersionMatchesExpected("5.6.4.5", 5, 6, 4);

            VerifyParseFails("5");
            VerifyParseFails("5.4.");
            VerifyParseFails(".5");
            VerifyParseFails("5.");
            VerifyParseFails("-5.2.1");
            VerifyParseFails("5.-2.1");
            VerifyParseFails("5.2.-1");
            VerifyParseFails("5.2.1a");
            VerifyParseFails("5.b.1");
            VerifyParseFails("H.4.1");
        }

        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void EqualsTest()
        {
            Assert.AreEqual(new OsVersion("4.1.0"), new OsVersion("4.1.0"));
            Assert.AreNotEqual(new OsVersion("4.1.0"), new OsVersion("4.1.1"));
            Assert.AreNotEqual(new OsVersion("4.1.0"), new OsVersion("4.5.0"));
            Assert.AreNotEqual(new OsVersion("4.1.0"), new OsVersion("5.1.0"));
        }
    }
}

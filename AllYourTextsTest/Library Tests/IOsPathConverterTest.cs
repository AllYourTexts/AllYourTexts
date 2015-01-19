using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AllYourTextsLib.DataReader;

namespace AllYourTextsTest.Library_Tests
{
    [TestClass]
    public class IOsPathConverterTest
    {
        [TestInitialize()]
        public void TestInitialize()
        {
            _pathConverter = new IOsPathConverter();
        }

        private void VerifyTranslatedPathMatchesExpected(string iPhonePathInput, string translatedPathExpected)
        {
            string translatedPathActual = _pathConverter.TranslateiPhoneAttachmentPathToComputerPath(iPhonePathInput, _MockBackupPath);
            Assert.AreEqual(translatedPathExpected, translatedPathActual);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void TranslateiPhoneAttachmentPathToComputerPathTest()
        {
            VerifyTranslatedPathMatchesExpected("~/Library/SMS/Attachments/f9/09/DD97CB48-3B51-4DD6-959F-9BF9F6ABB58F/IMG_0004.JPG", _MockBackupPath + "\\851584bf7c55a76d4ec7749cc72d5e0b9185c30b");
            VerifyTranslatedPathMatchesExpected("/var/mobile/Library/SMS/Parts/a5/14/1166-0.JPG", _MockBackupPath + "\\f7dd741d6f4e396966dbdb58166c269db64c9ccd");
        }

        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        [ExpectedException(typeof(ArgumentException))]
        public void TranslateInvalidiPhoneAttachmentPathToComputerPathTest()
        {
            VerifyTranslatedPathMatchesExpected("/invalidpath/DSC_2098.JPG", null);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void GetFilenameFromiPhonePathTest()
        {
            Assert.AreEqual("IMG_0004.JPG",
                            _pathConverter.GetFilenameFromiPhonePath("~/Library/SMS/Attachments/f9/09/DD97CB48-3B51-4DD6-959F-9BF9F6ABB58F/IMG_0004.JPG"));
            Assert.AreEqual("IMG_0003.MOV",
                            _pathConverter.GetFilenameFromiPhonePath("~/Library/SMS/Attachments/86/06/AAC38BE9-38C4-42E0-8B01-A1069FE89FC0/IMG_0003.MOV"));

        }

        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFilenameFromiPhonePathInvalidPathTest()
        {
            _pathConverter.GetFilenameFromiPhonePath("cheese_sandwich.txt");

        }

        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFilenameFromiPhonePathMissingFilenameTest()
        {
            _pathConverter.GetFilenameFromiPhonePath("~/Library/SMS/Attachments/f9/09/DD97CB48-3B51-4DD6-959F-9BF9F6ABB58F/");
        }

        private const string _MockBackupPath = @"C:\fakepath\backup";
        private IOsPathConverter _pathConverter;
    }
}

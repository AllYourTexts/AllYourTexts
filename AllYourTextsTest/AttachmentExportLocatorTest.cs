using AllYourTextsUi.Exporting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AllYourTextsTest
{
    
    
    /// <summary>
    ///This is a test class for AttachmentExportLocatorTest and is intended
    ///to contain all AttachmentExportLocatorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AttachmentExportLocatorTest
    {
        private void VerifyFulPathToRelativePathMatchesExpected(string basePath, string fullPath, string relativePathExpected)
        {
            AttachmentExportLocator_Accessor target = new AttachmentExportLocator_Accessor(basePath);
            string relativePathActual = target.FullPathToRelativePath(fullPath);
            Assert.AreEqual(relativePathExpected, relativePathActual);
        }

        /// <summary>
        ///A test for FullPathToRelativePath
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void FullPathToRelativePathTest()
        {
            VerifyFulPathToRelativePathMatchesExpected(@"C:\Users\Dave\Documents\backup", @"C:\Users\Dave\Documents\backup\JoeyMatthews_attachments\IMG_0036.JPG", @"JoeyMatthews_attachments\IMG_0036.JPG");
            VerifyFulPathToRelativePathMatchesExpected(@"D:\oldcrap\export\", @"D:\oldcrap\export\DarryWalker_attachments\download.jpeg", @"DarryWalker_attachments\download.jpeg");
        }

        /// <summary>
        ///A test for FullPathToRelativePath
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        [ExpectedException(typeof(ArgumentException))]
        public void FullPathToRelativePathInvalidPathTest()
        {
            AttachmentExportLocator_Accessor target = new AttachmentExportLocator_Accessor(@"C:\somepath\base");
            string relativePathActual = target.FullPathToRelativePath(@"C:\otherpath\base\output.txt");
        }
    }
}

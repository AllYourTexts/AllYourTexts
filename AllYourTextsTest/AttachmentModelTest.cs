using System.IO;
using AllYourTextsLib;
using AllYourTextsLib.Framework;
using AllYourTextsUi.Framework.Exceptions;
using AllYourTextsUi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AllYourTextsTest
{
    
    
    /// <summary>
    ///This is a test class for AttachmentModelTest and is intended
    ///to contain all AttachmentModelTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AttachmentModelTest
    {

        /// <summary>
        ///A test for CreateTempCopy
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        [ExpectedException(typeof(AttachmentOpenException))]
        public void CreateTempCopyMissingBackupTest()
        {
            MockFileSystem mockFileSystem = new MockFileSystem();
            IMessageAttachment attachment = new MessageAttachment(1, AttachmentType.Image, @"C:\backup\1", "abc.jpg");
            AttachmentModel_Accessor target = new AttachmentModel_Accessor(mockFileSystem, attachment);
            string actual = target.CreateTempCopy();
        }

        /// <summary>
        ///A test for CreateTempCopy
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void CreateTempCopyExistingBackupTest()
        {
            MockFileSystem mockFileSystem = new MockFileSystem();
            const string BackupPath = @"C:\backup\034234aaljkadfalakdjladfaljkdadfa";
            mockFileSystem.CreateNewFile(BackupPath);

            IMessageAttachment attachment = new MessageAttachment(6, AttachmentType.Image, BackupPath, "IMG_003.JPG");
            AttachmentModel_Accessor target = new AttachmentModel_Accessor(mockFileSystem, attachment);
            string copiedFilePath = target.CreateTempCopy();
            Assert.IsNotNull(copiedFilePath);
        }

        /// <summary>
        ///A test for SaveToFile
        ///</summary>
        [TestMethod()]
        public void SaveToFileTest()
        {
            const string backupFilename = @"C:\backup\1";
            const string outputFilename = @"C:\Documents\abc.jpg";

            MockFileSystem mockFileSystem = new MockFileSystem();
            Stream backupFile = mockFileSystem.CreateNewFile(backupFilename);
            StreamWriter sw = new StreamWriter(backupFile);
            sw.Write("this is dummy data in the backup file");

            IMessageAttachment attachment = new MessageAttachment(5, AttachmentType.Image, backupFilename, "abc.jpg");
            AttachmentModel_Accessor target = new AttachmentModel_Accessor(mockFileSystem, attachment);
            target.SaveToFile(outputFilename);

            Assert.AreEqual(2, mockFileSystem.FileCount);
            Assert.IsTrue(mockFileSystem.FileExists(outputFilename));
        }
    }
}

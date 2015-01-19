using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AllYourTextsUi.Exporting;

namespace AllYourTextsTest.UI_Tests
{
    [TestClass]
    public class FileCreatorTest
    {

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void CreateNewDirectoryWithRenameAttemptsTest()
        {
            MockFileSystem mockFileSystem = new MockFileSystem();
            FileCreator.CreateNewDirectoryWithRenameAttempts("x:\\abc\\def\\", mockFileSystem, 10);
            Assert.AreEqual(1, mockFileSystem.DirectoryCount);
            Assert.IsTrue(mockFileSystem.DirectoryExists("x:\\abc\\def\\"));
            FileCreator.CreateNewDirectoryWithRenameAttempts("x:\\abc\\def\\", mockFileSystem, 10);
            Assert.AreEqual(2, mockFileSystem.DirectoryCount);
            Assert.IsTrue(mockFileSystem.DirectoryExists("x:\\abc\\def (2)"));
            FileCreator.CreateNewDirectoryWithRenameAttempts("x:\\abc\\def\\", mockFileSystem, 10);
            Assert.AreEqual(3, mockFileSystem.DirectoryCount);
            Assert.IsTrue(mockFileSystem.DirectoryExists("x:\\abc\\def (3)"));
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void CreateNewDirectoryWithRenameAttemptsDateTest()
        {
            MockFileSystem mockFileSystem = new MockFileSystem();
            FileCreator.CreateNewDirectoryWithRenameAttempts("D:\\tmp\\ayt-export\\iPhone Text History Backup - 03-03-2012", mockFileSystem, 10);
            Assert.AreEqual(1, mockFileSystem.DirectoryCount);
            Assert.IsTrue(mockFileSystem.DirectoryExists("D:\\tmp\\ayt-export\\iPhone Text History Backup - 03-03-2012"));
            FileCreator.CreateNewDirectoryWithRenameAttempts("D:\\tmp\\ayt-export\\iPhone Text History Backup - 03-03-2012", mockFileSystem, 10);
            Assert.AreEqual(2, mockFileSystem.DirectoryCount);
            Assert.IsTrue(mockFileSystem.DirectoryExists("D:\\tmp\\ayt-export\\iPhone Text History Backup - 03-03-2012 (2)"));
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        [ExpectedException(typeof(Exception))]
        public void CreateNewDirectoryWithRenameAttemptsExcessiveRenamesTest()
        {
            MockFileSystem mockFileSystem = new MockFileSystem();
            FileCreator.CreateNewDirectoryWithRenameAttempts("D:\\fakepath\\namecollision", mockFileSystem, 1);
            FileCreator.CreateNewDirectoryWithRenameAttempts("D:\\fakepath\\namecollision", mockFileSystem, 1);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        [ExpectedException(typeof(Exception))]
        public void CreateNewFileWithRenameAttemptsExcessiveRenamesTest()
        {
            MockFileSystem mockFileSystem = new MockFileSystem();
            FileCreator.CreateNewFileWithRenameAttempts("D:\\fakepath\\namecollision.txt", mockFileSystem, 1);
            FileCreator.CreateNewFileWithRenameAttempts("D:\\fakepath\\namecollision.txt", mockFileSystem, 1);
        }
    }
}

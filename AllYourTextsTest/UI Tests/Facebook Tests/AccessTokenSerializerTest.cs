using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AllYourTextsUi.Facebook;

namespace AllYourTextsTest.UI_Tests.Facebook_Tests
{
    [TestClass]
    public class AccessTokenSerializerTest
    {
        [TestMethod]
        public void TestSaveLoad()
        {
            MockFileSystem fileSystem = new MockFileSystem();
            AccessTokenSerializer serializer = new AccessTokenSerializer(fileSystem);
            
            const string mockTokenValue = "abcdefg";
            AccessToken mockToken = new AccessToken(mockTokenValue, DateTime.Now.AddDays(5));
            serializer.Save(mockToken);

            string deserialized = serializer.Load();
            Assert.AreEqual(mockTokenValue, deserialized);
        }

        [TestMethod]
        public void TestNonExistentLoad()
        {
            MockFileSystem fileSystem = new MockFileSystem();
            AccessTokenSerializer serializer = new AccessTokenSerializer(fileSystem);

            Assert.IsNull(serializer.Load());
        }

        [TestMethod]
        public void TestLoadExpired()
        {
            MockFileSystem fileSystem = new MockFileSystem();
            AccessTokenSerializer serializer = new AccessTokenSerializer(fileSystem);

            Assert.IsNull(serializer.Load());

            const string mockTokenValue = "iamexpired";
            AccessToken mockToken = new AccessToken(mockTokenValue, DateTime.Now.Subtract(new TimeSpan(100)));
            serializer.Save(mockToken);

            string deserialized = serializer.Load();
            Assert.IsNull(deserialized);
        }
    }
}

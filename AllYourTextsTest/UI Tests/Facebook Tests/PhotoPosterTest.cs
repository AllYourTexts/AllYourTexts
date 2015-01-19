using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AllYourTextsUi.Facebook;
using AllYourTextsLib;
using AllYourTextsTest.Mock_Objects;
using System.Net;

namespace AllYourTextsTest.UI_Tests.Facebook_Tests
{
    [TestClass]
    public class PhotoPosterTest
    {
        private const string _MockAccessToken = "abc";
        private readonly byte[] _MockPhotoData = { 0x00, 0x01, 0x02 };
        private const string _MockCaption = "This is a caption!";

        [TestMethod]
        [ExpectedException(typeof(BadAccessTokenException))]
        public void BadAccessTokenTest()
        {
            IHttpFormUploader mockUploader = new MockThrowingFormUploader(new WebException("this is a (400) Bad Request."));

            PhotoPoster photoPoster = new PhotoPoster(_MockAccessToken, mockUploader);
            string result = photoPoster.PostPhoto(_MockPhotoData, _MockCaption);
        }

        [TestMethod]
        [ExpectedException(typeof(UploadFailedException))]
        public void UploadFailedTest()
        {
            IHttpFormUploader mockUploader = new MockThrowingFormUploader(new WebException("Internal 500 Error"));

            PhotoPoster photoPoster = new PhotoPoster(_MockAccessToken, mockUploader);
            string result = photoPoster.PostPhoto(_MockPhotoData, _MockCaption);
        }

        [TestMethod]
        public void SuccessTest()
        {
            string photoIdExpected = "123654";
            string mockResult = "{\"id\":\"" + photoIdExpected + "\"}";
            IHttpFormUploader mockUploader = new MockFormUploader(mockResult);

            PhotoPoster photoPoster = new PhotoPoster(_MockAccessToken, mockUploader);
            string photoIdActual = photoPoster.PostPhoto(_MockPhotoData, _MockCaption);
            Assert.AreEqual(photoIdExpected, photoIdActual);
        }
    }
}

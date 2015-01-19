using AllYourTextsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsTest.Mock_Objects
{
    public class MockThrowingFormUploader : IHttpFormUploader
    {
        public string Url { get; set; }

        private Exception _exceptionToThrow;

        public MockThrowingFormUploader(Exception exceptionToThrow)
        {
            _exceptionToThrow = exceptionToThrow;
        }

        public void AddInputField(string name, string value)
        {
            ;
        }

        public void AddUploadData(string name, string contentType, byte[] data)
        {
            ;
        }

        public string MakeRequest()
        {
            throw _exceptionToThrow;
        }
    }
}

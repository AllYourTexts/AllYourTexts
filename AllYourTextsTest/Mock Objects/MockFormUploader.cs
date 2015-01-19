using AllYourTextsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsTest.Mock_Objects
{
    public class MockFormUploader : IHttpFormUploader
    {

        public string Url { get; set; }

        private string _uploadResult;

        public MockFormUploader(string uploadResult)
        {
            _uploadResult = uploadResult;
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
            return _uploadResult;
        }
    }
}

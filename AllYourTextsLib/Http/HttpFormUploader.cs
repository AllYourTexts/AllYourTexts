using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace AllYourTextsLib
{
    public class HttpFormUploader : IHttpFormUploader
    {
        public string Url { get; set; }
        private string _uploadFileFieldName;
        private string _uploadContentType;
        private byte[] _uploadFileData;
        private NameValueCollection _PostParameters;

        public HttpFormUploader()
        {
            _PostParameters = new NameValueCollection();
        }

        public void AddInputField(string name, string value)
        {
            _PostParameters.Add(name, value);
        }

        public void AddUploadData(string name, string contentType, byte[] data)
        {
            _uploadFileFieldName = name;
            _uploadContentType = contentType;
            _uploadFileData = data;
        }

        public string MakeRequest()
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(Url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = CredentialCache.DefaultCredentials;

            using (Stream requestStream = wr.GetRequestStream())
            {
                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                foreach (string key in _PostParameters.Keys)
                {
                    requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                    string formItem = string.Format(formdataTemplate, key, _PostParameters[key]);
                    byte[] formItemBytes = Encoding.UTF8.GetBytes(formItem);
                    requestStream.Write(formItemBytes, 0, formItemBytes.Length);
                }
                requestStream.Write(boundarybytes, 0, boundarybytes.Length);

                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                string header = string.Format(headerTemplate, _uploadFileFieldName, "upload.png", _uploadContentType);
                byte[] headerbytes = Encoding.UTF8.GetBytes(header);
                requestStream.Write(headerbytes, 0, headerbytes.Length);

                requestStream.Write(_uploadFileData, 0, _uploadFileData.Length);

                byte[] trailer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                requestStream.Write(trailer, 0, trailer.Length);
            }

            string responseString = "";
            using (StreamReader sr = new StreamReader(wr.GetResponse().GetResponseStream()))
            {
                responseString = sr.ReadToEnd();
            }
            return responseString;
        }
    }
}

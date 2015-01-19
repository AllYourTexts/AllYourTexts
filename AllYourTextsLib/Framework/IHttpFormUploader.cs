using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllYourTextsLib
{
    public interface IHttpFormUploader
    {
        string Url { get; set; }

        void AddInputField(string name, string value);

        void AddUploadData(string name, string contentType, byte[] data);

        string MakeRequest();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllYourTextsUi.Facebook
{
    public class PhotoUploadCompleteEventArgs : EventArgs
    {
        public Exception Error { get; private set; }

        public string UploadedId { get; private set; }

        public PhotoUploadCompleteEventArgs(string uploadedId)
        {
            UploadedId = uploadedId;
            Error = null;
        }

        public PhotoUploadCompleteEventArgs(Exception error)
        {
            UploadedId = null;
            Error = error;
        }

    }
}

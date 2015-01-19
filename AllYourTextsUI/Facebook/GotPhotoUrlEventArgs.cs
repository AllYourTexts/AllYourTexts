using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllYourTextsUi.Facebook
{
    public class GotPhotoUrlEventArgs : EventArgs
    {
        public Exception Error { get; private set; }

        public string PhotoUrl { get; private set; }

        public GotPhotoUrlEventArgs(string photoUrl)
        {
            PhotoUrl = photoUrl;
            Error = null;
        }

        public GotPhotoUrlEventArgs(Exception error)
        {
            PhotoUrl = null;
            Error = error;
        }
    }
}

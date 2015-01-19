using AllYourTextsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace AllYourTextsUi.Facebook
{
    public static class PhotoUploadViewFactory
    {
        public static PhotoUploadView Create(BitmapSource photoBitmap, string accessTokenValue)
        {
            HttpFormUploader httpPostRequestor = new HttpFormUploader();

            PhotoPoster photoPoster = new PhotoPoster(accessTokenValue, httpPostRequestor);

            PhotoLinkGrabber linkGrabber = new PhotoLinkGrabber(accessTokenValue);

            PhotoUploadModel uploadModel = new PhotoUploadModel(photoBitmap, photoPoster, linkGrabber);
            PhotoUploadView uploadWindow = new PhotoUploadView(uploadModel);

            return uploadWindow;
        }
    }
}

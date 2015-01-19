using AllYourTextsLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AllYourTextsUi.Facebook
{
    public class PhotoPoster : IPhotoPoster
    {
        private string _accessToken;

        private IHttpFormUploader _httpFormUploader;

        public PhotoPoster(string accessToken, IHttpFormUploader requestor)
        {
            _accessToken = accessToken;
            _httpFormUploader = requestor;
        }

        public string PostPhoto(byte[] photoData, string photoCaption)
        {
            _httpFormUploader.Url = "https://graph.facebook.com/me/photos?access_token=" + _accessToken;

            _httpFormUploader.AddUploadData("source", "image/png", photoData);

            _httpFormUploader.AddInputField("message", photoCaption);

            string postedPhotoId;
            try
            {
                string result = _httpFormUploader.MakeRequest();

                dynamic jsonResult = JsonConvert.DeserializeObject(result);
                postedPhotoId = jsonResult.id;
            }
            catch (WebException ex)
            {
                if (ex.Message.EndsWith("(400) Bad Request."))
                {
                    throw new BadAccessTokenException(ex);
                }
                throw new UploadFailedException(ex);
            }

            return postedPhotoId;
        }
    }
}

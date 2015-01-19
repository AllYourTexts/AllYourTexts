using Newtonsoft.Json;
using System.Net;

namespace AllYourTextsUi.Facebook
{
    public class PhotoLinkGrabber : IPhotoLinkGrabber
    {
        private string _accessToken;

        public PhotoLinkGrabber(string accessToken)
        {
            _accessToken = accessToken;
        }

        public string GetPhotoLink(string photoId)
        {
            WebClient wc = new WebClient();
            string graphUrl = "https://graph.facebook.com/" + photoId +
                                    "?access_token=" + _accessToken;

            string photoMetadataJson = wc.DownloadString(graphUrl);
            
            dynamic photoMetadata = JsonConvert.DeserializeObject(photoMetadataJson);
            string photoLink = photoMetadata.link;

            return photoLink;
        }
    }
}

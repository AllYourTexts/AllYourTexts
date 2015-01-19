using Google.GData.Client;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Web;

namespace AllYourTextsUi.Facebook
{
    public class OAuthTokenGrabber
    {
        public OAuthTokenGrabber()
        {
            ;
        }

        public AccessToken GetAccessToken(string code)
        {
            string token_url = "https://graph.facebook.com/oauth/access_token?" +
                                "client_id=" + OAuthData.AppId +
                                "&redirect_uri=" + Uri.EscapeUriString(OAuthData.RedirectUrl) +
                                "&client_secret=" + OAuthData.AppSecret +
                                "&code=" + code;
            WebClient wc = new WebClient();
            string result = wc.DownloadString(token_url);
            NameValueCollection queryValues = new NameValueCollection();

            HttpUtility.ParseQueryString(result, Encoding.ASCII, queryValues);

            string accessToken = queryValues["access_token"];
            string expires = queryValues["expires"];

            DateTime expirationTime = ParseExpirationTime(expires);

            return new AccessToken(accessToken, expirationTime);
        }

        private DateTime ParseExpirationTime(string expirationTimeString)
        {
            int secondsUntilExpiration = Int32.Parse(expirationTimeString);
            DateTime expiration = DateTime.Now.AddSeconds(secondsUntilExpiration);

            return expiration;
        }
    }
}

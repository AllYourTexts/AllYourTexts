using AllYourTextsUi.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace AllYourTextsUi.Facebook
{
    public class GraphFacebookUploader
    {
        private IAccessTokenSerializer _tokenSerializer;

        public GraphFacebookUploader(IAccessTokenSerializer tokenSerializer)
        {
            _tokenSerializer = tokenSerializer;
        }

        public void DoUpload(BitmapSource graphBitmap)
        {
            try
            {
                ShowUploadDialog(graphBitmap);
            }
            catch (BadAccessTokenException)
            {
                _tokenSerializer.Clear();

                ShowUploadDialog(graphBitmap);
            }
        }

        private void ShowUploadDialog(BitmapSource graphBitmap)
        {
            string accessTokenValue = GetAccessTokenValue();
            if (accessTokenValue == null)
            {
                return;
            }

            PhotoUploadView uploadWindow = PhotoUploadViewFactory.Create(graphBitmap, accessTokenValue);
            uploadWindow.ShowDialog();
        }

        private string GetAccessTokenValue()
        {
            string accessTokenValue = _tokenSerializer.Load();
            if (accessTokenValue != null)
            {
                return accessTokenValue;
            }

            OAuthConsentView consentView = new OAuthConsentView();
            if (consentView.ShowDialog() == true)
            {
                _tokenSerializer.Save(consentView.OAuthAccessToken);

                return consentView.OAuthAccessToken.TokenValue;
            }
            else
            {
                return null;
            }
        }
    }
}

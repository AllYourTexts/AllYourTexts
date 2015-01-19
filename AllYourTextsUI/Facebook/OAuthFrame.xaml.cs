using Google.GData.Client;
using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace AllYourTextsUi.Facebook
{
    /// <summary>
    /// Interaction logic for FacebookOAuthFrame.xaml
    /// </summary>
    public partial class OAuthFrame : UserControl
    {
        public delegate void OAuthCompleteEventHandler(OAuthCompleteEventArgs args);

        public event OAuthCompleteEventHandler OAuthComplete;

        public OAuthFrame()
        {
            InitializeComponent();

            browser.Navigating += OnNavigating;
        }

        public void NavigateToOAuthDialog()
        {
            string oAuthDialogUrl = "http://www.facebook.com/dialog/oauth?" +
                                        "client_id=" + OAuthData.AppId +
                                        "&redirect_uri=" + Uri.EscapeUriString(OAuthData.RedirectUrl) +
                                        "&scope=publish_stream";

            browser.Navigate(oAuthDialogUrl);
        }

        private void OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            Uri uri = e.Uri;
            if (!uri.Host.Equals("www.allyourtexts.com"))
            {
                return;
            }

            NameValueCollection queryValues = new NameValueCollection();
            HttpUtility.ParseQueryString(uri.Query, Encoding.ASCII, queryValues);

            string code = queryValues["code"];
            if (code != null)
            {
                e.Cancel = true;
                FireOAuthCodeFoundEvent(code);
            }
        }

        private void FireOAuthCodeFoundEvent(string oAuthCode)
        {
            if (OAuthComplete != null)
            {
                OAuthComplete(new OAuthCompleteEventArgs(oAuthCode));
            }
        }
    }
}

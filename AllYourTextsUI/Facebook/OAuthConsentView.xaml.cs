using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AllYourTextsUi.Facebook
{
    /// <summary>
    /// Interaction logic for OAuthConsentView.xaml
    /// </summary>
    public partial class OAuthConsentView : Window
    {
        public AccessToken OAuthAccessToken { get; private set; }

        public OAuthConsentView()
        {
            InitializeComponent();

            Loaded += delegate
            {
                fbOAuthFrame.OAuthComplete += handleOAuthCompleteEvent;

                fbOAuthFrame.NavigateToOAuthDialog();
            };
        }

        private void handleOAuthCompleteEvent(OAuthCompleteEventArgs e)
        {
            OAuthTokenGrabber grabber = new OAuthTokenGrabber();
            string oAuthCode = e.OAuthCode;

            OAuthAccessToken = grabber.GetAccessToken(oAuthCode);

            this.DialogResult = true;
            this.Close();
        }
    }
}

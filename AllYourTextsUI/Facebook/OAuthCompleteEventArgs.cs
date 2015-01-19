using System;

namespace AllYourTextsUi.Facebook
{
    public class OAuthCompleteEventArgs : EventArgs
    {
        public string OAuthCode { get; private set; }

        public OAuthCompleteEventArgs(string oAuthCode)
        {
            this.OAuthCode = oAuthCode;
        }
    }
}

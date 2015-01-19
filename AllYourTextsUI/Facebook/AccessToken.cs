using System;

namespace AllYourTextsUi.Facebook
{
    [Serializable()]
    public class AccessToken
    {
        public string TokenValue { get; private set; }

        public DateTime Expires { get; private set; }

        public AccessToken(string tokenValue, DateTime expirationTime)
        {
            TokenValue = tokenValue;
            Expires = expirationTime;
        }
    }
}

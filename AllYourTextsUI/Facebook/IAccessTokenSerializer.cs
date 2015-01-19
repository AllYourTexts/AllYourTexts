using System;

namespace AllYourTextsUi.Facebook
{
    public interface IAccessTokenSerializer
    {
        void Clear();

        string Load();

        void Save(AccessToken accessToken);
    }
}

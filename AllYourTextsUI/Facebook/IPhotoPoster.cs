using System;

namespace AllYourTextsUi.Facebook
{
    public interface IPhotoPoster
    {
        string PostPhoto(byte[] photoData, string photoCaption);
    }
}

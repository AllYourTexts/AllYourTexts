using System;

namespace AllYourTextsUi.Facebook
{
    public interface IPhotoLinkGrabber
    {
        string GetPhotoLink(string photoId);
    }
}

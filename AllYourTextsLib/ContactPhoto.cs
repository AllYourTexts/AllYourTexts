using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsLib
{
    public class ContactPhoto
    {
        private int _contactId;
        
        private int _cropX;
        
        private int _cropY;
        
        private int _cropWidth;

        private int _thumbnailFormat;

        private byte[] _thumbnailData;

        private byte[] _fullSizeData;

        public ContactPhoto(int contactId, int cropX, int cropY, int cropWidth, int thumbnailFormat, byte[] fullSizeData, byte[] thumbnailData)
        {
            _contactId = contactId;

            _thumbnailFormat = thumbnailFormat;

            _cropX = cropX;

            _cropY = cropY;

            _cropWidth = cropWidth;

            _thumbnailData = thumbnailData;

            _fullSizeData = fullSizeData;
        }
    }
}

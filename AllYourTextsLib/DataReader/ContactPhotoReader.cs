using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib.DataReader
{
    public class ContactPhotoReader : DatabaseParserBase<ContactPhoto>
    {
        private const int RecordIdIndex = 0;
        private const int CropXIndex = 1;
        private const int CropYIndex = 2;
        private const int CropWidthIndex = 3;
        private const int FullSizeDataIndex = 4;
        private const int ThumbnailFormatIndex = 5;
        private const int ThumbnailDataIndex = 6;

        protected override string DataQuery
        {
            get
            {
                return @"
SELECT
    ABFullSizeImage.record_id,
    ABFullSizeImage.crop_x,
    ABFullSizeImage.crop_y,
    ABFullSizeImage.crop_width,
    ABFullSizeImage.data,
    ABThumbnailImage.format,
    ABThumbnailImage.data
FROM
    ABFullSizeImage,
    ABThumbnailImage
WHERE
    ABFullSizeImage.record_id=ABThumbnailImage.record_id
";
            }
        }

        protected override string DataCountQuery
        {
            get
            {
                return @"
SELECT
    COUNT(*)
FROM
    ABFullSizeImage,
    ABThumbnailImage
WHERE
    ABFullSizeImage.record_id=ABThumbnailImage.record_id
";
            }
        }

        protected override ContactPhoto ParseItemFromDatabase(IDatabaseReader databaseReader)
        {
            long recordId = databaseReader.GetInt64(RecordIdIndex);
            long cropX = databaseReader.GetInt64(CropXIndex);
            long cropY = databaseReader.GetInt64(CropYIndex);
            long cropWidth = databaseReader.GetInt64(CropWidthIndex);
            byte[] fullSizeData = databaseReader.GetBlob(FullSizeDataIndex);
            long thumbnailFormat = databaseReader.GetInt64(ThumbnailFormatIndex);
            byte[] thumbnailData = databaseReader.GetBlob(ThumbnailDataIndex);

            //TODO: Convert to int gracefully.

            return new ContactPhoto((int)recordId, (int)thumbnailFormat, (int)cropX, (int)cropY, (int)cropWidth, thumbnailData, fullSizeData);
        }
    }
}

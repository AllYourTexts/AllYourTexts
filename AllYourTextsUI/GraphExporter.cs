using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace AllYourTextsUi
{
    public class GraphExporter
    {
        public static void ExportToFile(BitmapSource graphBitmap)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            const int FilterIndexJpeg = 1;
            const int FilterIndexPng = 2;
            saveDialog.Filter = "JPEG|*.jpg|PNG|*.png";
            saveDialog.Title = "Save Graph As";
            saveDialog.AddExtension = true;
            saveDialog.ShowDialog();

            if (string.IsNullOrEmpty(saveDialog.FileName))
            {
                return;
            }

            using (FileStream fileStream = (FileStream)saveDialog.OpenFile())
            {
                BitmapEncoder bitmapEncoder;

                switch (saveDialog.FilterIndex)
                {
                    case FilterIndexJpeg:
                        bitmapEncoder = new JpegBitmapEncoder();
                        break;
                    case FilterIndexPng:
                        bitmapEncoder = new PngBitmapEncoder();
                        break;
                    default:
                        throw new ArgumentException("Invalid file save type");
                }

                bitmapEncoder.Frames.Add(BitmapFrame.Create(graphBitmap));
                bitmapEncoder.Save(fileStream);
            }
        }
    }
}

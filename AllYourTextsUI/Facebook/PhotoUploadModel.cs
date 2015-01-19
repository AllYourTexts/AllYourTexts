using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace AllYourTextsUi.Facebook
{
    public class PhotoUploadModel
    {
        public string Caption { get; set; }

        private BitmapSource _photoBitmap;

        private IPhotoPoster _photoPoster;

        private IPhotoLinkGrabber _photoLinkGrabber;

        public delegate void FacebookPhotoUploadCompleteEventHandler(PhotoUploadCompleteEventArgs args);

        public delegate void GotPhotoUrlEventHandler(GotPhotoUrlEventArgs args);

        public event FacebookPhotoUploadCompleteEventHandler PhotoUploadCompleteEvent;

        public event GotPhotoUrlEventHandler GotPhotoUrlEvent;

        public BitmapSource PhotoBitmap
        {
            get
            {
                return _photoBitmap;
            }
            private set
            {
                _photoBitmap = value;
            }
        }

        public PhotoUploadModel(BitmapSource photoBitmap, IPhotoPoster photoPoster, IPhotoLinkGrabber photoLinkGrabber)
        {
            _photoBitmap = photoBitmap;
            _photoPoster = photoPoster;
            _photoLinkGrabber = photoLinkGrabber;
        }

        public void PublishAsync()
        {
            BackgroundWorker uploadWorker = new BackgroundWorker();
            uploadWorker.WorkerReportsProgress = false;
            uploadWorker.WorkerSupportsCancellation = false;

            uploadWorker.DoWork += uploadWorker_DoWork;
            uploadWorker.RunWorkerCompleted += uploadWorker_RunWorkerCompleted;

            byte[] photoData = BitmapToJpegBytes(_photoBitmap);
            uploadWorker.RunWorkerAsync(photoData);
        }

        void uploadWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            byte[] photoData = (byte[])e.Argument;
            e.Result = _photoPoster.PostPhoto(photoData, Caption + "\r\nhttp://www.allyourtexts.com");
        }

        private void uploadWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ReportUploadError(e.Error);
                return;
            }

            string uploadedId = (string)e.Result;
            ReportUploadSuccess(uploadedId);

            BackgroundWorker linkGrabberWorker = new BackgroundWorker();
            linkGrabberWorker.WorkerReportsProgress = false;
            linkGrabberWorker.WorkerSupportsCancellation = false;

            linkGrabberWorker.DoWork += linkGrabberWorker_DoWork;
            linkGrabberWorker.RunWorkerCompleted += linkGrabberWorker_RunWorkerCompleted;

            linkGrabberWorker.RunWorkerAsync(uploadedId);
        }

        private void linkGrabberWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string photoId = (string)e.Argument;

            e.Result = _photoLinkGrabber.GetPhotoLink(photoId);
        }

        private void linkGrabberWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ReportLinkGrabError(e.Error);
                return;
            }

            string photoUrl = (string)e.Result;

            ReportLinkGrabSuccess(photoUrl);
        }

        private byte[] BitmapToJpegBytes(BitmapSource bitmapSource)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BitmapEncoder bitmapEncoder = new JpegBitmapEncoder();
                bitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                bitmapEncoder.Save(memoryStream);

                return memoryStream.GetBuffer();
            }
        }

        private void ReportUploadError(Exception error)
        {
            if (PhotoUploadCompleteEvent != null)
            {
                PhotoUploadCompleteEvent(new PhotoUploadCompleteEventArgs(error));
            }
        }

        private void ReportUploadSuccess(string uploadedId)
        {
            if (PhotoUploadCompleteEvent != null)
            {
                PhotoUploadCompleteEvent(new PhotoUploadCompleteEventArgs(uploadedId));
            }
        }

        private void ReportLinkGrabError(Exception error)
        {
            if (GotPhotoUrlEvent != null)
            {
                GotPhotoUrlEvent(new GotPhotoUrlEventArgs(error));
            }
        }

        private void ReportLinkGrabSuccess(string photoUrl)
        {
            if (GotPhotoUrlEvent != null)
            {
                GotPhotoUrlEvent(new GotPhotoUrlEventArgs(photoUrl));
            }
        }
    }
}

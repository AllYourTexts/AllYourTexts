using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for PhotoUploadView.xaml
    /// </summary>
    public partial class PhotoUploadView : Window
    {
        private PhotoUploadModel _model;

        public PhotoUploadView()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        public PhotoUploadView(PhotoUploadModel model)
            :this()
        {
            _model = model;
            _model.PhotoUploadCompleteEvent += OnPhotoUploadCompleteEvent;
            _model.GotPhotoUrlEvent += OnGotPhotoUrlEvent;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            uploadProgressBar.Minimum = 0.0;
            uploadProgressBar.Maximum = 1.0;

            graphPreview.Source = _model.PhotoBitmap;
        }

        private void DoPublishAsync()
        {
            ShowUploadingState();

            _model.Caption = photoCaption.Text.Trim();
            _model.PublishAsync();
        }

        private void OnPublishClick(object sender, RoutedEventArgs e)
        {
            DoPublishAsync();
        }

        private void OnCaptionKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DoPublishAsync();
            }
        }

        private void OnPhotoUploadCompleteEvent(PhotoUploadCompleteEventArgs e)
        {
            if (e.Error != null)
            {
                if (e.Error.GetType() == typeof(BadAccessTokenException))
                {
                    Close();
                    throw e.Error;
                }

                ShowErrorState(e.Error.Message);
                return;
            }
        }

        private void OnGotPhotoUrlEvent(GotPhotoUrlEventArgs e)
        {
            if (e.Error != null)
            {
                if (e.Error.GetType() == typeof(BadAccessTokenException))
                {
                    Close();
                    throw e.Error;
                }

                ShowUploadSuccessfulState();
                return;
            }

            Process.Start(e.PhotoUrl);
            Close();
        }

        private void ShowUploadingState()
        {
            photoCaption.IsEnabled = false;

            messagePane.Visibility = Visibility.Hidden;
            errorBlock.Visibility = Visibility.Collapsed;
            successBlock.Visibility = Visibility.Collapsed;

            uploadProgressBar.Visibility = Visibility.Visible;

            publishButton.IsEnabled = false;
        }

        private void ShowErrorState(string errorMessage)
        {
            photoCaption.IsEnabled = true;

            messagePane.Visibility = Visibility.Visible;
            errorBlock.Visibility = Visibility.Visible;
            successBlock.Visibility = Visibility.Collapsed;

            errorText.Text = errorMessage;

            photoCaption.IsEnabled = true;

            uploadProgressBar.Visibility = Visibility.Hidden;

            publishButton.IsEnabled = true;
        }

        private void ShowUploadSuccessfulState()
        {
            photoCaption.IsEnabled = false;

            messagePane.Visibility = Visibility.Visible;
            errorBlock.Visibility = Visibility.Collapsed;
            successBlock.Visibility = Visibility.Visible;

            uploadProgressBar.Value = 1.0;
            uploadProgressBar.IsIndeterminate = false;
            uploadProgressBar.Visibility = Visibility.Visible;

            publishButton.IsEnabled = false;
            cancelButton.Content = "_Close";
        }
    }
}

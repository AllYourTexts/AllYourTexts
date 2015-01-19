using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AllYourTextsUi.Models;
using System.IO;

namespace AllYourTextsUi.Controls
{
    /// <summary>
    /// Interaction logic for AttachmentImage.xaml
    /// </summary>
    public partial class AttachmentImage : AttachmentBase
    {
        public AttachmentImage(AttachmentModel model)
            :base(model)
        {
            InitializeComponent();

            try
            {
                LoadAttachmentImage();
            }
            catch
            {
                LoadErrorImage();
            }
        }

        private void LoadAttachmentImage()
        {
            BitmapImage attachmentImageContents = new BitmapImage();

            using (MemoryStream attachmentMemoryStream = new MemoryStream(File.ReadAllBytes(_model.BackupPath)))
            {
                attachmentImageContents.BeginInit();
                attachmentImageContents.StreamSource = attachmentMemoryStream;
                attachmentImageContents.CacheOption = BitmapCacheOption.OnLoad;
                attachmentImageContents.EndInit();
            }

            attachmentImage.Source = attachmentImageContents;
        }

        private void LoadErrorImage()
        {
            BitmapImage attachmentImageContents = new BitmapImage();
            attachmentImageContents.BeginInit();
            attachmentImageContents.StreamSource = Application.GetResourceStream(new Uri("pack://application:,,,/Images/error_icon.png")).Stream;
            attachmentImageContents.CacheOption = BitmapCacheOption.OnLoad;
            attachmentImageContents.EndInit();

            attachmentImage.Source = attachmentImageContents;
            attachmentImage.Width = 50;
            attachmentImage.Height = 50;
            renderError.Visibility = Visibility.Visible;
            this.BorderThickness = new Thickness(1.0);
            this.BorderBrush = Brushes.Gray;
        }
    }
}

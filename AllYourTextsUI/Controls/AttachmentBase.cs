using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using AllYourTextsUi.Models;
using System.Windows.Input;
using Microsoft.Win32;
using System.Windows;

namespace AllYourTextsUi.Controls
{
    public class AttachmentBase : UserControl
    {
        protected AttachmentModel _model;

        public AttachmentBase()
        {
            ;
        }

        public AttachmentBase(AttachmentModel model)
        {
            _model = model;
        }

        protected void OpenAttachment_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _model.OpenTempCopy();
        }

        protected void OpenAttachment_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        protected void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveAttachmentToFile();
        }

        private void SaveAttachmentToFile()
        {
            string originalExtension = System.IO.Path.GetExtension(_model.OrignalFilename);
            string filterString = "";
            if (originalExtension != string.Empty)
            {
                filterString = string.Format("{0} files|*.{0}|", originalExtension.Substring(1));
            }
            filterString += "All Files|*.*";

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.FileName = _model.OrignalFilename;
            saveDialog.Filter = filterString;
            saveDialog.Title = "Save As";
            saveDialog.ShowDialog();

            if (string.IsNullOrEmpty(saveDialog.FileName))
            {
                return;
            }

            _model.SaveToFile(saveDialog.FileName);
        }
    }
}

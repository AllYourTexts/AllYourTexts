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
using System.Windows.Shapes;
using AllYourTextsLib.Framework;
using AllYourTextsUi.Framework;
using System.ComponentModel;
using AllYourTextsUi.Exporting;
using AllYourTextsUi.Models;

namespace AllYourTextsUi
{

    /// <summary>
    /// Interaction logic for ExportMultipleDialogView.xaml
    /// </summary>
    public partial class ExportMultipleDialogView : Window
    {
        private IProgressCallback _progressCallback;

        private ExportMultipleDialogModel _model;

        public ExportMultipleDialogView(IConversationManager conversationManager, IDisplayOptionsReadOnly displayOptions)
        {
            InitializeComponent();

            IFileSystem exportFileSystem = new OsFileSystem();

            ExportErrorFormatter exportErrorFormatter = new ExportErrorFormatter();

            _model = new ExportMultipleDialogModel(conversationManager, displayOptions, exportFileSystem, exportErrorFormatter);
            _progressCallback = null;

            Loaded += delegate
            {
                folderPathTextBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            };
        }

        private void ChooseFolder_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dlg.ShowDialog(this.GetIWin32Window());
            string selectedPath = dlg.SelectedPath;

            if (string.IsNullOrEmpty(selectedPath))
            {
                return;
            }

            folderPathTextBox.Text = selectedPath;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ExportConversationsAsync();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (_progressCallback == null || _progressCallback.IsCanceled)
            {
                return;
            }

            e.Cancel = true;
            _progressCallback.RequestCancel();
        }

        private ExportMultipleDialogModel.ExportFormat GetExportFormat()
        {
            if (htmlRadioButton.IsChecked == true)
            {
                return ExportMultipleDialogModel.ExportFormat.Html;
            }
            else if (plaintextRadioButton.IsChecked == true)
            {
                return ExportMultipleDialogModel.ExportFormat.Plaintext;
            }

            return ExportMultipleDialogModel.ExportFormat.Unknown;
        }

        private void SetUiToExportInProgress()
        {
            exportingProgressTextBlock.Visibility = Visibility.Visible;
            errorTextBox.Visibility = Visibility.Collapsed;

            folderPathTextBox.IsEnabled = false;
            htmlRadioButton.IsEnabled = false;
            plaintextRadioButton.IsEnabled = false;
            exportButton.IsEnabled = false;
            chooseFolderButton.IsEnabled = false;
        }

        private void SetUiToExportComplete()
        {
            exportingProgressTextBlock.Visibility = Visibility.Visible;

            folderPathTextBox.IsEnabled = false;
            htmlRadioButton.IsEnabled = false;
            plaintextRadioButton.IsEnabled = false;
            chooseFolderButton.IsEnabled = false;

            exportButton.Visibility = Visibility.Hidden;
            cancelButton.Content = "Close";
        }

        private void SetUiToExportSucceeded()
        {
            errorTextBox.Visibility = Visibility.Collapsed;

            exportingProgressTextBlock.Text += ". All conversations exported successfully!";

            SetUiToExportComplete();
        }

        private void SetUiToExportFailed(string errorMessage)
        {
            errorTextBox.Visibility = Visibility.Visible;

            exportingProgressTextBlock.Text += " with errors";
            errorTextBox.Text = errorMessage;

            exportButton.Visibility = Visibility.Collapsed;
            cancelButtonLabel.Content = "OK";

            SetUiToExportComplete();
        }

        private void ExportConversationsAsync()
        {
            ExportedFilenameGenerator filenameGenerator = new ExportedFilenameGenerator();
            string folderName = filenameGenerator.CreateExportFolderNameSuggestion();

            string outputPath = System.IO.Path.Combine(folderPathTextBox.Text, folderName);

            BackgroundWorker backgroundWorker = new BackgroundWorker();
            _progressCallback = new ProgressCallback(backgroundWorker);

            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += OnDoWork;
            backgroundWorker.ProgressChanged += OnProgressChanged;
            backgroundWorker.RunWorkerCompleted += OnWorkerCompleted;

            ExportMultipleDialogModel.ExportFormat exportFormat = GetExportFormat();

            AsyncExportingProgressParams exportingParams = new AsyncExportingProgressParams(_progressCallback, _model, exportFormat, outputPath);

            SetUiToExportInProgress();

            backgroundWorker.RunWorkerAsync(exportingParams);
        }

        private static void OnDoWork(object sender, DoWorkEventArgs e)
        {
            AsyncExportingProgressParams exportingParams = (AsyncExportingProgressParams)e.Argument;

            ExportMultipleDialogModel model = exportingParams.Model;
            ExportMultipleDialogModel.ExportFormat exportFormat = exportingParams.ExportFormat;
            string exportPath = exportingParams.ExportPath;
            IProgressCallback progressCallback = exportingParams.ProgressCallback;

            try
            {
                progressCallback.Begin(model.ConversationCount);

                model.ExportConversations(exportFormat, exportPath, progressCallback);

                progressCallback.End();
            }
            catch (OperationCanceledException)
            {
                e.Cancel = true;
            }
        }

        private void OnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            percentageValueRun.Text = e.ProgressPercentage.ToString();
        }

        private void OnWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _progressCallback = null;
            if (e.Cancelled)
            {
                SetUiToExportFailed("Export canceled.");
            }
            else if (e.Error != null)
            {
                SetUiToExportFailed(e.Error.Message);
            }
            else if (!_model.ExportSucceeded)
            {
                SetUiToExportFailed(_model.ErrorMessage);
            }
            else
            {
                SetUiToExportSucceeded();
            }
        }
    }
}

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
using AllYourTextsUi.Framework;
using System.ComponentModel;

namespace AllYourTextsUi
{
    public partial class SendBugReportDialogView : BugSendDialogViewBase
    {
        public SendBugReportDialogView(IBugSendDialogModel model)
            :base(model)
        {
            InitializeComponent();

            Loaded += delegate { ModelToView(); };
        }

        protected override void ViewToModel()
        {
            _model.CustomerEmailAddress = emailAddressTextBox.Text;

            _model.CustomerComments = userCrashDescriptionTextBox.Text;
        }

        protected override void ModelToView()
        {
            errorReportPreviewTextBox.Text = _model.BugPreviewText;

            //
            // We could try to pre-populate the e-mail address based on registration information, but we don't
            // risk it because it requires reading from the registry.
            //
        }

        private void sendErrorReportButton_Click(object sender, RoutedEventArgs e)
        {
            UploadInformation();
        }

        protected override void DisableSendControls()
        {
            emailAddressTextBox.IsEnabled = false;

            userCrashDescriptionTextBox.IsEnabled = false;

            sendErrorReportButton.IsEnabled = false;
        }

        protected override void EnableSendControls()
        {
            emailAddressTextBox.IsEnabled = true;

            userCrashDescriptionTextBox.IsEnabled = true;

            sendErrorReportButton.IsEnabled = true;
        }

        protected override void ShowError()
        {
            errorTextBlock.Text = _model.ErrorMessage;
            errorTextBlock.Visibility = Visibility.Visible;
        }

        protected override void HideError()
        {
            errorTextBlock.Visibility = Visibility.Hidden;
        }

        protected override void ShowProgressBar()
        {
            sendProgressBar.Visibility = Visibility.Visible;
        }

        protected override void HideProgressBar()
        {
            sendProgressBar.Visibility = Visibility.Hidden;
        }
    }
}

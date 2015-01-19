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
    public partial class SendFeedbackDialogView : BugSendDialogViewBase
    {
        public SendFeedbackDialogView(IBugSendDialogModel model)
            : base(model)
        {
            InitializeComponent();

            Loaded += delegate
                {
                    ModelToView();
                };
        }

        private void sendFeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            UploadInformation();
        }

        protected override void ModelToView()
        {
            emailAddressTextBox.Text = _model.CustomerEmailAddress;
            feedbackTextBox.Text = _model.CustomerComments;
        }

        protected override void ViewToModel()
        {
            _model.CustomerEmailAddress = emailAddressTextBox.Text;
            _model.CustomerComments = feedbackTextBox.Text;
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

        protected override void DisableSendControls()
        {
            emailAddressTextBox.IsEnabled = false;
            feedbackTextBox.IsEnabled = false;
            sendFeedbackButton.IsEnabled = false;
        }

        protected override void EnableSendControls()
        {
            emailAddressTextBox.IsEnabled = true;
            feedbackTextBox.IsEnabled = true;
            sendFeedbackButton.IsEnabled = true;
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

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

namespace AllYourTextsUi
{
    /// <summary>
    /// Interaction logic for LoadingProgressDialogView.xaml
    /// </summary>
    public partial class LoadingProgressDialogView : Window
    {
        public event EventHandler Cancel;

        public LoadingProgressDialogView()
        {
            InitializeComponent();
        }

        public int ProgressValue
        {
            set
            {
                //
                // Only allow increases in progress bar value.
                //

                if (value > progressBar.Value)
                {
                    percentageValueRun.Text = value.ToString();
                    progressBar.Value = value;
                }
            }
        }

        public string ErrorText
        {
            set
            {
                errorTextBox.Text = string.Format("Error: {0}", value);
                errorTextBox.Visibility = Visibility.Visible;
                progressBar.IsIndeterminate = true;
                progressBar.IsEnabled = false;
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            CancelOperation();
        }

        private void CancelOperation()
        {
            if (string.IsNullOrEmpty(errorTextBox.Text) && (Cancel != null))
            {
                Cancel(this, EventArgs.Empty);
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CancelOperation();
        }
    }
}

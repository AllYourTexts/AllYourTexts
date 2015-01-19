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

namespace AllYourTextsUi
{

    /// <summary>
    /// Interaction logic for ErrorDialog.xaml
    /// </summary>
    public partial class DatabaseErrorDialog : Window
    {
        const string MissingDatabaseError = "{0}\r\n\r\n" +
             "This can occur if you have not yet synchronized your iPhone with this PC. Please connect your iPhone to this PC, run a sync with iTunes, and try again.";

        const string UnreadableDatabaseFileError =
            "There was an error loading one of the databases containing text message information. The file '{0}' does not appear to be valid.\r\n\r\n" +
            "If you have iTunes configured to encrypt your backups, you must uncheck the box \"Encrypt iPhone Backup\" in the iPhone configuration window " +
            "of iTunes to access your texts.\r\n\r\n" +
            "If you are still seeing this error after ensuring your iPhone backups are not encrypted, please use the Send Feedback form and we will assist you.";

        public DatabaseErrorDialog(BackupDatabaseReadException ex)
        {
            InitializeComponent();

            Loaded += delegate
            {
                if ((ex is MissingBackupPathException) || (ex is MissingBackupFileException) || (ex is NoBackupsFoundException))
                {
                    errorDetailTextBox.Text = string.Format(MissingDatabaseError, ex.Message);
                    troubleshootingLink.Visibility = Visibility.Visible;
                }
                else if (ex is UnreadableDatabaseFileException)
                {
                    UnreadableDatabaseFileException unreadableDatabaseException = (UnreadableDatabaseFileException)ex;
                    errorDetailTextBox.Text = string.Format(UnreadableDatabaseFileError, unreadableDatabaseException.Filename);
                }
                else
                {
                    throw new ArgumentException("Unrecognized database error type.");
                }
            };
        }

        private void sendFeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}

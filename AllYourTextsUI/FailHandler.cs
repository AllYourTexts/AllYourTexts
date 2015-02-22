using System;
using System.Windows;
using AllYourTextsLib;

namespace AllYourTextsUi
{
    public class FailHandler
    {
        public static void HandleUnrecoverableFailure(Exception e)
        {
            SendBugReportDialogModel bugReportModel = new SendBugReportDialogModel(e, new BugReportCreator());
            SendBugReportDialogView bugReportView = new SendBugReportDialogView(bugReportModel);
            bugReportView.ShowDialog();
            Application.Current.Shutdown(-1);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib;
using System.Windows;

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

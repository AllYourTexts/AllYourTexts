using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsUi.Framework;
using AllYourTextsLib.Framework;
using System.ComponentModel;

namespace AllYourTextsUi
{
    public class SendBugReportDialogModel : BugSendDialogModelBase
    {

        public SendBugReportDialogModel(Exception ex, IBugReportCreator bugReporter)
        {
            _bugReporter = bugReporter;
            _bugReporter.Area = BugArea.FieldCrash;
            _bugReporter.IncludesSystemInformation = true;
            _bugReporter.IncludesVersionInformation = true;
            _bugReporter.RelatedException = ex;

            BugPreviewText = _bugReporter.FullBugDetail.Trim();
        }

        public override bool CanUploadInformation
        {
            get
            {
                return true;
            }
        }

        public override string BugPreviewText { get; protected set; }
    }
}

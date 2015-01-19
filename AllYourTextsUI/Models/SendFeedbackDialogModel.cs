using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsUi.Framework;
using System.Threading;
using AllYourTextsLib.Framework;

namespace AllYourTextsUi
{
    public class SendFeedbackDialogModel : BugSendDialogModelBase
    {
        public SendFeedbackDialogModel(IBugReportCreator bugReporter)
        {
            _bugReporter = bugReporter;
            _bugReporter.Area = BugArea.CustomerFeedback;
            _bugReporter.IncludesSystemInformation = true;
            _bugReporter.IncludesVersionInformation = true;
            _bugReporter.ForceNewBugCreation = true;
        }

        public override bool CanUploadInformation
        {
            get
            {
                if (string.IsNullOrWhiteSpace(CustomerComments))
                {
                    ErrorMessage = "No feedback entered.";
                    return false;
                }

                return true;
            }
        }

        public override string BugPreviewText
        {
            get
            {
                throw new NotImplementedException();
            }
            protected set
            {
                throw new NotImplementedException();
            }
        }
    }
}

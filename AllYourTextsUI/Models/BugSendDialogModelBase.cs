using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;
using AllYourTextsUi.Framework;
using AllYourTextsLib;

namespace AllYourTextsUi
{
    public abstract class BugSendDialogModelBase : IBugSendDialogModel
    {
        protected IBugReportCreator _bugReporter;

        public string CustomerEmailAddress { get; set; }

        public string CustomerComments { get; set; }

        public string ErrorMessage { get; protected set; }

        public bool SendInformationSucceeded { get; protected set; }

        public void UploadInformation()
        {
            GatherInformationForUpload();

            try
            {
                const string BugUrl = "https://nonsensical.fogbugz.com/scoutSubmit.asp";
                const string BugUser = "customer";

                BugzScoutWrapper bugzScout = new BugzScoutWrapper(BugUrl, BugUser);

                SendInformationSucceeded = _bugReporter.Report(bugzScout);
                if (!SendInformationSucceeded)
                {
                    ErrorMessage = _bugReporter.BugServerResponse;
                }
            }
            catch (Exception ex)
            {
                SendInformationSucceeded = false;
                ErrorMessage = ex.Message;
            }
        }

        private void GatherInformationForUpload()
        {
            if (!string.IsNullOrEmpty(CustomerEmailAddress))
            {
                _bugReporter.CustomerEmail = CustomerEmailAddress;
            }
            _bugReporter.CustomerComments = CustomerComments;
        }

        public abstract bool CanUploadInformation { get; }

        public abstract string BugPreviewText { get; protected set; }
    }
}

using System;

namespace AllYourTextsUi.Framework
{
    public interface IBugSendDialogModel
    {
        string CustomerComments { get; set; }

        string CustomerEmailAddress { get; set; }

        string ErrorMessage { get; }

        bool CanUploadInformation { get; }

        void UploadInformation();

        bool SendInformationSucceeded { get; }

        string BugPreviewText { get; }
    }
}

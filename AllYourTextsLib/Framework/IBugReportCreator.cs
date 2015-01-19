using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsLib.Framework
{
    public enum BugArea
    {
        Unknown = 0,
        CustomerFeedback,
        FieldCrash
    }

    public interface IBugReportCreator
    {
        string FullBugDetail { get; }

        string CustomerEmail { get; set; }

        string CustomerComments { get; set; }

        string EnvironmentInfo { get; }

        Exception RelatedException { get; set; }

        BugArea Area { get; set; }

        bool ForceNewBugCreation { get; set; }

        bool IncludesSystemInformation { get; set; }

        bool IncludesVersionInformation { get; set; }

        bool Report(IBugReportCollector reportCollector);

        string BugServerResponse { get; }
    }
}

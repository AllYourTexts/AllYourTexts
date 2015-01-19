using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsLib.Framework
{
    public interface IBugReportCollector
    {
        string SubmitUrl { get; }

        string SubmitUser { get; }

        string Project { get; set; }

        string Area { get; set; }

        string DefaultMessage { get; set; }

        string CustomerEmail { get; set; }

        string Title { get; set; }

        string ExtraInformation { get; set; }

        bool ForceNewBug { get; set; }

        string Submit();
    }
}

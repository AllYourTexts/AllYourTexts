using System;
using System.Text;
using AllYourTextsLib.Framework;

namespace AllYourTextsTest.Mock_Objects
{
    public class MockBugReportCollector : IBugReportCollector
    {
        public string SubmitUrl
        {
            get { return "http://fakebugreportingsite.com/acceptreports.php"; }
        }

        public string SubmitUser
        {
            get { return "FakeUser"; }
        }

        public string Project { get; set; }

        public string Area { get; set; }

        public string DefaultMessage { get; set; }

        public string CustomerEmail { get; set; }

        public string Title { get; set; }

        public string ExtraInformation { get; set; }

        public bool ForceNewBug { get; set; }

        public string Submit()
        {
            return "";  // empty response indicates success
        }
    }
}

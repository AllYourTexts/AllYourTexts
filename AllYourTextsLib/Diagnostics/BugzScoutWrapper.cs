using AllYourTextsLib.Framework;

namespace AllYourTextsLib
{
    public class BugzScoutWrapper : IBugReportCollector
    {
        private FogBugz.BugReport _bugzScoutBug;

        public BugzScoutWrapper(string bugSubmitUrl, string bugSubmitUsername)
        {
            _bugzScoutBug = new FogBugz.BugReport(bugSubmitUrl, bugSubmitUsername);
        }

        public string SubmitUrl
        {
            get { return _bugzScoutBug.FogBugzUrl; }
        }

        public string SubmitUser
        {
            get { return _bugzScoutBug.FogBugzUsername; }
        }

        public string Project
        {
            get { return _bugzScoutBug.Project; }
            set { _bugzScoutBug.Project = value; }
        }

        public string Area
        {
            get { return _bugzScoutBug.Area; }
            set { _bugzScoutBug.Area = value; }
        }

        public string DefaultMessage
        {
            get { return _bugzScoutBug.DefaultMsg; }
            set { _bugzScoutBug.DefaultMsg = value; }
        }

        public string CustomerEmail
        {
            get { return _bugzScoutBug.Email; }
            set { _bugzScoutBug.Email = value; }
        }

        public string Title
        {
            get { return _bugzScoutBug.Description; }
            set { _bugzScoutBug.Description = value; }
        }

        public string ExtraInformation
        {
            get { return _bugzScoutBug.ExtraInformation; }
            set { _bugzScoutBug.ExtraInformation = value; }
        }

        public bool ForceNewBug
        {
            get { return _bugzScoutBug.ForceNewBug; }
            set { _bugzScoutBug.ForceNewBug = value; }
        }

        public string Submit()
        {
            return _bugzScoutBug.Submit();
        }
    }
}

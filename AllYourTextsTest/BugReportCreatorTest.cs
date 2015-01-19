using AllYourTextsLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AllYourTextsLib.Framework;
using AllYourTextsTest.Mock_Objects;

namespace AllYourTextsTest
{
    
    
    /// <summary>
    ///This is a test class for BugReportCreatorTest and is intended
    ///to contain all BugReportCreatorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BugReportCreatorTest
    {
        /// <summary>
        ///A test for Report
        ///</summary>
        [TestMethod()]
        public void ReportTest()
        {
            const string ExceptionMessage = "This is a fake exception message!";
            BugReportCreator target = new BugReportCreator();
            try
            {
                throw new InvalidOperationException(ExceptionMessage);
            }
            catch (Exception ex)
            {
                target.RelatedException = ex;
            }
            
            IBugReportCollector reportCollector = new MockBugReportCollector();
            target.IncludesSystemInformation = true;
            target.IncludesVersionInformation = true;
            Assert.IsTrue(target.Report(reportCollector));

            Assert.IsTrue(reportCollector.ExtraInformation.Contains("===Environment%20Information==="));
            Assert.IsTrue(reportCollector.ExtraInformation.Contains("===Exception%20Information==="));
            Assert.IsFalse(reportCollector.ExtraInformation.Contains("===Customer%20Comments==="));
            Assert.IsTrue(reportCollector.ExtraInformation.Contains(Uri.EscapeUriString(ExceptionMessage)));
        }
    }
}

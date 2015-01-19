using AllYourTextsLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AllYourTextsTest
{
    
    
    /// <summary>
    ///This is a test class for BugReporterTest and is intended
    ///to contain all BugReporterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BugReporterTest
    {

        private void VerifyFirstLineMatchesExpected(string inputString, string firstLineExpected)
        {
            string firstLineActual = BugReportCreator_Accessor.GetFirstLine(inputString);

            Assert.AreEqual(firstLineExpected, firstLineActual);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void GetFirstLineTest()
        {
            VerifyFirstLineMatchesExpected("This program is awesome!!!! Seriously, it's so cool. I love it and want to marry it.",
                                           "This program is awesome");

            VerifyFirstLineMatchesExpected(".",
                                           ".");

            VerifyFirstLineMatchesExpected(".backwards written is feedback This",
                                           ".backwards written is feedback This");

            VerifyFirstLineMatchesExpected("AMAZING\n\nI like how all the features are there and not absent\nIt'd be lame if they weren't there thx",
                                           "AMAZING");
        }
    }
}

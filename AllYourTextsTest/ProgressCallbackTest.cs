using AllYourTextsUi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AllYourTextsTest
{
    
    /// <summary>
    ///This is a test class for ProgressCallbackTest and is intended
    ///to contain all ProgressCallbackTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProgressCallbackTest
    {

        private void VerifyPercentComplete(int amountCompleted, int amountTotal, int amountCompletedExpected)
        {
            int amountCompletedActual = ProgressCallback_Accessor.GetPercentComplete(amountCompleted, amountTotal);
            Assert.AreEqual(amountCompletedExpected, amountCompletedActual);
        }

        /// <summary>
        ///A test for GetPercentComplete
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AllYourTexts.exe")]
        public void GetPercentCompleteTest()
        {
            VerifyPercentComplete(0, 100, 0);
            VerifyPercentComplete(5, 100, 5);
            VerifyPercentComplete(85, 100, 85);
            VerifyPercentComplete(101, 200, 50);
            VerifyPercentComplete(26, 0, 0);
        }
    }
}

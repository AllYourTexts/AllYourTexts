using AllYourTextsLib;
using AllYourTextsLib.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AllYourTextsTest
{
    
    
    /// <summary>
    ///This is a test class for ContactComparerTest and is intended
    ///to contain all ContactComparerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ContactComparerTest
    {

        private static int ReduceToUnitValue(int value)
        {
            if (value < 0)
            {
                return -1;
            }
            else if (value > 0)
            {
                return 1;
            }

            return 0;
        }

        private static void VerifyRightSideGreater(IContact contactA, IContact contactB)
        {
            int comparisonValue = ContactComparer_Accessor.CompareAlphabetically(contactA, contactB);
            int unitValue = ReduceToUnitValue(comparisonValue);

            Assert.AreEqual(1, unitValue);
        }

        private static void VerifyContactsSortingEqual(IContact contactA, IContact contactB)
        {
            int comparisonValue = ContactComparer_Accessor.CompareAlphabetically(contactA, contactB);

            Assert.AreEqual(0, comparisonValue);
        }

        [TestMethod()]
        public void CompareContactsTest()
        {
            VerifyRightSideGreater(new MockContact("Alex", null, "Aaronson"), new MockContact("Alex", "James", "Aaronsen"));
            VerifyRightSideGreater(new MockContact("Alex", null, "Aaronson"), new MockContact("alex", "james", "aaronsen"));

            VerifyRightSideGreater(new MockContact("Mickey", null, "Mouse"), new MockContact("mickey", null, "mouse"));
        }
    }
}

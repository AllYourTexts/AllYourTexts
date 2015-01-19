using AllYourTextsLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AllYourTextsLib.Framework;

namespace AllYourTextsTest
{
    
    
    /// <summary>
    ///This is a test class for PhoneNumberTest and is intended
    ///to contain all PhoneNumberTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PhoneNumberTest
    {

        public void VerifyNumbersAreEquivalent(string numberValue1, string numberValue2)
        {
            PhoneNumber number1 = new PhoneNumber(numberValue1);
            PhoneNumber number2 = new PhoneNumber(numberValue2);
            Assert.IsTrue(PhoneNumber.NumbersAreEquivalent(number1, number2));
        }

        public void VerifyNumbersAreEquivalent(string numberValue1, string country1, string numberValue2, string country2)
        {
            PhoneNumber number1 = new PhoneNumber(numberValue1, country1);
            PhoneNumber number2 = new PhoneNumber(numberValue2, country2);
            Assert.IsTrue(PhoneNumber.NumbersAreEquivalent(number1, number2));
        }

        public void VerifyNumbersNotEquivalent(string numberValue1, string numberValue2)
        {
            PhoneNumber number1 = new PhoneNumber(numberValue1);
            PhoneNumber number2 = new PhoneNumber(numberValue2);
            Assert.IsFalse(PhoneNumber.NumbersAreEquivalent(number1, number2));
        }

        /// <summary>
        ///A test for StripPhoneNumber
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void NumbersAreEquivalentTest()
        {
            VerifyNumbersAreEquivalent("(452) 823-9238", "(452) 823-9238");         // Number should be equivalent to itself.
            VerifyNumbersAreEquivalent("(452) 823-9238", "  (452) 823 - 9238   ");  // Whitespace should be ignored.
            VerifyNumbersAreEquivalent("(452) 823-9238", "452-823-9238");           // Number should be equivalent to itself.
            VerifyNumbersAreEquivalent("(452) 823-9238", "4528239238");             // Number should be equivalent to itself.
            VerifyNumbersAreEquivalent("(452) 823-9238", "+1 (452) 823-9238");      // Ensure leading +1 is ignored.
            VerifyNumbersAreEquivalent("(452) 823-9238", "   +1 (452) 823-9238");   // Ensure leading +1 with whitespace is ignored.
            VerifyNumbersAreEquivalent("(452) 823-9238", "1 (452) 823-9238");       // Ensure leading 1 is ignored.
            VerifyNumbersAreEquivalent("(452) 823-9238", "  1 (452) 823-9238");     // Ensure leading 1 with whitespace is ignored.
            VerifyNumbersAreEquivalent("(552) 823-9238", "1-552-823-9238");         // Ensure leading 1 is ignored.
            VerifyNumbersAreEquivalent("(552) 823-9238", "ca", "1-552-823-9238", "ca");         // Ensure leading 1 is ignored for Canada.
            VerifyNumbersAreEquivalent("00353872501930", "uk", "+353872501930", "uk");          // England->Ireland and Ireland->England.
            VerifyNumbersAreEquivalent("+447711210689", "uk", "07711 210689", "uk");            // Ensure British numbers are equivalent regardless of leading zero.
            VerifyNumbersAreEquivalent("447711210689", "uk", "07711 210689", "uk");            // Ensure British numbers are equivalent even when prefixed w/ country code and no +.
            VerifyNumbersAreEquivalent("+33645372804", "fr", "0645372804", "fr");               // Ensure French numbers are equivalent regardless of leading zero.
            VerifyNumbersAreEquivalent("+61 420 015 435", "au", "0420 015 435", "au");               // Ensure French numbers are equivalent regardless of leading zero.
            VerifyNumbersAreEquivalent("VerizonBuddy", "VerizonBuddy");             // Ensure character numbers are equivalent.

            VerifyNumbersNotEquivalent("(52) 823-9238", "152-823-9238");            // Make sure not all leading 1's are stripped.
            VerifyNumbersNotEquivalent("*41", "41");                                // Ensure asterisks are not stripped.
            VerifyNumbersNotEquivalent("(452) 823-9238#8726", "(452) 823-9238 8726");   // Ensure hash symbol is not ignored.
            VerifyNumbersNotEquivalent("BTUpdates", "OrangeUpdates");               // Ensure character numbers are recognized as different".
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AllYourTextsUi;
using AllYourTextsLib;
using AllYourTextsLib.Framework;

namespace AllYourTextsTest
{
    [TestClass()]
    public class PhoneNumberFormatterTest
    {

        private static void VerifyPhoneNumberFormatMatches(string phoneNumberUnformatted, string phoneNumberFormattedExpected)
        {
            Assert.AreEqual(phoneNumberFormattedExpected, PhoneNumberFormatter.FormatForContactList(new PhoneNumber(phoneNumberUnformatted)));
        }

        private static void VerifyPhoneNumberFormatMatches(string phoneNumberUnformatted, string country, string phoneNumberFormattedExpected)
        {
            Assert.AreEqual(phoneNumberFormattedExpected, PhoneNumberFormatter.FormatForContactList(new PhoneNumber(phoneNumberUnformatted, country)));
        }

        /// <summary>
        ///A test for FormatPhoneNumber
        ///</summary>
        [TestMethod()]
        public void FormatPhoneNumberTest()
        {
            VerifyPhoneNumberFormatMatches("(555) 924-1355", "555-924-1355");
            VerifyPhoneNumberFormatMatches("555-924-1355", "555-924-1355");
            VerifyPhoneNumberFormatMatches("555924-1355", "555-924-1355");
            VerifyPhoneNumberFormatMatches("5559241355", "555-924-1355");
            VerifyPhoneNumberFormatMatches("555924-1355", "555-924-1355");
            VerifyPhoneNumberFormatMatches("5559241355", "555-924-1355");
            VerifyPhoneNumberFormatMatches("5-5-5-924             1355", "555-924-1355");
            VerifyPhoneNumberFormatMatches("1-800-777-1234", "800-777-1234");
            VerifyPhoneNumberFormatMatches("+1-800-777-1234", "800-777-1234");
            VerifyPhoneNumberFormatMatches("321-456-7890", "321-456-7890");
            VerifyPhoneNumberFormatMatches("1-321-456-7890", "321-456-7890");
            VerifyPhoneNumberFormatMatches("+1-321-456-7890", "321-456-7890");
            VerifyPhoneNumberFormatMatches("3214567890", "US", "321-456-7890");
            VerifyPhoneNumberFormatMatches("321-456-7890", CountryCallingCodeFinder.CountryAbbreviationCanada, "321-456-7890");
            VerifyPhoneNumberFormatMatches("1-321-456-7890", CountryCallingCodeFinder.CountryAbbreviationCanada, "321-456-7890");
            VerifyPhoneNumberFormatMatches("+1-321-456-7890", CountryCallingCodeFinder.CountryAbbreviationCanada, "321-456-7890");
            VerifyPhoneNumberFormatMatches("5", "5");
            VerifyPhoneNumberFormatMatches("555-321-456", "555321456");
            VerifyPhoneNumberFormatMatches("*3256", "*3256");
            VerifyPhoneNumberFormatMatches("#8728", "#8728");
            VerifyPhoneNumberFormatMatches("+44 20 7840 0889", CountryCallingCodeFinder.CountryAbbreviationUnitedKingdom, "2078400889");
            VerifyPhoneNumberFormatMatches("+44 20 7840 0889", CountryCallingCodeFinder.CountryAbbreviationUnitedKingdom, "2078400889");
            VerifyPhoneNumberFormatMatches("0131 229 7899", CountryCallingCodeFinder.CountryAbbreviationUnitedKingdom, "1312297899"); //scottish number
            VerifyPhoneNumberFormatMatches("+49 30 22440", CountryCallingCodeFinder.CountryAbbreviationUnitedKingdom, "493022440");
            VerifyPhoneNumberFormatMatches("30 22440", CountryCallingCodeFinder.CountryAbbreviationUnitedKingdom, "3022440");
            VerifyPhoneNumberFormatMatches("OrangeUpdates", "OrangeUpdates");
            VerifyPhoneNumberFormatMatches("skippy.green@aol.com", "skippy.green@aol.com");
        }
    }
}

using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using AllYourTextsLib;
using AllYourTextsLib.Framework;

namespace AllYourTextsUi
{
    public class PhoneNumberFormatter
    {
        private static readonly Regex _onlyNumbersRegex = new Regex("^[0-9]{10}$");

        private static readonly string[] AmericanFormatCountries = { CountryCallingCodeFinder.CountryAbbreviationUnitedStates, CountryCallingCodeFinder.CountryAbbreviationCanada };

        public static string FormatForContactList(IPhoneNumber phoneNumber)
        {
            return FormatNumberWithDashes(phoneNumber);
        }

        public static string FormatNumberWithDashes(IPhoneNumber phoneNumber)
        {
            string phoneNumberStripped = PhoneNumber.Strip(phoneNumber);

            if (_onlyNumbersRegex.IsMatch(phoneNumberStripped) && (string.IsNullOrEmpty(phoneNumber.Country) || CountryUsesAmericanStylePhoneFormat(phoneNumber.Country)))
            {
                return string.Format("{0}-{1}-{2}",
                                     phoneNumberStripped.Substring(0, 3),
                                     phoneNumberStripped.Substring(3, 3),
                                     phoneNumberStripped.Substring(6, 4));
            }
            else
            {
                return string.Format("{0}", phoneNumberStripped);
            }
        }

        private static bool CountryUsesAmericanStylePhoneFormat(string countryName)
        {
            return AmericanFormatCountries.Contains(countryName);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib
{

    public class PhoneNumber : IPhoneNumber
    {
        public string Number { get; private set; }

        private string _country;

        public string Country
        {
            get
            {
                return _country;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _country = value.ToLower();
                }
                else
                {
                    _country = null;
                }
            }
        }
        
        private static readonly Regex CharStripRegex = new Regex("[^0-9*#+A-Za-z@.]", RegexOptions.Compiled);

        public PhoneNumber(string number, string country)
        {
            this.Number = number;
            this.Country = country;
        }

        public PhoneNumber(string number)
            :this(number, null)
        {
            ;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            PhoneNumber pn = (PhoneNumber)obj;
            return Equals(pn);
        }

        public bool Equals(PhoneNumber other)
        {
            if (this.Number != other.Number)
            {
                return false;
            }

            if (this.Country != other.Country)
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            return string.Format("{{ Number = {0}, Country = {1}}}", this.Number, this.Country);
        }

        public bool IsEquivalentTo(IPhoneNumber number)
        {
            return NumbersAreEquivalent(this, number);
        }

        public static bool NumbersAreEquivalent(IPhoneNumber number1, IPhoneNumber number2)
        {
            string number1Stripped = Strip(number1);
            string number2Stripped = Strip(number2);

            return number1Stripped.Equals(number2Stripped);
        }

        public static string Strip(IPhoneNumber number)
        {
            if (number.Number == null)
            {
                return null;
            }

            string strippedNumber;

            strippedNumber = number.Number;

            strippedNumber = CharStripRegex.Replace(strippedNumber, "");

            try
            {
                if (!string.IsNullOrEmpty(number.Country))
                {
                    string countryCallingCode = CountryCallingCodeFinder.GetCountryCallingCode(number.Country);
                    Regex countryCodeRegex = new Regex(@"^\+?" + countryCallingCode);
                    strippedNumber = countryCodeRegex.Replace(strippedNumber, "");
                }
            }
            catch
            {
                ;
            }

            strippedNumber = StripLeadingZeroes(strippedNumber, number.Country);

            //
            // Strip any remaining +'s
            //

            strippedNumber = strippedNumber.Replace("+", "");

            //
            // In an 11 digit US/Canadian number with a leading 1, strip the leading 1.
            //

            if ((number.Country == CountryCallingCodeFinder.CountryAbbreviationUnitedStates) || (number.Country == CountryCallingCodeFinder.CountryAbbreviationCanada) || string.IsNullOrEmpty(number.Country))
            {
                if ((strippedNumber.Length == 11) && (strippedNumber.StartsWith("1")))
                {
                    strippedNumber = strippedNumber.Substring(1);
                }
            }

            return strippedNumber;
        }

        public static string StripLeadingZeroes(string phoneNumberValue, string country)
        {
            if (phoneNumberValue.StartsWith("00"))
            {
                return phoneNumberValue.Substring(2);
            }
            else if (phoneNumberValue.StartsWith("0"))
            {
                return phoneNumberValue.Substring(1);
            }

            return phoneNumberValue;
        }

        public int CompareTo(object obj)
        {
            return CompareTo((PhoneNumber)obj);
        }

        public int CompareTo(PhoneNumber other)
        {
            return this.Number.CompareTo(other.Number);
        }
    }
}

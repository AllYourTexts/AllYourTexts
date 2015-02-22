using System.Collections.Generic;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib
{
    public abstract class ContactBase : IContact
    {
        public abstract long ContactId { get; protected set; }
        public abstract List<IPhoneNumber> PhoneNumbers { get; protected set; }
        public abstract string FirstName { get; protected set; }
        public abstract string MiddleName { get; protected set; }
        public abstract string LastName { get; protected set; }
        public abstract string DisplayName { get; }

        protected void AddPhoneNumber(IPhoneNumber phoneNumberToAdd)
        {
            //
            // If the number is already in the phone number list, ignore it.
            //

            foreach (IPhoneNumber phoneNumber in PhoneNumbers)
            {
                if (PhoneNumber.NumbersAreEquivalent(phoneNumber, phoneNumberToAdd))
                {
                    return;
                }
            }

            this.PhoneNumbers.Add(phoneNumberToAdd);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            IContact c = (IContact)obj;
            return Equals(c);
        }

        public bool Equals(IContact other)
        {
            if (this.ContactId != other.ContactId)
            {
                return false;
            }

            return true;
        }
    }
}

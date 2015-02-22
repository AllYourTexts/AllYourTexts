using System.Collections.Generic;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib
{
    public class MergedContact : ContactBase, IContact
    {
        public override long ContactId { get; protected set; }
        public override List<IPhoneNumber> PhoneNumbers { get; protected set; }
        public override string FirstName { get; protected set; }
        public override string MiddleName { get; protected set; }
        public override string LastName { get; protected set; }

        public override string DisplayName
        {
            get
            {
                return _displayName;
            }
        }

        private string _displayName;

        public MergedContact(IContact contactA, IContact contactB)
        {
            PhoneNumbers = new List<IPhoneNumber>();
            AddPhoneNumbers(contactA.PhoneNumbers);
            AddPhoneNumbers(contactB.PhoneNumbers);
            PhoneNumbers.Sort();

            ContactId = contactA.ContactId;
            FirstName = contactA.FirstName;
            MiddleName = contactA.MiddleName;
            LastName = contactA.LastName;
            _displayName = contactA.DisplayName;
        }

        private void AddPhoneNumbers(IEnumerable<IPhoneNumber> phoneNumbers)
        {
            foreach (IPhoneNumber phoneNumber in phoneNumbers)
            {
                AddPhoneNumber(phoneNumber);
            }
        }
    }
}

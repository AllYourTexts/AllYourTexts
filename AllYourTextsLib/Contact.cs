using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib
{
    public class Contact : ContactBase, IContact
    {
        public override long ContactId { get; protected set; }

        public override List<IPhoneNumber> PhoneNumbers { get; protected set; }

        private string _firstName;
        private string _middleName;
        private string _lastName;
        private string _displayNameCached;

        public const long UnknownContactId = -1;

        public Contact(long contactId, string firstName, string middleName, string lastName, IPhoneNumber phoneNumber)
        {
            ContactId = contactId;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            
            _displayNameCached = null;

            PhoneNumbers = new List<IPhoneNumber>();
            if (phoneNumber != null)
            {
                PhoneNumbers.Add(phoneNumber);
            }
        }

        public override string FirstName
        {
            get
            {
                return _firstName;
            }
            protected set
            {
                SetIfNotEmpty(out _firstName, value);
            }
        }

        public override string MiddleName
        {
            get
            {
                return _middleName;
            }
            protected set
            {
                SetIfNotEmpty(out _middleName, value);
            }
        }

        public override string LastName
        {
            get
            {
                return _lastName;
            }
            protected set
            {
                SetIfNotEmpty(out _lastName, value);
            }
        }

        public override string DisplayName
        {
            get
            {
                if (_displayNameCached == null)
                {
                    _displayNameCached = GenerateDisplayName();
                }

                return _displayNameCached;
            }
        }

        private string GenerateDisplayName()
        {
            List<string> nameParts = new List<string>(3);
            if (this.FirstName != null)
            {
                nameParts.Add(this.FirstName);
            }
            if (this.MiddleName != null)
            {
                nameParts.Add(this.MiddleName);
            }
            if (this.LastName != null)
            {
                nameParts.Add(this.LastName);
            }

            string nameJoined = string.Join(" ", nameParts);

            if (string.IsNullOrWhiteSpace(nameJoined))
            {
                return "Unknown Sender"; ;
            }

            return nameJoined;
        }

        private void SetIfNotEmpty(out string toSet, string newValue)
        {
            if (!string.IsNullOrEmpty(newValue))
            {
                toSet = newValue;
            }
            else
            {
                toSet = null;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

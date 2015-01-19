using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib;
using AllYourTextsLib.Framework;

namespace AllYourTextsTest
{
    public class MockContact : Contact
    {
        public const int MockContactId = 42;

        public MockContact(string firstName, string lastName)
            : this(firstName, null, lastName, null)
        {
            ;
        }

        public MockContact(string firstName, string lastName, IPhoneNumber phoneNumber)
            :this(firstName, null, lastName, phoneNumber)
        {
            ;
        }

        public MockContact(string firstName, string middleName, string lastName)
            :this(firstName, middleName, lastName, null)
        {
            ;
        }

        public MockContact(string firstName, string middleName, string lastName, IPhoneNumber phoneNumber)
            : base(MockContactId, firstName, middleName, lastName, phoneNumber)
        {
            ;
        }
    }
}

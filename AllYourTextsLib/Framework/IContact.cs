using System.Collections.Generic;

namespace AllYourTextsLib.Framework
{
    public interface IContact
    {
        long ContactId { get; }
        List<IPhoneNumber> PhoneNumbers { get; }
        string FirstName { get; }
        string MiddleName { get; }
        string LastName { get; }
        string DisplayName { get; }
    }
}

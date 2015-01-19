using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib.DataReader
{

    /// <summary>
    /// Creates an iterable series of Contact objects, given an interface to a contact database.
    /// </summary>
    public class ContactReader : DatabaseParserBase<IContact>
    {
        private const uint PhoneNumberPropertyId = 3;
        private const uint EmailAddressPropertyId = 4;

        protected override string DataQuery
        {
            get
            {
                return string.Format(
                    @"
SELECT
    ABPerson.ROWID,
    ABPerson.first,
    ABPerson.middle,
    ABPerson.last,
    ABMultiValue.value,
    ABMultiValue.label
FROM
    ABPerson,
    ABMultiValue
WHERE
    ABMultiValue.record_id=ABPerson.ROWID AND
    (
        ABMultiValue.property={0} OR
        ABMultiValue.property={1}
    )",
      PhoneNumberPropertyId.ToString(),
      EmailAddressPropertyId.ToString());
            }
        }

        protected override string DataCountQuery
        {
            get
            {
                return string.Format(@"
SELECT
    COUNT(*) 
FROM
    ABPerson,
    ABMultiValue
WHERE
    ABMultiValue.record_id=ABPerson.ROWID AND
    (
        ABMultiValue.property={0} OR
        ABMultiValue.property={1}
    )",
      PhoneNumberPropertyId.ToString(),
      EmailAddressPropertyId.ToString());
            }
        }
        
        private enum ValueIndex
        {
            RecordId = 0,
            FirstName,
            MiddleName,
            LastName,
            PhoneNumber,
            Label
        };

        protected override IContact ParseItemFromDatabase(IDatabaseReader databaseReader)
        {
            long recordId = databaseReader.GetInt64((int)ValueIndex.RecordId);

            string firstName = databaseReader.GetString((int)ValueIndex.FirstName);
            string middleName = databaseReader.GetString((int)ValueIndex.MiddleName);
            string lastName = databaseReader.GetString((int)ValueIndex.LastName);

            string phoneNumberValue = databaseReader.GetString((int)ValueIndex.PhoneNumber);

            if (string.IsNullOrEmpty(phoneNumberValue))
            {
                return null;
            }

            IPhoneNumber phoneNumber = new PhoneNumber(phoneNumberValue);

            return new Contact(recordId, firstName, middleName, lastName, phoneNumber);
        }
    }
}

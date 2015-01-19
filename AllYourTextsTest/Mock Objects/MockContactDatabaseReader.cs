using System;
using AllYourTextsLib.DataReader;

namespace AllYourTextsTest
{
    public class ContactDatabaseRow
    {
        public long RecordId { get; private set; }
        public string FirstName { get; private set; }
        public string MiddleName { get; private set; }
        public string LastName { get; private set; }
        public string PhoneNumber { get; private set; }

        public ContactDatabaseRow(long recordId, string firstName, string middleName, string lastName, string phoneNumber)
        {
            this.RecordId = recordId;
            this.FirstName = firstName;
            this.MiddleName = middleName;
            this.LastName = lastName;
            this.PhoneNumber = phoneNumber;
        }
    }

    public class MockContactDatabaseReader : MockDatabase<ContactDatabaseRow>
    {
        public override string GetString(int index)
        {
            ContactDatabaseRow currentRow = GetCurrentRow();

            if (index == ContactReader_Accessor.ValueIndex.FirstName.value__)
            {
                return currentRow.FirstName;
            }
            else if (index == ContactReader_Accessor.ValueIndex.MiddleName.value__)
            {
                return currentRow.MiddleName;
            }
            else if (index == ContactReader_Accessor.ValueIndex.LastName.value__)
            {
                return currentRow.LastName;
            }
            else if (index == ContactReader_Accessor.ValueIndex.PhoneNumber.value__)
            {
                return currentRow.PhoneNumber;
            }
            else
            {
                throw new ArgumentException("Invalid column number.");
            }
        }

        public override long GetInt64(int index)
        {
            ContactDatabaseRow currentRow = GetCurrentRow();

            if (index == ContactReader_Accessor.ValueIndex.RecordId.value__)
            {
                return currentRow.RecordId;
            }
            else
            {
                throw new ArgumentException("Invalid column number.");
            }
        }

        public void AddRow(uint recordId, string firstName, string middleName, string lastName, string phoneNumber, uint label)
        {
            ContactDatabaseRow row = new ContactDatabaseRow(recordId, firstName, middleName, lastName, phoneNumber);
            AddRow(row);
        }
    }
}

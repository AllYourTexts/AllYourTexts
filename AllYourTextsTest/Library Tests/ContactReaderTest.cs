using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AllYourTextsLib;
using System.Collections.Generic;
using AllYourTextsLib.Framework;
using AllYourTextsLib.DataReader;

namespace AllYourTextsTest
{
    /// <summary>
    ///This is a test class for ContactReaderTest and is intended
    ///to contain all ContactReaderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ContactReaderTest
    {

        public void VerifyDatabaseRowsMatchContacts(List<ContactDatabaseRow> inputRows, List<IContact> contactsExpected)
        {
            MockContactDatabaseReader mockReader = new MockContactDatabaseReader();
            foreach (ContactDatabaseRow row in inputRows)
            {
                mockReader.AddRow(row);
            }
            ContactReader contactReader = new ContactReader();
            contactReader.ParseDatabase(mockReader);
            List<IContact> contactsActual = new List<IContact>(contactReader);
            Assert.AreEqual(contactsExpected.Count, contactsActual.Count);
            foreach (IContact contactExpected in contactsExpected)
            {
                Assert.IsTrue(contactsActual.Contains(contactExpected));
            }
        }

        public void VerifyDatabaseRowsMatchSingleContact(List<ContactDatabaseRow> inputRows, IContact contactExpected)
        {
            List<IContact> contacts = new List<IContact>();
            contacts.Add(contactExpected);
            VerifyDatabaseRowsMatchContacts(inputRows, contacts);
        }

        public void VerifySingleDatabaseRowMatchesContact(long recordId, string firstName, string middleName, string lastName, string phoneNumber, Contact contactExpected)
        {
            List<ContactDatabaseRow> rows = new List<ContactDatabaseRow>();
            rows.Add(new ContactDatabaseRow(recordId, firstName, middleName, lastName, phoneNumber));
            VerifyDatabaseRowsMatchSingleContact(rows, contactExpected);
        }

        [TestMethod()]
        public void ReadSingleNumberContact()
        {
            PhoneNumber cellNumber = new PhoneNumber("801-994-0934");
            Contact contact = new Contact(MockContact.MockContactId, "James", null, "Incandenza", cellNumber);
            VerifySingleDatabaseRowMatchesContact(contact.ContactId,
                                                  contact.FirstName,
                                                  contact.MiddleName,
                                                  contact.LastName,
                                                  cellNumber.Number,
                                                  contact);
        }

        private ContactReader GetContactReaderFromSingleDatabaseRow(ContactDatabaseRow row)
        {
            MockContactDatabaseReader mockReader = new MockContactDatabaseReader();
            mockReader.AddRow(row);
            ContactReader contactReader = new ContactReader();
            contactReader.ParseDatabase(mockReader);

            return contactReader;
        }

        private IContact GetContactFromSingleDatabaseRow(ContactDatabaseRow row)
        {
            ContactReader contactReader = GetContactReaderFromSingleDatabaseRow(row);
            return new List<IContact>(contactReader)[0];
        }

        [TestMethod()]
        public void EmptyNameTest()
        {
            IContact contact = GetContactFromSingleDatabaseRow(new ContactDatabaseRow(1, "", "", "", "063-234-1853"));
            Assert.IsNull(contact.FirstName);
            Assert.IsNull(contact.MiddleName);
            Assert.IsNull(contact.LastName);
        }

        [TestMethod()]
        public void EmptyNumberTest()
        {
            ContactReader contactReader = GetContactReaderFromSingleDatabaseRow(new ContactDatabaseRow(1, "Larry", "Barry", "Sanders", ""));
            List<IContact> contacts = new List<IContact>(contactReader);
            Assert.AreEqual(0, contacts.Count);
        }
    }
}

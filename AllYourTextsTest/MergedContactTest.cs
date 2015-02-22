using AllYourTextsLib;
using AllYourTextsLib.Framework;
using DummyData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AllYourTextsTest
{
    
    
    /// <summary>
    ///This is a test class for MergedContactTest and is intended
    ///to contain all MergedContactTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MergedContactTest
    {
        [TestMethod()]
        public void TestEquals()
        {
            IContact davolaCell = DummyConversationDataGenerator.GetContact(DummyPhoneNumberId.DavolaCell);
            IContact davolaiPhone = DummyConversationDataGenerator.GetContact(DummyPhoneNumberId.DavolaiPhone);

            MergedContact mergedContact = new MergedContact(davolaCell, davolaiPhone);
            MergedContact mergedContactCopy = new MergedContact(davolaCell, davolaiPhone);
            MergedContact mergedContactReverseOrder = new MergedContact(davolaiPhone, davolaCell);

            Assert.AreEqual(mergedContact, mergedContact);
            Assert.AreEqual(mergedContact, mergedContactCopy);
            Assert.AreEqual(mergedContact, mergedContactReverseOrder);
        }

        [TestMethod()]
        public void DuplicateNumberTest()
        {
            IPhoneNumber duplicatedNumber = new PhoneNumber("646-123-4567");
            IPhoneNumber duplicatedNumberCopy = new PhoneNumber("646-123-4567");

            IContact contact = new MockContact("Johnny", "DoubleNumber", duplicatedNumber);
            IContact contactDuplicate = new MockContact("Johnny", "DoubleNumber", duplicatedNumberCopy);
            MergedContact mergedContact = new MergedContact(contact, contactDuplicate);

            Assert.AreEqual(1, mergedContact.PhoneNumbers.Count);
        }

        [TestMethod()]
        public void DuplicateNumberChangedTypeTest()
        {
            IPhoneNumber duplicatedNumber = new PhoneNumber("646-123-4567");
            IPhoneNumber duplicatedNumberCell = new PhoneNumber("646-123-4567");

            IContact contact = new MockContact("Johnny", "DoubleNumber", duplicatedNumber);
            IContact contactDuplicate = new MockContact("Johnny", "DoubleNumber", duplicatedNumberCell);
            MergedContact mergedContact = new MergedContact(contact, contactDuplicate);

            Assert.AreEqual(1, mergedContact.PhoneNumbers.Count);
        }

        [TestMethod()]
        public void CreateTripleNumberContactTest()
        {
            const string firstName = "Ben";
            const string lastName = "Stiller";

            IPhoneNumber phoneNumberA = new PhoneNumber("212-845-0923");
            IPhoneNumber phoneNumberB = new PhoneNumber("212-845-0924");
            IPhoneNumber phoneNumberC = new PhoneNumber("212-845-0925");

            IContact contactA = new MockContact(firstName, lastName, phoneNumberA);
            IContact contactB = new MockContact(firstName, lastName, phoneNumberB);
            IContact contactC = new MockContact(firstName, lastName, phoneNumberC);

            MergedContact contactMergedAB = new MergedContact(contactA, contactB);
            Assert.AreEqual(2, contactMergedAB.PhoneNumbers.Count);
            Assert.IsTrue(contactMergedAB.PhoneNumbers.Contains(phoneNumberA));
            Assert.IsTrue(contactMergedAB.PhoneNumbers.Contains(phoneNumberB));

            MergedContact contactMergedABC = new MergedContact(contactMergedAB, contactC);
            Assert.AreEqual(3, contactMergedABC.PhoneNumbers.Count);
            Assert.IsTrue(contactMergedABC.PhoneNumbers.Contains(phoneNumberA));
            Assert.IsTrue(contactMergedABC.PhoneNumbers.Contains(phoneNumberB));
            Assert.IsTrue(contactMergedABC.PhoneNumbers.Contains(phoneNumberC));
        }
    }
}

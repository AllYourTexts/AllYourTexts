using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using AllYourTextsLib;
using AllYourTextsLib.Framework;
using System.Linq;

namespace AllYourTextsTest
{
    
    /// <summary>
    ///This is a test class for ContactTest and is intended
    ///to contain all ContactTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ContactTest
    {

        [TestMethod()]
        public void TestEquals()
        {
            PhoneNumber numberACell = new PhoneNumber("212-483-0942");

            Contact contactA = new Contact(15, "Randy", null, "Quaid", numberACell);
            Contact contactACopy = new Contact(15, "Randy", null, "Quaid", numberACell);

            PhoneNumber seinfeldHomeNumber = new PhoneNumber("914-832-0093");
            
            Contact seinfeldContact = new Contact(16, "Jerry", null, "Seinfeld", seinfeldHomeNumber);

            Assert.AreEqual(contactA, contactA, "A Contact instance should be equal to itself.");
            Assert.AreEqual(contactACopy, contactA, "Exact copies of Contacts should be equal.");
            Assert.AreNotEqual(seinfeldContact, contactA, "Different Contacts should not be equal to each other.");
        }
    }
}

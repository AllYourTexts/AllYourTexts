using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib
{
    public class ContactComparer
    {
        public static int CompareAlphabetically(IContact contactA, IContact contactB)
        {
            int unknownCompare = CompareForUnknownContact(contactA, contactB);
            if (unknownCompare != 0)
            {
                return unknownCompare;
            }

            List<string> contactAParts = GetNamePartsFromContact(contactA);
            List<string> contactBParts = GetNamePartsFromContact(contactB);

            int compareResult;

            while ((contactAParts.Count > 0) && (contactBParts.Count > 0))
            {
                string aPart = contactAParts[0];
                string bPart = contactBParts[0];

                compareResult = string.Compare(aPart, bPart);
                if (compareResult != 0)
                {
                    return compareResult;
                }

                contactAParts.RemoveAt(0);
                contactBParts.RemoveAt(0);
            }

            if (contactAParts.Count != contactBParts.Count)
            {
                return contactAParts.Count - contactBParts.Count;
            }

            return string.Compare(contactA.PhoneNumbers[0].Number, contactB.PhoneNumbers[0].Number);
        }

        private static int CompareForUnknownContact(IContact contactA, IContact contactB)
        {
            if ((contactA.ContactId == Contact.UnknownContactId) && (contactB.ContactId != Contact.UnknownContactId))
            {
                return 1;
            }
            else if ((contactA.ContactId != Contact.UnknownContactId) && (contactB.ContactId == Contact.UnknownContactId))
            {
                return -1;
            }

            return 0;
        }

        private static List<string> GetNamePartsFromContact(IContact contact)
        {
            string[] nameParts;

            if (contact.FirstName != null)
            {
                nameParts = new string[] { contact.FirstName, contact.LastName, contact.MiddleName };
            }
            else
            {
                nameParts = new string[] { contact.MiddleName, contact.LastName };
            }

            List<string> nullStrippedNameParts = new List<string>(3);
            foreach (string namePart in nameParts)
            {
                if (namePart != null)
                {
                    nullStrippedNameParts.Add(namePart);
                }
            }

            return nullStrippedNameParts;
        }
    }
}

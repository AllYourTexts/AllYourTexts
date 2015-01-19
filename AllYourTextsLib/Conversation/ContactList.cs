using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib.Conversation
{
    public class ContactList : IContactList
    {
        private List<IContact> _contacts;
        
        public ContactList()
        {
            _contacts = new List<IContact>();
        }

        public ContactList(IEnumerable<IContact> contacts)
        {
            _contacts = new List<IContact>(contacts);
        }

        public int IndexOf(IContact item)
        {
            return _contacts.IndexOf(item);
        }

        public void Insert(int index, IContact item)
        {
            _contacts.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _contacts.RemoveAt(index);
        }

        public IContact this[int index]
        {
            get
            {
                return _contacts[index];
            }
            set
            {
                _contacts[index] = value;
            }
        }

        public void Add(IContact item)
        {
            _contacts.Add(item);
        }

        public void Clear()
        {
            _contacts.Clear();
        }

        public bool Contains(IContact item)
        {
            return _contacts.Contains(item);
        }

        public void CopyTo(IContact[] array, int arrayIndex)
        {
            _contacts.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _contacts.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(IContact item)
        {
            return _contacts.Remove(item);
        }

        public IEnumerator<IContact> GetEnumerator()
        {
            return _contacts.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _contacts.GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            return Equals((IContactList)obj);
        }

        public bool Equals(IContactList otherContactList)
        {
            return Enumerable.SequenceEqual(_contacts, otherContactList);
        }

        public override int GetHashCode()
        {
            int result = 0;
            unchecked
            {
                foreach (IContact contact in _contacts)
                {
                    result += contact.GetHashCode();
                }
            }
            return result;
        }

        public void Sort(Comparison<IContact> comparer)
        {
            _contacts.Sort(comparer);
        }
    }
}

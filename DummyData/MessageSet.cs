using System.Collections.Generic;
using AllYourTextsLib;

namespace DummyData
{
    class MessageSet : IList<TextMessage>
    {
        public List<TextMessage> MessageList;

        public MessageSet(List<TextMessage> messageList)
        {
            MessageList = messageList;
        }

        public int IndexOf(TextMessage item)
        {
            return MessageList.IndexOf(item);
        }

        public void Insert(int index, TextMessage item)
        {
            MessageList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            MessageList.RemoveAt(index);
        }

        public TextMessage this[int index]
        {
            get
            {
                return MessageList[index];
            }
            set
            {
                MessageList[index] = value;
            }
        }

        public void Add(TextMessage item)
        {
            MessageList.Add(item);
        }

        public void Clear()
        {
            MessageList.Clear();
        }

        public bool Contains(TextMessage item)
        {
            return MessageList.Contains(item);
        }

        public void CopyTo(TextMessage[] array, int arrayIndex)
        {
            MessageList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return MessageList.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(TextMessage item)
        {
            return MessageList.Remove(item);
        }

        public IEnumerator<TextMessage> GetEnumerator()
        {
            return MessageList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib.DataReader
{
    public abstract class DatabaseParserBase<T> : IEnumerable<T>
    {
        public int ItemCountEstimate { get; private set; }
        protected abstract string DataQuery { get; }
        protected abstract string DataCountQuery { get; }
        private List<T> _ParsedContents;

        public DatabaseParserBase()
        {
            _ParsedContents = new List<T>();
        }

        public void ParseDatabase(IDatabaseReader databaseReader)
        {
            ItemCountEstimate = GetItemCountEstimate(databaseReader);
            _ParsedContents = ParseItemsFromDatabase(databaseReader);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T item in _ParsedContents)
            {
                yield return item;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private int GetItemCountEstimate(IDatabaseReader databaseReader)
        {
            int countEstimate;

            databaseReader.ExecuteQuery(DataCountQuery);
            databaseReader.Read();
            countEstimate = databaseReader.GetInt32(0);

            return countEstimate;
        }

        private void ExecuteDataQuery(IDatabaseReader databaseReader)
        {
            databaseReader.ExecuteQuery(DataQuery);
        }

        protected abstract T ParseItemFromDatabase(IDatabaseReader databaseReader);

        protected virtual List<T> ParseItemsFromDatabase(IDatabaseReader databaseReader)
        {
            List<T> parsedItems = new List<T>();

            ExecuteDataQuery(databaseReader);
            while (databaseReader.Read())
            {
                T parsedItem = ParseItemFromDatabase(databaseReader);

                if (parsedItem != null)
                {
                    parsedItems.Add(parsedItem);
                }
            }

            return parsedItems;
        }
    }
}

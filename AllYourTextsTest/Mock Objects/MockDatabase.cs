using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;
using System.Diagnostics;

namespace AllYourTextsTest
{

    public abstract class MockDatabase<T> : IDatabaseReader
    {
        protected enum LastQueryType
        {
            None = 0,
            Count,
            Select
        }

        private List<T> _rows;
        private int _currentRow;
        protected LastQueryType _lastQueryType;

        public MockDatabase()
        {
            _rows = new List<T>();
            _currentRow = -1;
            _lastQueryType = LastQueryType.None;
        }

        public void ExecuteQuery(string query)
        {
            if (query.Contains("COUNT(*)"))
            {
                _lastQueryType = LastQueryType.Count;
            }
            else
            {
                _lastQueryType = LastQueryType.Select;
            }
        }

        public bool Read()
        {
            if (_lastQueryType == LastQueryType.Count)
            {
                return true;
            }

            if (this.HasRows)
            {
                IncrementRow();
                return true;
            }

            return false;
        }

        public bool HasRows
        {
            get
            {
                return _currentRow < (GetRowCount() - 1);
            }
        }

        public void AddRow(T row)
        {
            _rows.Add(row);
        }

        protected T GetCurrentRow()
        {
            return _rows[_currentRow];
        }

        public abstract string GetString(int index);

        public virtual byte[] GetBlob(int index)
        {
            throw new NotImplementedException();
        }

        public int GetInt32(int index)
        {

            //
            // N.B. Being a little lazy here because the only call to GetInt32 will be a count
            //  query, but if there are other calls in the future, this function will need to
            //  be revised.
            //

            Debug.Assert(_lastQueryType == LastQueryType.Count);
            return GetRowCount();
        }

        public abstract long GetInt64(int index);

        protected int GetRowCount()
        {
            return _rows.Count;
        }

        private void IncrementRow()
        {
            _currentRow++;
        }

        public void Dispose()
        {
            ;
        }
    }
}

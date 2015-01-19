using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SQLite;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib.DataReader
{
    public class DatabaseReader : IDatabaseReader
    {
        private SQLiteConnection _databaseConnection;
        private SQLiteDataReader _databaseReader;

        public DatabaseReader(string databaseFilename)
        {
            FileAttributes attr = File.GetAttributes(databaseFilename);
            if (attr.ToString() == "")
            {
                _databaseConnection = null;
            }

            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder();
            builder.DataSource = databaseFilename;
            builder.ReadOnly = true;

            _databaseConnection = new SQLiteConnection(builder.ConnectionString);

            try
            {
                _databaseConnection.Open();
            }
            catch (SQLiteException ex)
            {
                if (ex.ErrorCode == SQLiteErrorCode.NotADatabase)
                {
                    throw new UnreadableDatabaseFileException(databaseFilename, ex);
                }
                else
                {
                    throw;
                }
            }

        }

        ~DatabaseReader()
        {
            Dispose();
        }

        public void ExecuteQuery(string query)
        {
            DisposeReader();
            _databaseReader = ExecuteQueryInternal(query);
        }

        private SQLiteDataReader ExecuteQueryInternal(string query)
        {
            SQLiteDataReader dataReader = null;
            try
            {
                SQLiteCommand cmd = _databaseConnection.CreateCommand();
                cmd.CommandText = query;

                dataReader = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException(query, ex);
            }

            return dataReader;
        }

        public bool Read()
        {
            return _databaseReader.Read();
        }

        public bool HasRows
        {
            get
            {
                return _databaseReader.HasRows;
            }
        }

        public string GetString(int index)
        {
            object stringValue = _databaseReader.GetValue(index);

            return stringValue as string;
        }

        public int GetInt32(int index)
        {
            return _databaseReader.GetInt32(index);
        }

        public long GetInt64(int index)
        {
            return _databaseReader.GetInt64(index);
        }

        public byte[] GetBlob(int index)
        {
            long blobSize = _databaseReader.GetBytes(index, 0, null, 0, 0);
            byte[] blobData = new byte[blobSize];

            const int bufferSize = 1024;
            long bytesRead = 0;
            int currentPosition = 0;

            while (bytesRead < blobSize)
            {
                bytesRead += _databaseReader.GetBytes(index, currentPosition, blobData, currentPosition, bufferSize);
                currentPosition += bufferSize;
            }

            return blobData;
        }

        public void Dispose()
        {
            DisposeReader();
            DisposeConnection();
        }

        private void DisposeReader()
        {
            if (_databaseReader != null)
            {
                _databaseReader.Dispose();
                _databaseReader = null;
            }
        }

        private void DisposeConnection()
        {
            if (_databaseConnection != null)
            {
                _databaseConnection.Dispose();
                _databaseConnection = null;
            }
        }
    }
}

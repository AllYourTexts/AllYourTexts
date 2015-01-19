using System;

namespace AllYourTextsLib.Framework
{
    public interface IDatabaseReader : IDisposable
    {
        void ExecuteQuery(string query);

        bool Read();

        string GetString(int index);

        Int32 GetInt32(int index);

        Int64 GetInt64(int index);

        byte[] GetBlob(int index);

        bool HasRows { get; }
    }
}

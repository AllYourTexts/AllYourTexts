using System;
using AllYourTextsLib.DataReader;

namespace AllYourTextsTest
{
    //
    // Specialized mock database reader that throws an exception when attempting to read the phone number label
    //

    public class MockExceptionThrowingContactDatabaseReader : MockContactDatabaseReader
    {
        public override long GetInt64(int index)
        {
            if (index == ContactReader_Accessor.ValueIndex.Label.value__)
            {
                throw new InvalidCastException();
            }

            return base.GetInt64(index);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsLib.DataReader
{
    public class ByteConverter
    {
        public static byte ParseByte(byte[] encodedData, ref int dataPosition)
        {
            byte value = encodedData[dataPosition++];

            return value;
        }

        public static UInt16 ParseUInt16(byte[] encodedData, ref int dataPosition)
        {
            UInt16 value = ParseByte(encodedData, ref dataPosition);
            value <<= 8;
            value += ParseByte(encodedData, ref dataPosition);

            return value;
        }

        public static UInt32 ParseUInt32(byte[] encodedData, ref int dataPosition)
        {
            UInt32 value = ParseUInt16(encodedData, ref dataPosition);
            value <<= 16;
            value += ParseUInt16(encodedData, ref dataPosition);

            return value;
        }

        public static UInt64 ParseUInt64(byte[] encodedData, ref int dataPosition)
        {
            UInt64 value = ParseUInt32(encodedData, ref dataPosition);
            value <<= 32;
            value += ParseUInt32(encodedData, ref dataPosition);

            return value;
        }
    }
}

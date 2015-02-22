using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsLib.DataReader
{
    public class BPListParser
    {
        private enum PListObjectType
        {
            Unknown = 0,
            Null,
            BinaryFalse,
            BinaryTrue,
            Fill,
            Integer,
            RealNumber,
            Date,
            Data,
            AsciiString,
            UnicodeString,
            Uid,
            Array,
            Dictionary
        }

        private class BPListTrailer
        {
            public byte[] UnusedFields { get; set; }
            public byte SortVersion { get; set; }
            public byte OffsetIntSize { get; set; }
            public byte ObjectsRefSize { get; set; }
            public ulong NumObjects { get; set; }
            public ulong TopObject { get; set; }
            public ulong OffsetTableOffset { get; set; }
            public BPListTrailer()
            {
                this.UnusedFields = new byte[5];
            }

            public override bool Equals(object obj)
            {
                return this.Equals((BPListTrailer)obj);   
            }

            public bool Equals(BPListTrailer other)
            {
                if (this.SortVersion != other.SortVersion)
                {
                    return false;
                }

                if (this.OffsetIntSize != other.OffsetIntSize)
                {
                    return false;
                }

                if (this.ObjectsRefSize != other.ObjectsRefSize)
                {
                    return false;
                }

                if (this.NumObjects != other.NumObjects)
                {
                    return false;
                }

                if (this.TopObject != other.TopObject)
                {
                    return false;
                }

                if (this.OffsetTableOffset != other.OffsetTableOffset)
                {
                    return false;
                }

                return true;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        public static string[] ParseBPListStrings(byte[] plistData)
        {
            BPListTrailer trailer = ParseTrailer(plistData);

            int dataPosition = (int)trailer.OffsetTableOffset;
            byte[] objectOffsets = new byte[trailer.NumObjects];
            for (int i = 0; i < objectOffsets.Length; i++)
            {
                objectOffsets[i] = ByteConverter.ParseByte(plistData, ref dataPosition);
            }

            List<string> bpListStrings = new List<string>();

            for (int i = 0; i < objectOffsets.Length; i++)
            {
                dataPosition = objectOffsets[i];
                PListObjectType objectType = GetPListObjectType(plistData[dataPosition]);
                if (objectType == PListObjectType.AsciiString)
                {
                    string parsedString = ParseAsciiString(plistData, dataPosition);
                    bpListStrings.Add(parsedString);
                }
            }

            return bpListStrings.ToArray();
        }

        private static BPListTrailer ParseTrailer(byte[] plistData)
        {
            const int TrailerSize = 32;
            int trailerStartOffset = plistData.Length - TrailerSize;

            int dataPosition = trailerStartOffset;
            BPListTrailer trailer = new BPListTrailer();
            trailer.UnusedFields[0] = ByteConverter.ParseByte(plistData, ref dataPosition);
            trailer.UnusedFields[1] = ByteConverter.ParseByte(plistData, ref dataPosition);
            trailer.UnusedFields[2] = ByteConverter.ParseByte(plistData, ref dataPosition);
            trailer.UnusedFields[3] = ByteConverter.ParseByte(plistData, ref dataPosition);
            trailer.UnusedFields[4] = ByteConverter.ParseByte(plistData, ref dataPosition);

            trailer.SortVersion = ByteConverter.ParseByte(plistData, ref dataPosition);
            trailer.OffsetIntSize = ByteConverter.ParseByte(plistData, ref dataPosition);
            trailer.ObjectsRefSize = ByteConverter.ParseByte(plistData, ref dataPosition);

            trailer.NumObjects = ByteConverter.ParseUInt64(plistData, ref dataPosition);
            trailer.TopObject = ByteConverter.ParseUInt64(plistData, ref dataPosition);
            trailer.OffsetTableOffset = ByteConverter.ParseUInt64(plistData, ref dataPosition);

            return trailer;
        }

        private static PListObjectType GetPListObjectType(byte encodedObjectType)
        {
            if (MatchesBinaryValue(encodedObjectType, "00000000"))
            {
                return PListObjectType.Null;
            }
            else if (MatchesBinaryValue(encodedObjectType, "00001000"))
            {
                return PListObjectType.BinaryFalse;
            }
            else if (MatchesBinaryValue(encodedObjectType, "00001001"))
            {
                return PListObjectType.BinaryTrue;
            }
            else if (MatchesBinaryValue(encodedObjectType, "00001111"))
            {
                return PListObjectType.Fill;
            }
            else if (MatchesBinaryValue(encodedObjectType, "00110011"))
            {
                return PListObjectType.Date;
            }

            byte prefix = (byte)(encodedObjectType >> 4);
            if (MatchesBinaryValue(prefix, "0001"))
            {
                return PListObjectType.Integer;
            }
            else if (MatchesBinaryValue(prefix, "0010"))
            {
                return PListObjectType.RealNumber;
            }
            else if (MatchesBinaryValue(prefix, "0100"))
            {
                return PListObjectType.Data;
            }
            else if (MatchesBinaryValue(prefix, "0101"))
            {
                return PListObjectType.AsciiString;
            }
            else if (MatchesBinaryValue(prefix, "0110"))
            {
                return PListObjectType.UnicodeString;
            }
            else if (MatchesBinaryValue(prefix, "1000"))
            {
                return PListObjectType.Uid;
            }
            else if (MatchesBinaryValue(prefix, "1010"))
            {
                return PListObjectType.Array;
            }
            else if (MatchesBinaryValue(prefix, "1101"))
            {
                return PListObjectType.Dictionary;
            }

            return PListObjectType.Unknown;
        }

        private static bool MatchesBinaryValue(byte value, string binaryValueAsString)
        {
            return (value == Convert.ToInt32(binaryValueAsString, 2));
        }

        private static UInt64 ParseInt(byte[] bpListData, ref int intOffset)
        {
            //
            // The high 4 bits of the first byte in the int should be the int marker, 0001
            //

            byte markerBits = (byte)(bpListData[intOffset] & 0xF0);
            if (!MatchesBinaryValue(markerBits, "00010000"))
            {
                throw new FormatException("BPList int value begins with incorrect format specifier: " + bpListData[0]);
            }

            //
            // The low 4 bits of the first byte in the int object represent the power of 2 for number of bytes.
            //

            int lengthExponent = (bpListData[intOffset] & 0x0F);
            intOffset++;

            int byteCount = (int)Math.Pow(2, lengthExponent);
            switch (byteCount)
            {
                case 1:
                    return ByteConverter.ParseByte(bpListData, ref intOffset);
                case 2:
                    return ByteConverter.ParseUInt16(bpListData, ref intOffset);
                case 4:
                    return ByteConverter.ParseUInt32(bpListData, ref intOffset);
                case 8:
                    return ByteConverter.ParseUInt64(bpListData, ref intOffset);
                default:
                    throw new NotImplementedException("Unsupported integer size: " + byteCount.ToString());
            }
        }

        private static string ParseAsciiString(byte[] bpListData, int stringOffset)
        {
            //
            // The low 4 bits of the first byte in the string object contain the string length.
            //

            int stringLength = (bpListData[stringOffset] & 0x0F);

            int stringStartOffset;
            if (stringLength == Convert.ToInt32("1111", 2))
            {
                int intOffset = stringOffset + 1;
                UInt64 stringLength64 = ParseInt(bpListData, ref intOffset);
                stringLength = (int)stringLength64;
                stringStartOffset = intOffset;
            }
            else
            {
                stringStartOffset = stringOffset + 1;
            }

            byte[] stringData = new byte[stringLength];
            Array.Copy(bpListData, stringStartOffset, stringData, 0, stringLength);

            return ASCIIEncoding.ASCII.GetString(stringData);
        }
    }
}

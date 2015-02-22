using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AllYourTextsLib.Framework;

namespace AllYourTextsLib.DataReader
{
    public class MbdbParser
    {
        private static readonly byte[] MbdbSignature = new byte[] { 0x6D, 0x62, 0x64, 0x62, 0x05, 0x00 };

        public static MbdbRecord[] ParseMbdbData(byte[] mbdbData)
        {
            if (mbdbData.Length < MbdbSignature.Length)
            {
                throw new MbdbDataInvalidException("MBDB data is too short to be valid.");
            }

            int dataPosition = 0;

            while (dataPosition < MbdbSignature.Length)
            {
                if (mbdbData[dataPosition] != MbdbSignature[dataPosition])
                {
                    throw new MbdbDataInvalidException("MBDB data does not have valid data signature.");
                }
                dataPosition++;
            }

            List<MbdbRecord> records = new List<MbdbRecord>();
            while (dataPosition < mbdbData.Length)
            {
                try
                {
                    MbdbRecord nextRecord = ParseMbdbRecord(mbdbData, ref dataPosition);
                    records.Add(nextRecord);
                }
                catch (Exception ex)
                {
                    throw new MbdbDataInvalidException("Failed to parse MBDB data", ex);
                }
            }

            return records.ToArray(); ;
        }

        private static MbdbRecord ParseMbdbRecord(byte[] mbdbData, ref int dataPosition)
        {
            MbdbRecord record = new MbdbRecord();

            record.Domain = ParseString(mbdbData, ref dataPosition);
            record.Path = ParseString(mbdbData, ref dataPosition);
            record.LinkTarget = ParseString(mbdbData, ref dataPosition);
            record.DataHash = ParseString(mbdbData, ref dataPosition);
            record.UnknownField1 = ParseString(mbdbData, ref dataPosition);
            record.Mode = ByteConverter.ParseUInt16(mbdbData, ref dataPosition);
            record.UnknownField2 = ByteConverter.ParseUInt32(mbdbData, ref dataPosition);
            record.UnknownField3 = ByteConverter.ParseUInt32(mbdbData, ref dataPosition);
            record.UserId = ByteConverter.ParseUInt32(mbdbData, ref dataPosition);
            record.GroupId = ByteConverter.ParseUInt32(mbdbData, ref dataPosition);
            record.LastModificationTime = ByteConverter.ParseUInt32(mbdbData, ref dataPosition);
            record.LastAccessedTime = ByteConverter.ParseUInt32(mbdbData, ref dataPosition);
            record.CreationTime = ByteConverter.ParseUInt32(mbdbData, ref dataPosition);
            record.FileLength = ByteConverter.ParseUInt64(mbdbData, ref dataPosition);
            record.Flag = ByteConverter.ParseByte(mbdbData, ref dataPosition);

            byte propertyCount = ByteConverter.ParseByte(mbdbData, ref dataPosition);

            List<MbdbRecordProperty> recordProperties = new List<MbdbRecordProperty>();
            for (byte i = 0; i < propertyCount; i++)
            {
                MbdbRecordProperty nextRecordProperty = ParseRecordProperty(mbdbData, ref dataPosition);
                recordProperties.Add(nextRecordProperty);
            }
            record.Properties = recordProperties;

            return record;
        }

        private static MbdbRecordProperty ParseRecordProperty(byte[] mbdbData, ref int dataPosition)
        {
            string propertyName = ParseString(mbdbData, ref dataPosition);
            string propertyValue = ParseString(mbdbData, ref dataPosition);

            return new MbdbRecordProperty(propertyName, propertyValue);
        }

        private static string ParseString(byte[] mbdbData, ref int dataPosition)
        {
            UInt16 stringLength = ByteConverter.ParseUInt16(mbdbData, ref dataPosition);

            if (stringLength == 0 || stringLength == UInt16.MaxValue)
            {
                return null;
            }

            byte[] stringData = new byte[stringLength];
            Array.Copy(mbdbData, dataPosition, stringData, 0, stringLength);
            dataPosition += stringLength;

            return UTF8Encoding.UTF8.GetString(stringData);
        }
    }
}

using System;
using AllYourTextsLib.DataReader;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AllYourTextsTest
{
    
    
    /// <summary>
    ///This is a test class for ByteConverterTest and is intended
    ///to contain all ByteConverterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ByteConverterTest
    {
        private void VerifyParseUInt64(byte[] serializedData, int dataPosition, int dataPositionExpected, UInt64 valueExpected)
        {
            UInt64 valueActual = valueActual = ByteConverter.ParseUInt64(serializedData, ref dataPosition);
            Assert.AreEqual(dataPositionExpected, dataPosition);
            Assert.AreEqual(valueExpected, valueActual);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void ParseUInt64Test()
        {
            VerifyParseUInt64(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, 0, 8, UInt64.MaxValue);
            VerifyParseUInt64(new byte[] { 0, 0, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, 2, 10, UInt64.MaxValue);
            VerifyParseUInt64(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0x05 }, 0, 8, 5);
        }

        private void VerifyParseUInt32(byte[] serializedData, int dataPosition, int dataPositionExpected, UInt32 valueExpected)
        {
            UInt32 valueActual = valueActual = ByteConverter.ParseUInt32(serializedData, ref dataPosition);
            Assert.AreEqual(dataPositionExpected, dataPosition);
            Assert.AreEqual(valueExpected, valueActual);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void ParseUInt32Test()
        {
            VerifyParseUInt32(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, }, 0, 4, UInt32.MaxValue);
            VerifyParseUInt32(new byte[] { 0, 0, 0xFF, 0xFF, 0xFF, 0xFF }, 2, 6, UInt32.MaxValue);
            VerifyParseUInt32(new byte[] { 0, 0, 0, 0x05 }, 0, 4, 5);
        }

        private void VerifyParseUInt16(byte[] serializedData, int dataPosition, int dataPositionExpected, UInt16 valueExpected)
        {
            UInt16 valueActual = valueActual = ByteConverter.ParseUInt16(serializedData, ref dataPosition);
            Assert.AreEqual(dataPositionExpected, dataPosition);
            Assert.AreEqual(valueExpected, valueActual);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void ParseUInt16Test()
        {
            VerifyParseUInt16(new byte[] { 0xFF, 0xFF, }, 0, 2, UInt16.MaxValue);
            VerifyParseUInt16(new byte[] { 0, 0, 0xFF, 0xFF }, 2, 4, UInt16.MaxValue);
            VerifyParseUInt16(new byte[] { 0, 0x05 }, 0, 2, 5);
        }

        private void VerifyParseByte(byte[] serializedData, int dataPosition, int dataPositionExpected, byte valueExpected)
        {
            byte valueActual = valueActual = ByteConverter.ParseByte(serializedData, ref dataPosition);
            Assert.AreEqual(dataPositionExpected, dataPosition);
            Assert.AreEqual(valueExpected, valueActual);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void ParseByteTest()
        {
            VerifyParseByte(new byte[] { 0xFF, }, 0, 1, byte.MaxValue);
            VerifyParseByte(new byte[] { 0, 0, 0xFF }, 2, 3, byte.MaxValue);
            VerifyParseByte(new byte[] { 0x05 }, 0, 1, 5);
        }
    }
}

using System;
using System.Linq;
using AllYourTextsLib.DataReader;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AllYourTextsTest
{
    
    
    /// <summary>
    ///This is a test class for BPListParserTest and is intended
    ///to contain all BPListParserTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BPListParserTest
    {

        private readonly byte[] _pListData1 = new byte[] {0x62, 0x70, 0x6C, 0x69, 0x73, 0x74, 0x30, 0x30, 0xA2, 0x01, 0x02, 0x5C, 0x2B, 0x31, 0x35, 0x34, 0x30, 0x32, 0x33, 0x39,
                                                          0x32, 0x33, 0x30, 0x34, 0x5C, 0x2B, 0x31, 0x32, 0x30, 0x36, 0x38, 0x31, 0x38, 0x39, 0x32, 0x38, 0x38, 0x08, 0x0B, 0x18,
                                                          0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00,
                                                          0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x25};
        /// <summary>
        ///A test for PareBPList
        ///</summary>
        [TestMethod()]
        public void ParseBPListStringsTest()
        {
            string[] parsedStringsExpected = {"+15402392304", "+12068189288"};
            string[] parsedStringsActual = BPListParser.ParseBPListStrings(_pListData1);
            Assert.AreEqual(parsedStringsExpected.Length, parsedStringsActual.Length);
            Assert.IsTrue(Enumerable.SequenceEqual(parsedStringsExpected, parsedStringsActual));
        }

        /// <summary>
        ///A test for ParseTrailer
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void ParseTrailerTest()
        {
            BPListParser_Accessor.BPListTrailer expectedTrailer = new BPListParser_Accessor.BPListTrailer();
            expectedTrailer.NumObjects = 3;
            expectedTrailer.ObjectsRefSize = 1;
            expectedTrailer.OffsetIntSize = 1;
            expectedTrailer.OffsetTableOffset = 0x25;
            expectedTrailer.SortVersion = 0;
            expectedTrailer.TopObject = 0;
            expectedTrailer.UnusedFields = new byte[5];

            BPListParser_Accessor.BPListTrailer actualTrailer;
            actualTrailer = BPListParser_Accessor.ParseTrailer(_pListData1);
            Assert.AreEqual(expectedTrailer, actualTrailer);
        }

        /// <summary>
        ///A test for ParseAsciiString
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void ParseAsciiStringTest()
        {
            byte[] encodedString = { 0x5C, 0x2B, 0x31, 0x35, 0x34, 0x30, 0x32, 0x33, 0x39, 0x32, 0x33, 0x30, 0x34 };
            string parsedStringExpected = "+15402392304";
            string parsedStringActual = BPListParser_Accessor.ParseAsciiString(encodedString, 0);
            Assert.AreEqual(parsedStringExpected, parsedStringActual);
        }

        /// <summary>
        ///A test for ParseAsciiString
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void ParseAsciiLongStringTest()
        {
            byte[] encodedString = { 0x5F, 0x10, 0x27, 0x73, 0x75, 0x70, 0x65, 0x72, 0x64, 0x6f, 0x6F, 0x70, 0x65, 0x72, 0x61, 0x77, 0x65, 0x73, 0x6F,
                                     0x6D, 0x65, 0x67, 0x75, 0x79, 0x40, 0x61, 0x77, 0x65, 0x73, 0x6F, 0x6D, 0x65, 0x74, 0x6F, 0x77, 0x6E, 0x2E, 0x63,
                                     0x6F, 0x2E, 0x75, 0x6B };
            string parsedStringExpected = "superdooperawesomeguy@awesometown.co.uk"; //len=39
            string parsedStringActual = BPListParser_Accessor.ParseAsciiString(encodedString, 0);
            Assert.AreEqual(parsedStringExpected, parsedStringActual);
        }

        /// <summary>
        ///A test for ParseAsciiString
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void ParseAsciiLongLongStringTest()
        {
            byte[] encodedString = { 0x5F, 0x13, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x27,
                                     0x73, 0x75, 0x70, 0x65, 0x72, 0x64, 0x6f, 0x6F, 0x70, 0x65, 0x72, 0x61, 0x77, 0x65, 0x73, 0x6F, 0x6D, 0x65,
                                     0x67, 0x75, 0x79, 0x40, 0x61, 0x77, 0x65, 0x73, 0x6F, 0x6D, 0x65, 0x74, 0x6F, 0x77, 0x6E, 0x2E, 0x63, 0x6F,
                                     0x2E, 0x75, 0x6B };
            string parsedStringExpected = "superdooperawesomeguy@awesometown.co.uk"; //len=39
            string parsedStringActual = BPListParser_Accessor.ParseAsciiString(encodedString, 0);
            Assert.AreEqual(parsedStringExpected, parsedStringActual);
        }

        private void VerifyParseInt(byte[] bpListDataInput, UInt64 valueExpected)
        {
            int offset = 0;
            UInt64 valueActual = BPListParser_Accessor.ParseInt(bpListDataInput, ref offset);
            Assert.AreEqual(valueExpected, valueActual);
            Assert.AreEqual(bpListDataInput.Length, offset);
        }

        /// <summary>
        ///A test for ParseInt
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void ParseIntTest()
        {
            VerifyParseInt(new byte[] { 0x10, 0x03 }, 3);
            VerifyParseInt(new byte[] { 0x11, 0x00, 0x05 }, 5);
            VerifyParseInt(new byte[] { 0x11, 0x01, 0x02 }, 258);
            VerifyParseInt(new byte[] { 0x12, 0x00, 0x00, 0x00, 0x08 }, 8);
            VerifyParseInt(new byte[] { 0x12, 0x00, 0x01, 0x00, 0x00 }, (1 << 16));
            VerifyParseInt(new byte[] { 0x13, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, 72057594037927936);
        }
    }
}

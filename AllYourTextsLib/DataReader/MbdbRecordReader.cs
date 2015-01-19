using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AllYourTextsLib.DataReader
{
    public class MbdbRecordReader : IEnumerable<MbdbRecord>
    {
        private MbdbRecord[] _records;

        public MbdbRecordReader(Stream recordFileStream)
        {
            using (BinaryReader binaryReader = new BinaryReader(recordFileStream))
            {
                byte[] fileData = binaryReader.ReadBytes((int)recordFileStream.Length);
                _records = MbdbParser.ParseMbdbData(fileData);
            }
        }

        public IEnumerator<MbdbRecord> GetEnumerator()
        {
            foreach (MbdbRecord record in _records)
            {
                yield return record;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllYourTextsLib
{
    public class MbdbRecordProperty
    {
        public string PropertyName { get; private set; }

        public string Value { get; private set; }

        public MbdbRecordProperty(string propertyName, string value)
        {
            PropertyName = propertyName;

            Value = value;
        }

        public override bool Equals(object obj)
        {
            MbdbRecordProperty toCompare = obj as MbdbRecordProperty;
            if (toCompare == null)
            {
                return false;
            }

            return (string.Equals(this.PropertyName, toCompare.PropertyName) && string.Equals(this.Value, toCompare.Value));
        }

        public override int  GetHashCode()
        {
 	         return base.GetHashCode();
        }
    }

    public class MbdbRecord
    {
        public string Domain { get; set; }

        public string Path { get; set; }

        public string LinkTarget { get; set; }

        public string DataHash { get; set; }

        public string UnknownField1 { get; set; }

        public UInt16 Mode { get; set; }

        public UInt32 UnknownField2 { get; set; }

        public UInt32 UnknownField3 { get; set; }

        public UInt32 UserId { get; set; }

        public UInt32 GroupId { get; set; }

        public UInt32 LastModificationTime { get; set; }

        public UInt32 LastAccessedTime { get; set; }

        public UInt32 CreationTime { get; set; }

        public UInt64 FileLength { get; set; }

        public byte Flag { get; set; }

        public byte PropertyCount { get; set; }

        public List<MbdbRecordProperty> Properties { get; set; }

        public override bool Equals(object obj)
        {
            MbdbRecord toCompare = obj as MbdbRecord;
            if (toCompare == null)
            {
                return false;
            }

            if (!string.Equals(this.Domain, toCompare.Domain))
            {
                return false;
            }

            if (!string.Equals(this.Path, toCompare.Path))
            {
                return false;
            }

            if (!string.Equals(this.LinkTarget, toCompare.LinkTarget))
            {
                return false;
            }

            if (!string.Equals(this.DataHash, toCompare.DataHash))
            {
                return false;
            }

            if (!string.Equals(this.UnknownField1, toCompare.UnknownField1))
            {
                return false;
            }

            if (this.Mode != toCompare.Mode)
            {
                return false; 
            }

            if (this.UnknownField2 != toCompare.UnknownField2)
            {
                return false;
            }

            if (this.UnknownField3 != toCompare.UnknownField3)
            {
                return false;
            }

            if (this.UserId != toCompare.UserId)
            {
                return false;
            }

            if (this.GroupId != toCompare.GroupId)
            {
                return false;
            }

            if (this.LastModificationTime != toCompare.LastModificationTime)
            {
                return false;
            }

            if (this.LastAccessedTime != toCompare.LastAccessedTime)
            {
                return false;
            }

            if (this.CreationTime != toCompare.CreationTime)
            {
                return false;
            }

            if (this.FileLength != toCompare.FileLength)
            {
                return false;
            }

            if (this.Flag != toCompare.Flag)
            {
                return false;
            }

            if (this.PropertyCount != toCompare.PropertyCount)
            {
                return false;
            }

            for (int propertyIndex = 0; propertyIndex < this.PropertyCount; propertyIndex++)
            {
                if (this.Properties[propertyIndex] != toCompare.Properties[propertyIndex])
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}", this.Domain, this.Path);
        }
    }
}

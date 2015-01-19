using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AllYourTextsLib.Framework;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace AllYourTextsTest.Library_Tests
{
    [TestClass]
    public class ExceptionSerializationTest
    {
        private object SerializeUnserialize(object toSerialize)
        {
            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream serializedStream = new MemoryStream();
            serializer.Serialize(serializedStream, toSerialize);
            byte[] serializedBytes = serializedStream.GetBuffer();

            BinaryFormatter deserializer = new BinaryFormatter();
            MemoryStream streamToDeserialize = new MemoryStream(serializedBytes);
            return deserializer.Deserialize(streamToDeserialize);
        }

        [TestMethod]
        public void DatabaseQueryExceptionTest()
        {
            DatabaseQueryException dbQueryException = new DatabaseQueryException("SELECT * FROM users");
            DatabaseQueryException dbQueryExceptionUnserialized = (DatabaseQueryException)SerializeUnserialize(dbQueryException);
            Assert.AreEqual(dbQueryException.Query, dbQueryExceptionUnserialized.Query);
        }

        [TestMethod]
        public void MbdbDataInvalidExceptionTest()
        {
            MbdbDataInvalidException ex = new MbdbDataInvalidException("this stuff's invalid!");
            MbdbDataInvalidException exDeserialized = (MbdbDataInvalidException)SerializeUnserialize(ex);
        }

        [TestMethod]
        public void MissingBackupFileExceptionTest()
        {
            MissingBackupFileException ex = new MissingBackupFileException("C:\\somepath\\somefile.dat");
            MissingBackupFileException exUnserialized = (MissingBackupFileException)SerializeUnserialize(ex);
            Assert.AreEqual(ex.Filename, exUnserialized.Filename);
        }
        
        [TestMethod]
        public void MissingBackupPathExceptionTest()
        {
            MissingBackupPathException ex = new MissingBackupPathException("C:\\apple\backupPath\\");
            MissingBackupPathException exUnserialized = (MissingBackupPathException)SerializeUnserialize(ex);
            Assert.AreEqual(ex.BackupPath, exUnserialized.BackupPath);
        }
        
        [TestMethod]
        public void NoBackupsFoundExceptionTest()
        {
            NoBackupsFoundException ex = new NoBackupsFoundException("C:\\apple\backupPath\\");
            NoBackupsFoundException exUnserialized = (NoBackupsFoundException)SerializeUnserialize(ex);
            Assert.AreEqual(ex.Path, exUnserialized.Path);
        }

        [TestMethod]
        public void UnreadableDatabaseFileExceptionTest()
        {
            UnreadableDatabaseFileException ex = new UnreadableDatabaseFileException("C:\\somepath\\sms.db");
            UnreadableDatabaseFileException exUnserialized = (UnreadableDatabaseFileException)SerializeUnserialize(ex);
            Assert.AreEqual(ex.Filename, exUnserialized.Filename);
        }
    }
}

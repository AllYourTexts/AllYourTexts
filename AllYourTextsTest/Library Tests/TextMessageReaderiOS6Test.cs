using System;
using System.Collections.Generic;
using AllYourTextsLib;
using AllYourTextsLib.DataReader;
using AllYourTextsLib.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AllYourTextsTest
{
    
    /// <summary>
    ///This is a test class for TextMessageReaderiOS6Test and is intended
    ///to contain all TextMessageReaderiOS6Test Unit Tests
    ///</summary>
    [TestClass()]
    public class TextMessageReaderiOS6Test
    {
        private const string CountryAbbreviationUnitedStates = "us";

        private const string _MockBackupPath = @"C:\fakepath\backup";

        public static TextMessage SingleMessageFromDatabase(IDatabaseReader databaseReader)
        {
            TextMessageReaderiOS6_Accessor messageReader = new TextMessageReaderiOS6_Accessor(_MockBackupPath);
            messageReader.ParseDatabase(databaseReader);

            List<TextMessage> messages = new List<TextMessage>(messageReader);

            return messages[0];
        }

        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void ParseItemFromDatabaseiMessageTest()
        {
            const string messageContentsExpected = "Hello, kind sir!";
            const string phoneNumberValueExpected = "+19245550985";

            MockTextDatabaseReaderiOS6 mockReader = new MockTextDatabaseReaderiOS6();
            mockReader.AddRow(13,
                              phoneNumberValueExpected,
                              phoneNumberValueExpected,
                              messageContentsExpected,
                              CountryAbbreviationUnitedStates,
                              "iMessage",
                              347059294,
                              347059298,
                              0,
                              0,
                              null,
                              null);

            TextMessage actual = SingleMessageFromDatabase(mockReader);

            Assert.AreEqual(messageContentsExpected, actual.MessageContents);
            Assert.AreEqual(phoneNumberValueExpected, actual.Address);
            Assert.IsNull(actual.ChatId);
            Assert.IsFalse(actual.IsOutgoing);
            Assert.AreEqual(CountryAbbreviationUnitedStates, actual.Country);
            Assert.IsFalse(actual.HasAttachments());

            DateTime timestampExpected = new DateTime(2011, 12, 30, 16, 21, 38);
            Assert.AreEqual(timestampExpected, actual.Timestamp);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void ParseItemFromDatabaseSmsMessageTest()
        {
            const long messageIdExpected = 18;
            const string messageContentsExpected = "This is low text text messaging!";
            const string phoneNumberValueExpected = "+18245556592";

            MockTextDatabaseReaderiOS6 mockReader = new MockTextDatabaseReaderiOS6();
            mockReader.AddRow(messageIdExpected,
                              phoneNumberValueExpected,
                              phoneNumberValueExpected,
                              messageContentsExpected,
                              CountryAbbreviationUnitedStates,
                              "SMS",
                              347059294,
                              347059672,
                              0,
                              0,
                              null,
                              null);

            TextMessage actual = SingleMessageFromDatabase(mockReader);

            Assert.AreEqual(messageIdExpected, actual.MessageId);
            Assert.AreEqual(messageContentsExpected, actual.MessageContents);
            Assert.AreEqual(phoneNumberValueExpected, actual.Address);
            Assert.IsNull(actual.ChatId);
            Assert.IsFalse(actual.IsOutgoing);
            Assert.AreEqual(CountryAbbreviationUnitedStates, actual.Country);
            Assert.IsFalse(actual.HasAttachments());

            DateTime timestampExpected = new DateTime(2011, 12, 30, 16, 21, 34);
            Assert.AreEqual(timestampExpected, actual.Timestamp);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void ParseItemFromDatabaseMessageWithAttachmentTest()
        {
            const long messageIdExpected = 18;
            const string messageContentsExpected = "Check out this badass attachment I'm sending you!";
            const string phoneNumberValueExpected = "+15875552534";

            MockTextDatabaseReaderiOS6 mockReader = new MockTextDatabaseReaderiOS6();
            mockReader.AddRow(messageIdExpected,
                              phoneNumberValueExpected,
                              phoneNumberValueExpected,
                              messageContentsExpected,
                              CountryAbbreviationUnitedStates,
                              "iMessage",
                              347060295,
                              347060672,
                              0,
                              0,
                              "~/Library/SMS/Attachments/f9/09/DD97CB48-3B51-4DD6-959F-9BF9F6ABB58F/IMG_0004.JPG",
                              "image/jpeg");

            TextMessage actual = SingleMessageFromDatabase(mockReader);

            Assert.AreEqual(messageIdExpected, actual.MessageId);
            Assert.AreEqual(messageContentsExpected, actual.MessageContents);
            Assert.AreEqual(phoneNumberValueExpected, actual.Address);
            Assert.IsNull(actual.ChatId);
            Assert.IsFalse(actual.IsOutgoing);
            Assert.AreEqual(CountryAbbreviationUnitedStates, actual.Country);
            Assert.AreEqual(@"C:\fakepath\backup\851584bf7c55a76d4ec7749cc72d5e0b9185c30b", actual.Attachments[0].Path);
            Assert.AreEqual(AttachmentType.Image, actual.Attachments[0].Type);
            Assert.AreEqual("IMG_0004.JPG", actual.Attachments[0].OriginalFilename);

            DateTime timestampExpected = new DateTime(2011, 12, 30, 16, 44, 32);
            Assert.AreEqual(timestampExpected, actual.Timestamp);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void ParseItemFromDatabaseMessageWithMalformedAttachmentTest()
        {
            const long messageIdExpected = 18;
            const string messageContentsExpected = "Check out this badass attachment I'm sending you!";
            const string phoneNumberValueExpected = "+15875552534";

            MockTextDatabaseReaderiOS6 mockReader = new MockTextDatabaseReaderiOS6();
            mockReader.AddRow(messageIdExpected,
                              phoneNumberValueExpected,
                              phoneNumberValueExpected,
                              messageContentsExpected,
                              CountryAbbreviationUnitedStates,
                              "iMessage",
                              347060295,
                              347060672,
                              0,
                              0,
                              "/malformed/path+/Attachments/f9/09/DD97CB48-3B51-4DD6-959F-9BF9F6ABB58F/IMG_0004.JPG",
                              "image/jpeg");

            TextMessage actual = SingleMessageFromDatabase(mockReader);

            Assert.AreEqual(messageIdExpected, actual.MessageId);
            Assert.AreEqual(messageContentsExpected, actual.MessageContents);
            Assert.AreEqual(phoneNumberValueExpected, actual.Address);
            Assert.IsNull(actual.ChatId);
            Assert.IsFalse(actual.IsOutgoing);
            Assert.AreEqual(CountryAbbreviationUnitedStates, actual.Country);
            Assert.IsFalse(actual.HasAttachments());

            DateTime timestampExpected = new DateTime(2011, 12, 30, 16, 44, 32);
            Assert.AreEqual(timestampExpected, actual.Timestamp);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void ParseItemFromDatabaseChatMessageTest()
        {
            const string messageContentsExpected = "I wish to chat with yous";
            const string chatIdentifierxpected = "chat893428820";

            MockTextDatabaseReaderiOS6 mockReader = new MockTextDatabaseReaderiOS6();
            mockReader.AddRow(16,
                              "+16465551579",
                              chatIdentifierxpected,
                              messageContentsExpected,
                              CountryAbbreviationUnitedStates,
                              "iMessage",
                              347059294,
                              0,
                              347059298,
                              1,
                              null,
                              null);

            TextMessage actual = SingleMessageFromDatabase(mockReader);

            Assert.AreEqual(messageContentsExpected, actual.MessageContents);
            Assert.IsNull(actual.Address);
            Assert.AreEqual(chatIdentifierxpected, actual.ChatId);
            Assert.IsTrue(actual.IsOutgoing);
            Assert.AreEqual(CountryAbbreviationUnitedStates, actual.Country);
            Assert.IsFalse(actual.HasAttachments());

            DateTime timestampExpected = new DateTime(2011, 12, 30, 16, 21, 34);
            Assert.AreEqual(timestampExpected, actual.Timestamp);
        }

        private void VerifyMimeTypeMatchesExpected(string mimeTypeInput, AttachmentType typeExpected)
        {
            TextMessageReaderiOS6_Accessor reader = new TextMessageReaderiOS6_Accessor(@"C:\fakepath\");
            AttachmentType typeActual = reader.ParseAttachmentType(mimeTypeInput);
            Assert.AreEqual(typeExpected, typeActual);
        }

        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void ParseAttachmentTypeTest()
        {
            VerifyMimeTypeMatchesExpected("image/gif", AttachmentType.Image);
            VerifyMimeTypeMatchesExpected("image/jpeg", AttachmentType.Image);
            VerifyMimeTypeMatchesExpected("image/pjpeg", AttachmentType.Image);
            VerifyMimeTypeMatchesExpected("image/png", AttachmentType.Image);
            VerifyMimeTypeMatchesExpected("image/svg+xml", AttachmentType.Image);
            VerifyMimeTypeMatchesExpected("image/tiff", AttachmentType.Image);
            VerifyMimeTypeMatchesExpected("image/vnd.microsoft.icon", AttachmentType.Image);

            VerifyMimeTypeMatchesExpected("video/mpeg", AttachmentType.Video);
            VerifyMimeTypeMatchesExpected("video/mp4", AttachmentType.Video);
            VerifyMimeTypeMatchesExpected("video/ogg", AttachmentType.Video);
            VerifyMimeTypeMatchesExpected("video/quicktime", AttachmentType.Video);
            VerifyMimeTypeMatchesExpected("video/webm", AttachmentType.Video);
            VerifyMimeTypeMatchesExpected("video/x-matroska", AttachmentType.Video);
            VerifyMimeTypeMatchesExpected("video/x-ms-wmv", AttachmentType.Video);
            VerifyMimeTypeMatchesExpected("video/x-flv", AttachmentType.Video);

            VerifyMimeTypeMatchesExpected("audio/basic", AttachmentType.Audio);
            VerifyMimeTypeMatchesExpected("audio/mp4", AttachmentType.Audio);
            VerifyMimeTypeMatchesExpected("audio/mpeg", AttachmentType.Audio);
            VerifyMimeTypeMatchesExpected("audio/vorbis", AttachmentType.Audio);

            VerifyMimeTypeMatchesExpected("text/vcard", AttachmentType.Unknown);
            VerifyMimeTypeMatchesExpected("text/plain", AttachmentType.Unknown);
            VerifyMimeTypeMatchesExpected("text/html", AttachmentType.Unknown);
            VerifyMimeTypeMatchesExpected("text/javascript", AttachmentType.Unknown);
        }
    }
}

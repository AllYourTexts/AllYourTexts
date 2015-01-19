using AllYourTextsLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AllYourTextsLib.Framework;

namespace AllYourTextsTest
{
    [TestClass()]
    public class MessageAttachmentTest
    {

        /// <summary>
        ///A test for StripPhoneNumber
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void SimpleAttachmentTest()
        {
            const int messageId = 1;
            const AttachmentType attachmentType = AttachmentType.Image;
            const string filePath = @"c:\backup\somefile";
            const string originalFilename = @"IMG_0312.JPG";
            MessageAttachment attachment = new MessageAttachment(messageId, attachmentType, filePath, originalFilename);
            Assert.AreEqual(messageId, attachment.MessageId);
            Assert.AreEqual(attachmentType, attachment.Type);
            Assert.AreEqual(filePath, attachment.Path);
            Assert.AreEqual(originalFilename, attachment.OriginalFilename);

            MessageAttachment attachmentCopy = new MessageAttachment(messageId, attachmentType, filePath, originalFilename);
            Assert.AreEqual(attachment, attachmentCopy);

            MessageAttachment attachmentNonCopy = new MessageAttachment(messageId + 1, AttachmentType.Image, @"c:\otherpath\otherfile", "IMG_0021.JPG");
            Assert.AreNotEqual(attachment, attachmentNonCopy);
        }
    }
}

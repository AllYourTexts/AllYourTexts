using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AllYourTextsUi.Framework;
using AllYourTextsLib.Framework;
using DummyData;
using AllYourTextsUi;
using System.Windows.Media;
using AllYourTextsUi.Exporting;
using AllYourTextsTest.Mock_Objects;

namespace AllYourTextsTest
{

    [TestClass()]
    public class ConversationRendererHtmlTest
    {

        private void VerifyRenderedMessagesMatchExpected(DummyPhoneNumberId phoneNumberId, string renderedExpected)
        {
            IDisplayOptionsReadOnly displayOptions = new MockDisplayOptions();
            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(phoneNumberId);
            IConversationRenderer renderer = new ConversationRendererHtml(displayOptions, conversation, new AttachmentExportLocator(null));

            string renderedActual = renderer.RenderMessagesAsString(ConversationRendererBase.RenderAllMessages);
            Assert.AreEqual(renderedExpected, renderedActual);
        }

        [TestMethod()]
        public void RenderMessagesTest()
        {
            string renderedExpected = @"<p>
<span class=""date"">Tuesday, Nov 4, 2008</span><br />
<span style=""color:rgb(210,0,0);""><span class=""senderName"">Me</span> <span class=""timestamp"">(<span dir=""ltr"" lang=""en"">10:18:05 PM</span>)</span>: </span>Congrats, buddy!<br />
<span style=""color:rgb(0,0,210);""><span class=""senderName"">Barack Obama</span> <span class=""timestamp"">(<span dir=""ltr"" lang=""en"">10:18:40 PM</span>)</span>: </span>Thanks. Couldn&apos;t have done it without you!<br />
<span style=""color:rgb(210,0,0);""><span class=""senderName"">Me</span> <span class=""timestamp"">(<span dir=""ltr"" lang=""en"">10:25:58 PM</span>)</span>: </span>np
</p>

<p>
<span class=""date"">Sunday, May 1, 2011</span><br />
<span style=""color:rgb(210,0,0);""><span class=""senderName"">Me</span> <span class=""timestamp"">(<span dir=""ltr"" lang=""en"">8:47:27 AM</span>)</span>: </span>Yo, I think I know where Osama Bin Laden is hiding?<br />
<span style=""color:rgb(0,0,210);""><span class=""senderName"">Barack Obama</span> <span class=""timestamp"">(<span dir=""ltr"" lang=""en"">8:50:52 AM</span>)</span>: </span>o rly?<br />
<span style=""color:rgb(210,0,0);""><span class=""senderName"">Me</span> <span class=""timestamp"">(<span dir=""ltr"" lang=""en"">8:51:21 AM</span>)</span>: </span>Yeah, dude. Abottabad, Pakistan. Huge compound. Can&apos;t miss it.<br />
<span style=""color:rgb(0,0,210);""><span class=""senderName"">Barack Obama</span> <span class=""timestamp"">(<span dir=""ltr"" lang=""en"">8:51:46 AM</span>)</span>: </span>Sweet. I&apos;ll send some navy seals.
</p>
";
            VerifyRenderedMessagesMatchExpected(DummyPhoneNumberId.ObamaCell, renderedExpected);
        }

        [TestMethod()]
        public void RenderMessagesSingleParagraphTest()
        {
            string renderedExpected = @"<p>
<span class=""date"">Monday, Oct 17, 2011</span><br />
<span style=""color:rgb(210,0,0);""><span class=""senderName"">Me</span> <span class=""timestamp"">(<span dir=""ltr"" lang=""en"">3:45:40 PM</span>)</span>: </span>Whatup Cracker\Jack? Love the new number.<br />
<span style=""color:rgb(0,0,210);""><span class=""senderName"">Cracker\Jack</span> <span class=""timestamp"">(<span dir=""ltr"" lang=""en"">3:45:43 PM</span>)</span>: </span>Thanks, dog!
</p>
";
            VerifyRenderedMessagesMatchExpected(DummyPhoneNumberId.CrackerJackOffice, renderedExpected);
        }

        [TestMethod()]
        public void RenderMessagesEscapeHtmlTest()
        {
            string renderedExpected = @"<p>
<span class=""date"">Sunday, Oct 23, 2011</span><br />
<span style=""color:rgb(210,0,0);""><span class=""senderName"">Me</span> <span class=""timestamp"">(<span dir=""ltr"" lang=""en"">9:45:06 PM</span>)</span>: </span>How do you make stuff bold, Bobby?<br />
<span style=""color:rgb(0,0,210);""><span class=""senderName"">Bo&lt;b&gt;b&lt;/b&gt;by Css</span> <span class=""timestamp"">(<span dir=""ltr"" lang=""en"">9:45:24 PM</span>)</span>: </span>Use the &lt;b&gt; tag!<br />
<span style=""color:rgb(210,0,0);""><span class=""senderName"">Me</span> <span class=""timestamp"">(<span dir=""ltr"" lang=""en"">9:45:44 PM</span>)</span>: </span>And how do I do a right single quote?<br />
<span style=""color:rgb(0,0,210);""><span class=""senderName"">Bo&lt;b&gt;b&lt;/b&gt;by Css</span> <span class=""timestamp"">(<span dir=""ltr"" lang=""en"">9:46:03 PM</span>)</span>: </span>It&apos;s as easy as &amp;rsquo;!
</p>
";
            VerifyRenderedMessagesMatchExpected(DummyPhoneNumberId.BobbyCssOffice, renderedExpected);
        }

        [TestMethod()]
        public void RenderMessageWithAttachmentTest()
        {
            string renderedExpected = @"<p>
<span class=""date"">Sunday, Sep 9, 2012</span><br />
<span style=""color:rgb(0,0,210);""><span class=""senderName"">Frankie Coolpics</span> <span class=""timestamp"">(<span dir=""ltr"" lang=""en"">8:34:15 PM</span>)</span>: </span>Check this shit out!<br />
<a href=""FrankieCoolpics_attachments\IMG_0036.JPG"" target=""_blank""><img class=""attachmentImage"" src=""FrankieCoolpics_attachments\IMG_0036.JPG"" /></a><br />
<span style=""color:rgb(210,0,0);""><span class=""senderName"">Me</span> <span class=""timestamp"">(<span dir=""ltr"" lang=""en"">8:34:30 PM</span>)</span>: </span>Crazy!
</p>
";
            IDisplayOptionsReadOnly displayOptions = new MockDisplayOptions();
            AttachmentExportLocator attachmentExportLocator = new AttachmentExportLocator( @"C:\backup\export");
            attachmentExportLocator.AddFileExportLocation(@"C:\fakepath\backup\082308302382", @"C:\backup\export\FrankieCoolpics_attachments\IMG_0036.JPG");
            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.FrankieCoolPicsCell);
            IConversationRenderer renderer = new ConversationRendererHtml(displayOptions, conversation, attachmentExportLocator);

            string renderedActual = renderer.RenderMessagesAsString(ConversationRendererBase.RenderAllMessages);
            Assert.AreEqual(renderedExpected, renderedActual);
        }

        [TestMethod()]
        public void RenderMessagesEmptyTest()
        {
            string renderedExpected = "<p>" + ConversationRendererHtml_Accessor._noConversationMessage + "</p>";
            VerifyRenderedMessagesMatchExpected(DummyPhoneNumberId.NeverTexterCell, renderedExpected);
        }

        private void VerifyRenderedMessagesMatchExpected(Color color, string cssAttributeExpected)
        {
            IDisplayOptionsReadOnly displayOptions = new MockDisplayOptions();
            IConversation conversation = DummyConversationDataGenerator.GetSingleConversation(DummyPhoneNumberId.ObamaCell);
            ConversationRendererHtml_Accessor renderer = new ConversationRendererHtml_Accessor(displayOptions, conversation, new AttachmentExportLocator(null));

            string cssAttributeActual = renderer.ColorToCssAttribute(color);
            Assert.AreEqual(cssAttributeExpected, cssAttributeActual);
        }

        [TestMethod()]
        public void ColorToCssAttributeTest()
        {
            VerifyRenderedMessagesMatchExpected(Color.FromRgb(10, 20, 30), "rgb(10,20,30)");
        }
    }
}

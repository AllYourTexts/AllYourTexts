using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using AllYourTextsLib.Framework;
using AllYourTextsUi.Framework;
using AllYourTextsUi.Commands;

namespace AllYourTextsUi
{
    /// <summary>
    /// Interaction logic for ConversationRenderControl.xaml
    /// </summary>
    public partial class ConversationRenderControl : UserControl
    {
        public IDisplayOptions DisplayOptions { get; set; }

        private IConversation _conversation;

        public RichTextBox ConversationTextBox;

        private IConversationRenderer _renderer;

        private delegate void ClearTextBoxesDelegate();

        private delegate void NextMessageDelegate(int arg);

        private ManualResetEventSlim _renderCompleteEvent;

        public ConversationRenderControl()
        {
            InitializeComponent();

            DisplayOptions = null;

            ConversationTextBox = emptyRichTextBox;

            _renderCompleteEvent = new ManualResetEventSlim();
        }

        public IConversation Conversation
        {
            get
            {
                return _conversation;
            }
            set
            {
                _conversation = value;
                RenderConversation();
            }
        }

        public void DoCopyConversationText_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Clipboard.SetText(ConversationTextBox.Selection.Text);
        }

        public void DoCopyConversationText_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !ConversationTextBox.Selection.IsEmpty;
        }

        public void DoCopyConversationTextFormatted_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataObject selectionData = new DataObject();

            string richTextFormat = System.Windows.DataFormats.Rtf;
            MemoryStream richTextStream = new MemoryStream();
            ConversationTextBox.Selection.Save(richTextStream, richTextFormat);
            selectionData.SetData(richTextFormat, richTextStream);

            selectionData.SetText(ConversationTextBox.Selection.Text);

            Clipboard.SetDataObject(selectionData, true);
        }

        public void DoCopyConversationTextFormatted_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !ConversationTextBox.Selection.IsEmpty;
        }

        private void RenderConversation()
        {
            ConversationTextBox.Visibility = Visibility.Collapsed;

            ConversationTextBox = new ConvesationTextBox();
            ConversationTextBox.Document.Blocks.Clear();

            conversationPanel.Children.Add(ConversationTextBox);

            if (Conversation == null)
            {
                ConversationTextBox.Document.Blocks.Add(new Paragraph(new Italic(new Run("No conversation selected."))));
                _renderCompleteEvent.Set();
                return;
            }

            _renderer = new ConversationRendererRichText(DisplayOptions, Conversation);

            ConversationTextBox.Dispatcher.BeginInvoke(
                DispatcherPriority.Normal,
                new NextMessageDelegate(RenderNextMessage),
                40);

            ConversationTextBox.Dispatcher.BeginInvoke(
                DispatcherPriority.SystemIdle,
                new ClearTextBoxesDelegate(ClearTextBoxes));
        }

        private void ClearTextBoxes()
        {
            if (conversationPanel.Children.Count <= 1)
            {
                return;
            }

            RichTextBox textBoxToClear = (RichTextBox)conversationPanel.Children[0];

            int blockstoRemove = Math.Min(100, textBoxToClear.Document.Blocks.Count);

            if (blockstoRemove == 0)
            {
                conversationPanel.Children.RemoveAt(0);
                return;
            }

            for (int i = 0; i < blockstoRemove; i++)
            {
                textBoxToClear.Document.Blocks.Remove(textBoxToClear.Document.Blocks.LastBlock);
            }

            ConversationTextBox.Dispatcher.BeginInvoke(
                DispatcherPriority.SystemIdle,
                new ClearTextBoxesDelegate(ClearTextBoxes));
        }

        private void RenderNextMessage(int messageCount)
        {
            List<Paragraph> paragraphs = _renderer.RenderMessagesAsParagraphs(messageCount);

            ConversationTextBox.Document.Blocks.AddRange(paragraphs);

            if (!_renderer.HasUnprocessedMessages)
            {
                _renderCompleteEvent.Set();
                return;
            }

            ConversationTextBox.Dispatcher.BeginInvoke(
                DispatcherPriority.SystemIdle,
                new NextMessageDelegate(RenderNextMessage),
                200);
        }

        public void PrepareWaitForRenderComplete()
        {
            _renderCompleteEvent.Reset();
        }

        public void WaitForRenderComplete()
        {
            _renderCompleteEvent.Wait();
        }
    }
}

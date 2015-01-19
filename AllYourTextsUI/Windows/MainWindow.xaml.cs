using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using AllYourTextsLib;
using AllYourTextsLib.Conversation;
using AllYourTextsLib.DataReader;
using AllYourTextsLib.Framework;
using AllYourTextsUi.Framework;
using Microsoft.Win32;
using AllYourTextsUi.Windows;
using AllYourTextsUi.Commands;
using AllYourTextsUi.Facebook;
using AllYourTextsUi.Exporting;

namespace AllYourTextsUi
{

    public enum ApplicationView
    {
        Unknown = 0,
        ConversationView,
        GraphView
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IConversationSearchTarget
    {
        private IMainWindowModel _mainWindowModel;

        private IConversationWindowModel _conversationWindowModel;

        private IGraphWindowModel _graphWindowModel;

        private ApplicationView _currentView;

        private IConversationManager _conversationManager;

        private IDisplayOptions _displayOptions;

        private IPhoneSelectOptions _phoneSelectOptions;

        private IPhoneDeviceInfo _deviceInfo;

        private LoadingProgressDialogView _progressDialog;

        private ILoadingProgressCallback _progressCallback;

        const ApplicationView DefaultApplicationView = ApplicationView.ConversationView;

        public MainWindow()
        {
            InitializeComponent();

            _currentView = DefaultApplicationView;
            _conversationManager = null;
            DisplayOptions options = new DisplayOptions();
            _displayOptions = options as IDisplayOptions;
            _displayOptions.TimeDisplayFormatPropertyChanged += OnTimeDisplayFormatPropertyChanged;
            _displayOptions.HideEmptyConversationsPropertyChanged += OnHideEmptyConversationsPropertyChanged;
            _displayOptions.MergeContactsPropertyChanged += OnMergeContactsPropertyChanged;
            _displayOptions.LoadMmsAttachmentsPropertyChanged += OnLoadMmsAttachmentsPropertyChanged;
            _displayOptions.ConversationSortingPropertyChanged += OnConversationSortingPropertyChanged;
            _phoneSelectOptions = options as IPhoneSelectOptions;

            _deviceInfo = null;

            conversationRenderControl.DisplayOptions = _displayOptions;
            conversationRenderControl.findBar.FindModel = new FindDialogModel(this);

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            try
            {
                GetSelectedPhoneInfo();
                PerformRefresh();
            }
            catch (BackupDatabaseReadException ex)
            {
                ShowDatabaseErrorDialog(ex);
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                FailHandler.HandleUnrecoverableFailure(ex);
            }
        }

        public void DoClose_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public void DoClose_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public void DoCopyGraph_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Clipboard.SetImage(graphControl.GetGraphBitmap());
        }

        public void DoCopyGraph_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public void DoShareOnFacebook_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IAccessTokenSerializer tokenSerializer = new AccessTokenSerializer(new OsFileSystem());
            GraphFacebookUploader uploader = new GraphFacebookUploader(tokenSerializer);

            try
            {
                uploader.DoUpload(graphControl.GetGraphBitmap());
            }
            catch
            {
                ;
            }
        }

        public void DoShareOnFacebook_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public void DoFind_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (conversationRenderControl.findBar.Visibility == Visibility.Collapsed)
            {
                conversationRenderControl.findBar.Visibility = Visibility.Visible;
            }
        }

        public void DoFind_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (_currentView == ApplicationView.ConversationView);
        }

        public void DoSaveGraph_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            GraphExporter.ExportToFile(graphControl.GetGraphBitmap());
        }

        public void DoSaveGraph_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if ((_currentView == ApplicationView.GraphView) && (_graphWindowModel.SelectedConversation != null))
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void DoRefresh_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PerformRefresh();
        }

        public void DoRefresh_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void DoExportSingleConversation_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExportCurrentConversation();
        }

        public void DoExportSingleConversation_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_mainWindowModel.SelectedConversation != null && (_mainWindowModel.SelectedConversation.GetType() != typeof(AggregateConversation)))
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void DoExportAllConversations_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ExportAllConversations();
        }

        public void DoExportAllConversations_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void DoSwitchToConversationView_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SetApplicationView(ApplicationView.ConversationView);

            HideGraphViewControls();
            ShowConversationViewControls();
            PopulateConversationList();
            Update();
        }

        public void DoSwitchToConversationView_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void DoSwitchToGraphView_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SetApplicationView(ApplicationView.GraphView);

            HideConversationViewControls();
            ShowGraphViewControls();
            PopulateConversationList();
            Update();
        }

        public void DoSwitchToGraphView_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ShowSettingsDialog_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IDisplayOptions displayOptions = _mainWindowModel.DisplayOptions;
            IPhoneSelectOptions phoneSelectOptions = _mainWindowModel.PhoneSelectOptions;
            OptionsDialogView optionsDialog = new OptionsDialogView(displayOptions, phoneSelectOptions);

            optionsDialog.Owner = this;
            optionsDialog.SelectedDevice = _deviceInfo;
            optionsDialog.ShowDialog();

            if (optionsDialog.DialogResult == true)
            {
                if (optionsDialog.SelectedDevice != _deviceInfo)
                {
                    _deviceInfo = optionsDialog.SelectedDevice;
                    PerformRefresh();
                }
            }
        }

        public void ShowSettingsDialog_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public void OnHideEmptyConversationsPropertyChanged(object sender, EventArgs e)
        {
            if (_mainWindowModel == null)
            {
                return;
            }

            PopulateConversationList();
            Update();
        }

        public void OnMergeContactsPropertyChanged(object sender, EventArgs e)
        {
            PerformRefresh();
        }

        public void OnLoadMmsAttachmentsPropertyChanged(object sender, EventArgs e)
        {
            PerformRefresh();
        }

        public void OnTimeDisplayFormatPropertyChanged(object sender, EventArgs e)
        {
            if ((_currentView == ApplicationView.ConversationView) && (_conversationWindowModel != null))
            {
                UpdateConversationPanel(_conversationWindowModel.SelectedConversation);
            }
        }

        public void OnConversationSortingPropertyChanged(object sender, EventArgs e)
        {
            if (_mainWindowModel == null)
            {
                return;
            }

            PopulateConversationList();
            Update();
        }

        private void PerformRefresh()
        {
            LoadConversationManagerAsync();
        }

        private void LaunchUrl(Uri url)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(url.AbsoluteUri));
        }

        private void ExportCurrentConversation()
        {
            ExportedFilenameGenerator filenameGenerator = new ExportedFilenameGenerator();
            SaveFileDialog saveDialog = new SaveFileDialog();
            const int FilterIndexHtml = 1;
            const int FilterIndexPlaintext = 2;
            saveDialog.Filter = "Web Page|*.html|Text File|*.txt";
            saveDialog.Title = "Save Conversation History As";
            saveDialog.AddExtension = true;
            saveDialog.FileName = filenameGenerator.CreateExportFilenameSuggestion(_mainWindowModel.SelectedConversation);
            saveDialog.ShowDialog();

            if (string.IsNullOrEmpty(saveDialog.FileName))
            {
                return;
            }

            IFileSystem exportFileSystem = new OsFileSystem();
            IConversationExporter conversationExporter;

            switch (saveDialog.FilterIndex)
            {
                case FilterIndexHtml:
                    conversationExporter  = new ConversationExporterHtml(exportFileSystem);
                    break;
                case FilterIndexPlaintext:
                    conversationExporter = new ConversationExporterPlaintext(exportFileSystem);
                    break;
                default:
                    throw new ArgumentException("Invalid file save type.");
            }
            conversationExporter.Export(_mainWindowModel.SelectedConversation, _displayOptions, saveDialog.FileName);
        }

        private void ExportAllConversations()
        {
            ExportMultipleDialogView exportDialog = new ExportMultipleDialogView(_conversationManager, _displayOptions);
            exportDialog.ShowDialog();
        }

        private void HelpAbout_Click(object sender, RoutedEventArgs e)
        {
            IAboutBoxModel aboutBoxModel = new AboutBoxModel();
            AboutBoxView aboutBox = new AboutBoxView(aboutBoxModel);

            aboutBox.Owner = this;
            aboutBox.ShowDialog();
        }

        private void HelpGiveFeedback_Click(object sender, RoutedEventArgs e)
        {
            ShowSendFeedbackDialog();
        }

        private void HelpGoToWebSite_Click(object sender, RoutedEventArgs e)
        {
            LaunchUrl(ProductWebSiteInfo.Url);
        }

        private void ShowSendFeedbackDialog()
        {
            IBugSendDialogModel feedbackDialogModel = new SendFeedbackDialogModel(new BugReportCreator());
            SendFeedbackDialogView dialog = new SendFeedbackDialogView(feedbackDialogModel);
            dialog.Owner = this;
            dialog.ShowDialog();
        }

        private void ShowConversationViewControls()
        {
            conversationRenderControl.Visibility = Visibility.Visible;
        }

        private void HideConversationViewControls()
        {
            conversationRenderControl.Visibility = Visibility.Collapsed;
        }

        private void ShowGraphViewControls()
        {
            graphControl.Visibility = Visibility.Visible;
            graphTypeLabel.Visibility = Visibility.Visible;
            graphTypeComboBox.Visibility = Visibility.Visible;
        }

        private void HideGraphViewControls()
        {
            graphControl.Visibility = Visibility.Collapsed;
            graphTypeLabel.Visibility = Visibility.Hidden;
            graphTypeComboBox.Visibility = Visibility.Hidden;
        }

        private void SetApplicationView(ApplicationView view)
        {
            menuViewConversation.IsChecked = false;
            menuViewGraph.IsChecked = false;

            switch (view)
            {
                case ApplicationView.ConversationView:
                    menuViewConversation.IsChecked = true;
                    menuEditCopy.Command = CopyConversationTextCommand.CopyConversationText;
                    _mainWindowModel = _conversationWindowModel;
                    break;
                case ApplicationView.GraphView:
                    menuViewGraph.IsChecked = true;
                    menuEditCopy.Command = CopyGraphCommand.CopyGraph;
                    _mainWindowModel = _graphWindowModel;
                    break;
                default:
                    throw new ArgumentException("Invalid conversation view.");
            }

            _currentView = view;
        }

        private IPhoneDeviceInfo GetSelectedPhoneInfo()
        {
            List<IPhoneDeviceInfo> devicesInfo = devicesInfo = new List<IPhoneDeviceInfo>(PhoneDeviceInfoReader.GetDevicesInfo());
            PhoneSelector phoneSelector = new PhoneSelector(devicesInfo, _phoneSelectOptions);

            IPhoneDeviceInfo selectedPhone;

            selectedPhone = phoneSelector.AutoSelectPhoneDevice();

            if (phoneSelector.ShouldWarnAboutLaterSyncedPhone())
            {
                bool letUserChoosePhone;
                bool ignoreNewerSyncs;
                PromptToChooseLaterSync(out letUserChoosePhone, out ignoreNewerSyncs);

                if (letUserChoosePhone)
                {
                    selectedPhone = null;
                }
                else if (ignoreNewerSyncs)
                {
                    _phoneSelectOptions.WarnAboutMoreRecentSync = false;
                    _phoneSelectOptions.Save();
                }
            }

            if (selectedPhone != null)
            {
                _deviceInfo = selectedPhone;
                return selectedPhone;
            }

            bool alwaysPrompt;
            if (PromptForPhoneSelectionOptions(devicesInfo, out selectedPhone, out alwaysPrompt) == false)
            {
                Environment.Exit(0);
            }

            _deviceInfo = selectedPhone;
            _phoneSelectOptions.PromptForPhoneChoice = alwaysPrompt;
            _phoneSelectOptions.PhoneDataPath = selectedPhone.BackupPath;
            _phoneSelectOptions.Save();

            return selectedPhone;
        }

        private void PromptToChooseLaterSync(out bool letUserChoosePhone, out bool ignoreNewerSyncs)
        {
            LaterSyncWarning laterSyncWarningWindow = new LaterSyncWarning();
            laterSyncWarningWindow.Owner = this;
            laterSyncWarningWindow.ShowDialog();

            letUserChoosePhone = (laterSyncWarningWindow.DialogResult == true);
            ignoreNewerSyncs = laterSyncWarningWindow.IgnoreMoreRecentSyncs;
        }

        private bool PromptForPhoneSelectionOptions(IEnumerable<IPhoneDeviceInfo> devicesInfo, out IPhoneDeviceInfo selectedPhone, out bool alwaysPromptForChoice)
        {
            PhoneSelectionView phoneSelection = new PhoneSelectionView(devicesInfo, _phoneSelectOptions);
            phoneSelection.Owner = this;
            phoneSelection.ShowDialog();

            if (phoneSelection.DialogResult == false)
            {
                selectedPhone = null;
                alwaysPromptForChoice = false;
                return false;
            }

            alwaysPromptForChoice = phoneSelection.AlwaysPrompt;
            selectedPhone = phoneSelection.SelectedPhoneInfo;

            return true;
        }

        private void LoadConversationManagerAsync()
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            _progressCallback = new LoadingProgressCallback(backgroundWorker);

            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += OnDoWork;
            backgroundWorker.ProgressChanged += OnProgressChanged;
            backgroundWorker.RunWorkerCompleted += OnWorkerCompleted;

            _progressDialog = new LoadingProgressDialogView();
            _progressDialog.Owner = this;
            _progressDialog.Cancel += OnCancelProcess;
            AsyncLoadingProgressParams workParams = new AsyncLoadingProgressParams(_progressCallback, _displayOptions.MergeContacts, _deviceInfo);

            backgroundWorker.RunWorkerAsync(workParams);
            _progressDialog.ShowDialog();
        }

        private static void OnDoWork(object sender, DoWorkEventArgs e)
        {
            AsyncLoadingProgressParams workParams = (AsyncLoadingProgressParams)e.Argument;
            ILoadingProgressCallback progressCallback = workParams.LoadingProgressCallback;
            bool mergeConversations = workParams.MergeConversations;
            IPhoneDeviceInfo deviceInfo = workParams.DeviceInfo;

            try
            {
                IConversationManager conversationManager = MainWindowModelBase.LoadConversationManager(progressCallback, deviceInfo, mergeConversations);

                progressCallback.End();

                e.Result = conversationManager;
            }
            catch (OperationCanceledException)
            {
                e.Cancel = true;
            }
        }

        private void UpdateModels()
        {
            _conversationWindowModel = new ConversationWindowModel(_displayOptions, _phoneSelectOptions);
            _conversationWindowModel.ConversationManager = _conversationManager;
            _conversationWindowModel.DeviceInfo = _deviceInfo;

            _graphWindowModel = new GraphWindowModel(_displayOptions, _phoneSelectOptions);
            _graphWindowModel.ConversationManager = _conversationManager;
            _graphWindowModel.DeviceInfo = _deviceInfo;

            SetApplicationView(_currentView);
        }

        private void OnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _progressDialog.ProgressValue = e.ProgressPercentage;
        }

        private void OnWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                if (_conversationManager == null)
                {
                    Application.Current.Shutdown();
                }
                else if (_progressDialog != null)
                {
                    _progressDialog.Close();
                }
            }
            else if (e.Error != null)
            {
                _progressDialog.Close();

                //
                // If the database file is unreadable, don't auto-open it.
                //

                if (_displayOptions != null)
                {
                    _phoneSelectOptions.PhoneDataPath = "";
                    _phoneSelectOptions.Save();
                }

                if (e.Error is DirectoryNotFoundException)
                {
                    ShowDatabaseErrorDialog(new MissingBackupPathException());
                }
                else if (e.Error is FileNotFoundException)
                {
                    FileNotFoundException ex = (FileNotFoundException)e.Error;
                    ShowDatabaseErrorDialog(new MissingBackupFileException(ex.FileName, ex));
                }
                else if (e.Error is UnreadableDatabaseFileException)
                {
                    UnreadableDatabaseFileException ex = (UnreadableDatabaseFileException)e.Error;
                    ShowDatabaseErrorDialog(ex);
                }
                else
                {
                    FailHandler.HandleUnrecoverableFailure(e.Error);
                }

                Application.Current.Shutdown();
            }
            else
            {
                _conversationManager = (IConversationManager)e.Result;

                UpdateModels();

                PopulateConversationList();

                Update();

                _progressDialog.Close();

                _progressCallback = null;
                _progressDialog = null;

                ShowTextsOutOfDateWarningIfNeeded();
            }
        }

        private void ShowDatabaseErrorDialog(BackupDatabaseReadException ex)
        {
            DatabaseErrorDialog errorDialog = new DatabaseErrorDialog(ex);
            errorDialog.Owner = this;
            errorDialog.ShowDialog();
            if (errorDialog.DialogResult == true)
            {
                ShowSendFeedbackDialog();
            }
        }

        private void OnCancelProcess(object sender, EventArgs e)
        {
            _progressCallback.RequestCancel();
        }

        private void conversationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!conversationComboBox.IsEnabled)
            {
                return;
            }

            IConversation selectedConversation;

            object addedItemRaw = e.AddedItems[0];
            if (addedItemRaw != null)
            {
                IConversationListItem listItem = (IConversationListItem)addedItemRaw;

                selectedConversation = listItem.Conversation;
            }
            else
            {
                selectedConversation = null;
            }

            _conversationWindowModel.SelectedConversation = selectedConversation;

            _graphWindowModel.SelectedConversation = selectedConversation;

            Update();
        }

        public void Update()
        {
            UpdateStatistics(_mainWindowModel.ConversationStatistics);
            UpdateSelectedConversation(_mainWindowModel.SelectedConversation);

            switch (_currentView)
            {
                case ApplicationView.ConversationView:
                    UpdateConversationViewControls();
                    break;
                case ApplicationView.GraphView:
                    UpdateGraphViewControls();
                    break;
                default:
                    throw new ArgumentException("Unexpected view type.");
            }
        }

        private void UpdateSelectedConversation(IConversation conversation)
        {
            if (conversationComboBox.SelectedValue != conversation)
            {
                conversationComboBox.SelectedValue = conversation;
            }
        }

        private void UpdateConversationViewControls()
        {
            IConversation conversation = _conversationWindowModel.SelectedConversation;

            UpdateConversationPanel(conversation);
        }

        private void UpdateGraphViewControls()
        {
            GraphType comboSelectedGraphType = GraphType.Unknown;
            if (graphTypeComboBox.SelectedValue != null)
            {
                comboSelectedGraphType = (GraphType)graphTypeComboBox.SelectedValue;
            }

            if (comboSelectedGraphType != _graphWindowModel.SelectedGraphType)
            {
                graphTypeComboBox.IsEnabled = false;
                graphTypeComboBox.SelectedValue = _graphWindowModel.SelectedGraphType;
                graphTypeComboBox.IsEnabled = true;
            }

            graphControl.UpdateToModel(_graphWindowModel);
        }

        private void PopulateConversationList()
        {
            //
            // Conversation combo box must be disabled otherwise it will trigger a selection changed event when the items are cleared,
            // which will update the model to the wrong value.
            //

            conversationComboBox.IsEnabled = false;
            conversationComboBox.ItemsSource = _mainWindowModel.ConversationListItems;
            conversationComboBox.SelectedValue = _mainWindowModel.SelectedConversation;
            conversationComboBox.IsEnabled = true;

            if (conversationComboBox.SelectedValue == null)
            {
                _mainWindowModel.SelectedConversation = null;
            }
        }

        private void UpdateStatistics(IConversationStatistics stats)
        {
            if (stats == null)
            {
                return;
            }

            textsSentValueLabel.Content = stats.MessagesSent.ToString();
            textsReceivedValueLabel.Content = stats.MessagesReceived.ToString();
            textsTotalValueLabel.Content = stats.MessagesExchanged.ToString();
            daysValueLabel.Content = stats.DayCount.ToString();
            textsPerDayValueLabel.Content = string.Format("{0:0.0}", stats.MessagesPerDay);
        }

        private void UpdateConversationPanel(IConversation conversation)
        {
            conversationRenderControl.Conversation = conversation;
        }

        private void ShowTextsOutOfDateWarningIfNeeded()
        {
            if (IsOutOfDateWarningNeeded(DateTime.Today, _deviceInfo.LastSync, _displayOptions))
            {
                SyncTroubleshootingDialogView syncTroubleshootingDialog = new SyncTroubleshootingDialogView();
                syncTroubleshootingDialog.ShowDialog();
                bool promptForSyncTroubleshooting;
                if (syncTroubleshootingDialog.SuppressOutOfDateWarnings)
                {
                    promptForSyncTroubleshooting = false;
                }
                else
                {
                    promptForSyncTroubleshooting = true;
                }

                if (promptForSyncTroubleshooting != _displayOptions.PromptForSyncTroubleshooting)
                {
                    _displayOptions.PromptForSyncTroubleshooting = promptForSyncTroubleshooting;
                    _displayOptions.Save();
                }
            }
        }

        private static bool IsOutOfDateWarningNeeded(DateTime currentDate, DateTime? lastSyncDate, IDisplayOptionsReadOnly displayOptions)
        {
            const int OldAgeTimeInDays = 30;
            DateTime oldAgeDate = currentDate.Subtract(new TimeSpan(OldAgeTimeInDays, 0, 0, 0));
            if (lastSyncDate.HasValue && (lastSyncDate < oldAgeDate))
            {
                return displayOptions.PromptForSyncTroubleshooting;
            }

            return false;
        }

        private void graphTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!graphTypeComboBox.IsEnabled)
            {
                return;
            }

            if ((_graphWindowModel != null) && (graphTypeComboBox.SelectedValue != null))
            {
                _graphWindowModel.SelectedGraphType = (GraphType)graphTypeComboBox.SelectedValue;
            }

            graphControl.UpdateToModel(_graphWindowModel);
        }

        public IConversation PreviousConversation(IConversation conversation)
        {
            return _mainWindowModel.PreviousConversation(conversation);
        }

        public IConversation NextConversation(IConversation conversation)
        {
            return _mainWindowModel.NextConversation(conversation);
        }

        public IConversation CurrentConversation
        {
            get
            {
                return _mainWindowModel.SelectedConversation;
            }
            set
            {
                _mainWindowModel.SelectedConversation = value;
                Update();
            }
        }

        public object SearchTargetControl
        {
            get
            {
                return conversationRenderControl.ConversationTextBox;
            }
        }

        public void PrepareWaitForRenderComplete()
        {
            conversationRenderControl.PrepareWaitForRenderComplete();
        }

        public void WaitForRenderComplete()
        {
            conversationRenderControl.WaitForRenderComplete();
        }
    }
}

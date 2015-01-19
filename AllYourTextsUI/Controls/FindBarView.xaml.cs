using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AllYourTextsUi.Framework;

namespace AllYourTextsUi
{
    /// <summary>
    /// Interaction logic for FindBarView.xaml
    /// </summary>
    public partial class FindBarView : UserControl
    {
        private IFindDialogModel _findModel;

        public FindBarView()
        {
            InitializeComponent();
            DisableControls();
        }

        public IFindDialogModel FindModel
        {
            get
            {
                return _findModel;
            }
            set
            {
                _findModel = value;
                _findModel.SearchAllConversationsStarted += OnSearchAllConversationsStarted;
                _findModel.SearchAllConversationsCompleted += OnSearchAllConversationsCompleted;
            }
        }

        private void ViewToModel()
        {
            if (FindModel == null)
            {
                return;
            }

            FindModel.Query = findQueryTextBox.Text;

            FindModel.CaseSensitive = (matchCaseCheckBox.IsChecked == true);
        }

        private void findQueryTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ClearFailedQueryNotice();

            if (findQueryTextBox.Text == string.Empty)
            {
                DisableControls();
            }
            else
            {
                EnableControls();
                FindInCurrentDisplay(SearchDirection.Down);
            }
        }

        private void findQueryTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FindInCurrentDisplay(SearchDirection.Down);
            }
        }

        private void FindInCurrentDisplay(SearchDirection searchDirection)
        {
            if (FindModel == null)
            {
                return;
            }

            ViewToModel();

            if (string.IsNullOrEmpty(FindModel.Query))
            {
                return;
            }

            FindModel.SearchDisplayedConversation(searchDirection);

            findQueryTextBox.Focus();

            if (!FindModel.LastQuerySuccessful)
            {
                ShowFailedQueryNotice();
            }
            else
            {
                ClearFailedQueryNotice();
            }
        }

        private void SearchAllConversations()
        {
            if (FindModel == null)
            {
                return;
            }

            ViewToModel();

            FindModel.SearchAllConversations();
        }

        private void ShowFailedQueryNotice()
        {
            findQueryTextBox.Background = Brushes.LightSalmon;
        }

        private void ClearFailedQueryNotice()
        {
            findQueryTextBox.Background = Brushes.White;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            CancelSearch();

            this.Visibility = Visibility.Collapsed;
        }

        private void searchAllConversationsButton_Click(object sender, RoutedEventArgs e)
        {
            SearchAllConversations();
        }

        void OnSearchAllConversationsStarted(object sender, EventArgs e)
        {
            DisplaySearchInProgress();
        }

        void OnSearchAllConversationsCompleted(object sender, EventArgs e)
        {
            EndDisplaySearchInProgress();
            if (FindModel.LastQuerySuccessful)
            {
                ClearFailedQueryNotice();
            }
            else
            {
                ShowFailedQueryNotice();
                MessageBox.Show("No results found.", "Search Result", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }
        }

        private void DisplaySearchInProgress()
        {
            searchControls.IsEnabled = false;
            searchInProgressPanel.Visibility = Visibility.Visible;
        }

        private void EndDisplaySearchInProgress()
        {
            searchControls.IsEnabled = true;
            searchInProgressPanel.Visibility = Visibility.Hidden;
        }

        private void cancelSearchButton_Click(object sender, RoutedEventArgs e)
        {
            CancelSearch();
        }

        private void CancelSearch()
        {
            _findModel.CancelSearch();
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            FindInCurrentDisplay(SearchDirection.Up);
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            FindInCurrentDisplay(SearchDirection.Down);
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible == true)
            {
                this.Dispatcher.BeginInvoke((Action)delegate
                {
                    Keyboard.Focus(findQueryTextBox);
                }, System.Windows.Threading.DispatcherPriority.Render);
            }
        }

        private void EnableControls()
        {
            SetControlsIsEnabledState(true);
        }

        private void DisableControls()
        {
            SetControlsIsEnabledState(false);
        }

        private void SetControlsIsEnabledState(bool isEnabled)
        {
            upButton.IsEnabled = isEnabled;
            downButton.IsEnabled = isEnabled;
            matchCaseCheckBox.IsEnabled = isEnabled;
            searchAllConversationsButton.IsEnabled = isEnabled;
        }
    }
}

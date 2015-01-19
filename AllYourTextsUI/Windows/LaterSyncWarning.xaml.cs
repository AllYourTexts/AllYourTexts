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
using System.Windows.Shapes;

namespace AllYourTextsUi.Windows
{
    /// <summary>
    /// Interaction logic for LaterSyncWarning.xaml
    /// </summary>
    public partial class LaterSyncWarning : Window
    {
        public bool IgnoreMoreRecentSyncs { get; set; }

        public LaterSyncWarning()
        {
            InitializeComponent();

            IgnoreMoreRecentSyncs = false;

            Loaded += delegate
            {
                DataContext = this;
            };
        }

        private void yesButton_Click(object sender, RoutedEventArgs e)
        {
            IgnoreMoreRecentSyncs = false;
            this.DialogResult = true;
        }

        private void noButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}

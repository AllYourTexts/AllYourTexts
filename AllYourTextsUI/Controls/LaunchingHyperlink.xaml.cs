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
using System.Diagnostics;

namespace AllYourTextsUi
{
    /// <summary>
    /// Interaction logic for LaunchingHyperlink.xaml
    /// </summary>
    public partial class LaunchingHyperlink : Hyperlink
    {
        public LaunchingHyperlink()
        {
            InitializeComponent();

            RequestNavigate += OnRequestNavigate;
        }

        public string Text
        {
            set
            {
                this.Inlines.Clear();
                this.Inlines.Add(new Run(value));
            }
        }

        private void OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}

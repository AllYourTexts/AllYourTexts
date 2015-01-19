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
using AllYourTextsUi.Framework;

namespace AllYourTextsUi
{
    /// <summary>
    /// Interaction logic for AboutBoxView.xaml
    /// </summary>
    public partial class AboutBoxView : Window
    {
        private IAboutBoxModel _aboutBoxModel;

        public AboutBoxView(IAboutBoxModel model)
        {
            InitializeComponent();

            _aboutBoxModel = model;

            Loaded += delegate
                {
                    ModelToView();
                };
        }

        private void ModelToView()
        {
            buildDateValueLabel.Content = _aboutBoxModel.BuildDateString;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}

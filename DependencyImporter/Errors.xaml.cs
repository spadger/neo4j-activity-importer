using System;
using System.Collections.Generic;
using System.Windows;

namespace DependencyImporter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Errors : Window
    {
        public Errors()
        {
            InitializeComponent();
        }

        public void SetErrors(IEnumerable<string> errors)
        {
            txtErrors.Text = string.Join(Environment.NewLine, errors);
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

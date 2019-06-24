using CsautoLicker.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CsautoLicker.Views
{
    /// <summary>
    /// Interaction logic for ClickerMainWindow.xaml
    /// </summary>
    public partial class ClickerMainWindow : Window
    {
        public ClickerMainWindow()
        {
            InitializeComponent();
            this.DataContext = ViewModel = new ClickerMainViewModell();
        }

        public ClickerMainViewModell ViewModel { get; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Tabs.SelectedIndex = 0;
        }
    }
}

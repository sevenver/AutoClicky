﻿using CsautoLicker.Helpers;
using CsautoLicker.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CsautoLicker.Views
{
    /// <summary>
    /// Interaction logic for ClickerUserControl.xaml
    /// </summary>
    public partial class ClickerUserControl : UserControl
    {
        public ClickerUserControl()
        {
            InitializeComponent();
            this.DataContext = ViewModel = new ClickerViewModell();
            Title = ViewModel.Title;
        }


        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ClickerUserControl), new PropertyMetadata(""));


        public ClickerViewModell ViewModel { get; }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            
            Regex regex = new Regex("^-?\\d * (\\.\\d +)?$");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}

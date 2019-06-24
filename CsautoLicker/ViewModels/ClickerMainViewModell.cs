using CsautoLicker.Helpers;
using CsautoLicker.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;

namespace CsautoLicker.ViewModels
{
    public class ClickerMainViewModell : BaseViewModell
    {
        public ClickerMainViewModell() : base("Zolika egy Buzika")
        {
            
            var tabTemp = new ClickerUserControl();
            TabPages.Add(new TabItem() { Header = tabTemp.Title, Content = tabTemp });
            var tabTemp2 = new SettingsUserControl();
            TabPages.Add(new TabItem() { Header = tabTemp2.Title, Content = tabTemp2});
        }
        public ObservableCollection<TabItem> TabPages { get; set; } = new ObservableCollection<TabItem>();
    }
}

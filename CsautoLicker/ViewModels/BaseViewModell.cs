using CsautoLicker.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsautoLicker.ViewModels
{
    public  class BaseViewModell : Observable
    {
        public BaseViewModell(string title)
        {
            Title = title;
        }
        private string title;

        public string Title
        {
            get { return title; }
            set { Set(ref title, value); }
        }


    }
}

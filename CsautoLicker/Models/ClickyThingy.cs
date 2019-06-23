using CsautoLicker.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace CsautoLicker.Models
{
    public class ClickyThingy : Observable
    {
        public ClickyThingy()
        {
            enabled = true;
        }
        private Point location;

        public Point Location
        {
            get { return location; }
            set { location = value; }
        }

        private int delay;

        public int Delay
        {
            get { return delay; }
            set { Set(ref delay, value); }
        }

        private bool enabled;

        public bool Enabled
        {
            get { return enabled; }
            set {Set(ref enabled, value); }
        }

        private int repats;

        public int Repeats
        {
            get { return repats; }
            set {Set(ref repats , value); }
        }




    }
}

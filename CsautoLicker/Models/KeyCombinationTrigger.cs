
using LowLevelInput.Hooks;
using System;

namespace CsautoLicker.Models
{
    public class KeyCombinationTrigger
    {
        public string Name { get; set; }
        public VirtualKeyCode Keys { get; set; }
        public Action Execute { get; set; }
        public bool Control { get; set; }
        public bool Shift { get; set; }

        public KeyCombinationTrigger(Action execute, VirtualKeyCode keys, string name, bool shift = false, bool control = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("message", nameof(name));
            }

            Execute = execute ?? throw new ArgumentNullException(nameof(execute));
            Keys = keys;
            Name = name;
            Control = control;
            Shift = shift;
        }

        private string StringCombination()
        {
            string returnString = "";
            if (Control) returnString += "Control + ";
            if (Shift) returnString += "Shift + ";
            returnString += Keys;
            return returnString;
        }
        public string CombinedName { get => StringCombination(); }

        public bool Compare(bool shift, bool ctrl, VirtualKeyCode key)
        {
            return shift == Shift && ctrl == Control && key == Keys;
        }
    }
}

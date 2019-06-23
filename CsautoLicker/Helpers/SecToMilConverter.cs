using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace CsautoLicker.Helpers
{
    public class SecToMilConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.TryParse(value.ToString(), out int milisecs))
            {
                return TimeSpan.FromMilliseconds(milisecs).TotalSeconds;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.TryParse(value.ToString(), out int seconds))
            {
                var milisecs = TimeSpan.FromSeconds(seconds).TotalMilliseconds;
                if (milisecs == 0)
                {
                    return 20;

                }
                else
                {
                    return milisecs;
                }
            }
            return DependencyProperty.UnsetValue;

        }
    }
}

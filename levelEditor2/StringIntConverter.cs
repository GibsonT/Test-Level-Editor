using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace levelEditor2
{
    class StringIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int intValue;
            if (!Int32.TryParse(value.ToString(), out intValue))
            {
                return 0;
            }
            else
            {
                return intValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string stringValue = value.ToString();
            return stringValue;
        }
    }
}

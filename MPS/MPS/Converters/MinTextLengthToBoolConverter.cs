using System;
using System.Globalization;
using Xamarin.Forms;

namespace MPS.Converters
{
    public class MinTextLengthToBoolConverter : IValueConverter
    {
        public int MinLength { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value >= MinLength;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}

using System;
using System.Globalization;
using Xamarin.Forms;

namespace MPS.Converters
{
    public class TextLengthToLeftCharactersConverter : IValueConverter
    {
        public int MaxMessageLength { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {           
            return MaxMessageLength - (int) value + "/" + MaxMessageLength;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}

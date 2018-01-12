using System;
using System.Globalization;
using Xamarin.Forms;

namespace MPS.Converters
{
    public class DoubleToLeftCharactersConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value + "/" + Application.Current.Resources["MaxMessageLength"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}

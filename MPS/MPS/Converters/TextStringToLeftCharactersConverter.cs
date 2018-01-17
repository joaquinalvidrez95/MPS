using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MPS.Converters
{
    public class TextStringToLeftCharactersConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var x = ((string) value)?.Length ?? 0;
            return (int)Application.Current.Resources["MaxMessageLength"] - x + "/" + Application.Current.Resources["MaxMessageLength"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}

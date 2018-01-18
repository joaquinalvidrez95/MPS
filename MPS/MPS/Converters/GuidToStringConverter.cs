using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MPS.Converters
{
    public class GuidToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "Dirección desconocida";
            //return (Guid)value == null ?  : ((Guid)value).ToString();

            var guid = ((Guid)value).ToString().Substring(24);
            return guid.Substring(0, 2) + ":" + guid.Substring(2, 2) + ":" + guid.Substring(4, 2) + ":" + guid.Substring(6, 2) + ":" + guid.Substring(8, 2) + ":" + guid.Substring(10, 2);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}

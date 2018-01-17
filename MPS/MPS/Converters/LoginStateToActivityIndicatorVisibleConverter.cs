using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPS.Model;
using Xamarin.Forms;

namespace MPS.Converters
{
    public class LoginStateToActivityIndicatorVisibleConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((PasswordLoginState)value)
            {
                case PasswordLoginState.Normal:
                    return false;
                case PasswordLoginState.PasswordInvalid:
                    return false;
                case PasswordLoginState.WaitingForRequest:
                    return true;
                case PasswordLoginState.TimeoutExpired:
                    return false;
                default:
                    return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
}

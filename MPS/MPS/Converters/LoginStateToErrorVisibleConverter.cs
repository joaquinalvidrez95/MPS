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
    public class LoginStateToErrorVisibleConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((PasswordLoginState)value)
            {
                case PasswordLoginState.Normal:
                    return false;                   
                case PasswordLoginState.PasswordInvalid:
                    return true;
                case PasswordLoginState.WaitingForRequest:
                    return false;
                case PasswordLoginState.TimeoutExpired:
                    return true;
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

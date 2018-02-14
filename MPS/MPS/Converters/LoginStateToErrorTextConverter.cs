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
    public class LoginStateToErrorTextConverter : IValueConverter
    {
        public string PasswordInvalidText { get; set; }
        public string TimeoutExpiredText { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((PasswordLoginState)value)
            {
                case PasswordLoginState.Normal:
                    return "";
                case PasswordLoginState.PasswordInvalid:
                    return PasswordInvalidText;
                case PasswordLoginState.WaitingForRequest:
                    return "";
                case PasswordLoginState.TimeoutExpired:
                    return TimeoutExpiredText;
                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}

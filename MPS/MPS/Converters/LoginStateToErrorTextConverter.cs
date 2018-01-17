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
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((PasswordLoginState)value)
            {
                case PasswordLoginState.Normal:
                    return "";
                case PasswordLoginState.PasswordInvalid:
                    return (string)Application.Current.Resources["TextPasswordIncorrect"];
                case PasswordLoginState.WaitingForRequest:
                    return "";
                case PasswordLoginState.TimeoutExpired:
                    return (string)Application.Current.Resources["TextTimeExpired"];
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

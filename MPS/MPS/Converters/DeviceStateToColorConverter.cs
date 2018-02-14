using System;
using System.Globalization;
using Plugin.BLE.Abstractions;
using Xamarin.Forms;

namespace MPS.Converters
{
    public class DeviceStateToColorConverter : IValueConverter
    {
        public Color ConnectedColor { get; set; }
        public Color DisconnectedColor { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {           
            return (DeviceState)value == DeviceState.Connected ? ConnectedColor : DisconnectedColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DeviceState.Connected;
        }
    }
}

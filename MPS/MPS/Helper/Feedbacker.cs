using MPS.Model;
using Xamarin.Forms;

namespace MPS.Helper
{
    public class Feedbacker
    {
        public void SendUiParameters(byte[] bytes)
        {
            MessagingCenter.Send(this, MessengerKeys.Power, bytes[1] - 48 == 1);
            MessagingCenter.Send(this, MessengerKeys.Speed, bytes[2] - 48);
            MessagingCenter.Send(this, MessengerKeys.CurrentView, bytes[3] - 48);
            MessagingCenter.Send(this, MessengerKeys.TimeFormat, (TimeFormat)(bytes[4] - 48));

            var display = new DisplayColors
            {
                ColorUpperLineRgb =
                {
                    ColorCode = (bytes[5] - 48).ToString()+ (bytes[6] - 48)+(bytes[7] - 48)
                },
                ColorLowerLineRgb =
                {
                    ColorCode = (bytes[8] - 48).ToString()+ (bytes[9] - 48)+(bytes[10] - 48)
                },
                ColorBackgroundRgb =
                {
                    ColorCode = (bytes[11] - 48).ToString()+ (bytes[12] - 48)+(bytes[13] - 48)
                }
            };
            MessagingCenter.Send(this, MessengerKeys.Colours, display);

            MessagingCenter.Send(this, MessengerKeys.ViewMode, (ViewMode)(bytes[14] - 48));
            var displayVisibility = new DisplayVisibility
            {
                IsTimeVisible = bytes[15] - 48 == 1,
                IsTemperatureVisible = bytes[16] - 48 == 1,
                IsDateVisible = bytes[17] - 48 == 1,
            };
            MessagingCenter.Send(this, MessengerKeys.Visibilities, displayVisibility);
        }
    }
}

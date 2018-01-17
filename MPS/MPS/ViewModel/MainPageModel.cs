using MPS.Bluetooth;
using MPS.Utilities;
using MPS.View;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Plugin.BLE.Abstractions.EventArgs;
using MPS.Model;
using Rg.Plugins.Popup.Services;

namespace MPS.ViewModel
{
    public class MainPageModel : BaseViewModel
    {
        private IDevice _connectedDevice;
        //private bool _hasFeedback;
        private bool _isFixingControls;
        private const int TimeoutForFixingControls = 20;
        public ICommand BluetoothConnectionCommand { get; }
        public ICommand AboutCommand { get; }
        //private INavigation Navigation { get; }

        //private const int Timeout = 1500;

        public MainPageModel()
        {
            BluetoothConnectionCommand = new Command(GoToBluetoothDevicesPageAsync);
            AboutCommand = new Command(GoToAboutPage);
            //Navigation = navigation;
            CrossBluetoothLE.Current.Adapter.DeviceConnected += OnDeviceStateChanged;
            CrossBluetoothLE.Current.Adapter.DeviceConnectionLost += OnDeviceConnectionLost;
            CrossBluetoothLE.Current.Adapter.DeviceDisconnected += OnDeviceStateChanged;


        }

        private void OnDeviceConnectionLost(object sender, DeviceErrorEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() => Application.Current.MainPage.DisplayAlert(
                (string)Application.Current.Resources["DisplayAlertTitleError"],
                 (string)Application.Current.Resources["DisplayAlertMessageConexionLost"] + e.Device.Name,
                (string)Application.Current.Resources["DisplayAlertCancelAccept"]
            ));

            MessagingCenter.Send(this, MessengerKeys.DeviceStatus, e.Device);
        }

        private async void GoToAboutPage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AboutPage());
        }


        private async void OnDeviceSelected(BluetoothDevicesPageModel arg1, IDevice device)
        {

            try
            {
                await CrossBluetoothLE.Current.Adapter.ConnectToDeviceAsync(device);
                _connectedDevice = device;
                //var service = await device.GetServiceAsync(Guid.Parse(BluetoothHelper.BluetoothUuid.ServiceUuid));
                //var characteristic = await service.GetCharacteristicAsync(Guid.Parse(BluetoothHelper.BluetoothUuid.CharacteristicUuid));
                //characteristic.ValueUpdated += OnDataReceived;
                //await characteristic.StartUpdatesAsync();
                //_hasFeedback = false;

                //Device.StartTimer(TimeSpan.FromMilliseconds(Timeout), () =>
                //{
                //    if (!_hasFeedback)
                //    {
                //        RequestParameters();
                //    }
                //    return !_hasFeedback;
                //});
            }
            catch (DeviceConnectionException)
            {
                Debug.WriteLine("Error al conectar");
            }

        }

        private async void OnDeviceToDisconnectReceived(BluetoothDevicesPageModel bluetoothDevicesPageModel, IDevice device)
        {
            await CrossBluetoothLE.Current.Adapter.DisconnectDeviceAsync(device);

        }

        private void OnDeviceSelected(PasswordPopupModel arg1, IDevice arg2)
        {
            _isFixingControls = false;
            CrossBluetoothLE.Current.Adapter.DeviceConnectionLost += OnDeviceConnectionLost;
            _connectedDevice = arg2;
            MessagingCenter.Send(this, MessengerKeys.DeviceStatus, arg2);
        }

        //private void OnDataReceived(object sender, CharacteristicUpdatedEventArgs e)
        //{

        //    var bytes = e.Characteristic.Value;

        //    var x = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        //    //Debug.WriteLine(x);
        //    switch (x[0].ToString())
        //    {
        //        case BluetoothHelper.BluetoothContract.Feedback:
        //            _hasFeedback = true;
        //            _isFixingControls = true;
        //            SendUiParameters(bytes);

        //            Device.StartTimer(TimeSpan.FromMilliseconds(TimeoutForFixingControls), () =>
        //            {
        //                _isFixingControls = false;
        //                return false;
        //            });

        //            break;
        //    }
        //}

        private void SendUiParameters(byte[] bytes)
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

      
        private void OnPowerStatusReceived(MainParametersPageModel arg1, bool arg2)
        {
            string data = MessengerKeys.Power + (arg2 ? 1 : 0) + '\n';
            WriteData(data);
        }

        private void OnQuickMessageReceived(QuickMessagePopupModel quickMessagePopupModel, Message message)
        {
            SendMessage(message.Text);
        }

        private void OnMessageStoredReceived(MessagePageModel arg1, Message message)
        {
            SendMessage(message.Text);
        }

        private void SendMessage(string message)
        {
            var data = MessengerKeys.Message + message + "   " + '\n';
            int i;
            const int max = 15;
            for (i = 0; i < data.Length / max; i++)
            {
                WriteData(data.Substring(i * max, max));
                Task.Delay(1);
            }
            if (data.Length % max != 0)
                WriteData(data.Substring(i * max, data.Length % max));
        }

        private void OnColoursReceived(ColorsPageModel arg1, DisplayColors arg2)
        {
            string data = MessengerKeys.Colours + arg2.GetColorCode() + '\n';
            WriteData(data);
        }

        private void OnDateTimeReceived(MainParametersPageModel arg1, DateTime arg2)
        {
            var data =
                MessengerKeys.DateTime
                + arg2.ToString("HHmmssddMMyy")
                + '\n';
            WriteData(data);
        }

        private void OnDeviceStateChanged(object sender, DeviceEventArgs e)
        {
            switch (e.Device.State)
            {
                case DeviceState.Disconnected:
                    MessagingCenter.Send(this, MessengerKeys.DeviceStatus, e.Device);
                    break;
                case DeviceState.Connecting:
                    break;
                case DeviceState.Connected:
                    CrossBluetoothLE.Current.Adapter.DeviceConnectionLost -= OnDeviceConnectionLost;                    
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await PopupNavigation.PushAsync(new PasswordPopup());
                        MessagingCenter.Send(this, MessengerKeys.DeviceSelected, e.Device);
                    });
                    break;


            }
        }

        private void OnSpeedReceived(MessagePageModel messagePageModel, int arg2)
        {
            string data = MessengerKeys.Speed + arg2 + '\n';
            WriteData(data);
        }

        private void OnViewReceived(MainParametersPageModel arg1, int arg2)
        {
            string data = MessengerKeys.CurrentView + arg2 + '\n';
            WriteData(data);
        }

        private void OnDisplayVisibilityReceived(VisibilityPageModel visibilityPageModel, DisplayVisibility visibility)
        {
            var data = MessengerKeys.Visibilities
                + (visibility.IsTimeVisible ? 1 : 0)
                + (visibility.IsTemperatureVisible ? 1 : 0)
                + (visibility.IsDateVisible ? 1 : 0)
                + '\n';
            WriteData(data);
        }

        private void OnViewModeReceived(VisibilityPageModel visibilityPageModel, ViewMode viewMode)
        {
            string data = MessengerKeys.ViewMode + (int)viewMode + '\n';
            WriteData(data);
        }

        private void OnTimeFormatReceived(VisibilityPageModel visibilityPageModel, TimeFormat timeFormat)
        {
            string data = MessengerKeys.TimeFormat + (int)timeFormat + '\n';
            WriteData(data);
        }

        private async void WriteData(string data)
        {
            if (_isFixingControls) return;
            if (_connectedDevice == null)
            {
                return;
            }
            if (_connectedDevice?.State != DeviceState.Connected)
            {
                MessagingCenter.Send(this, MessengerKeys.DeviceStatus, _connectedDevice);
                return;
            }
            var service = await _connectedDevice.GetServiceAsync(Guid.Parse(BluetoothHelper.BluetoothUuid.ServiceUuid));
            var characteristic = await service.GetCharacteristicAsync(Guid.Parse(BluetoothHelper.BluetoothUuid.CharacteristicUuid));
            var array = Encoding.UTF8.GetBytes(data);
            await characteristic.WriteAsync(array);
            Debug.WriteLine(GetType() + "Se envío: " + data);
        }

        private async void GoToBluetoothDevicesPageAsync()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new BluetoothDevicesPage());
            //CrossBluetoothLE.Current.Adapter.DeviceConnectionLost += OnDeviceDisconnected;
        }


        protected override void Subscribe()
        {
            MessagingCenter.Subscribe<BluetoothDevicesPageModel, IDevice>(this, MessengerKeys.DeviceSelected, OnDeviceSelected);
            MessagingCenter.Subscribe<BluetoothDevicesPageModel, IDevice>(this, MessengerKeys.DeviceToDisconnect, OnDeviceToDisconnectReceived);
            MessagingCenter.Subscribe<PasswordPopupModel, IDevice>(this, MessengerKeys.DeviceSelected, OnDeviceSelected);
            MessagingCenter.Subscribe<PasswordPopupModel>(this, MessengerKeys.FeedbackStarted, OnControlsFixed);
            MessagingCenter.Subscribe<MainParametersPageModel, bool>(this, MessengerKeys.Power, OnPowerStatusReceived);
            MessagingCenter.Subscribe<MainParametersPageModel, int>(this, MessengerKeys.CurrentView, OnViewReceived);
            MessagingCenter.Subscribe<MessagePageModel, int>(this, MessengerKeys.Speed, OnSpeedReceived);
            MessagingCenter.Subscribe<MainParametersPageModel, DateTime>(this, MessengerKeys.DateTime, OnDateTimeReceived);
            MessagingCenter.Subscribe<QuickMessagePopupModel, Message>(this, MessengerKeys.Message, OnQuickMessageReceived);
            MessagingCenter.Subscribe<MessagePageModel, Message>(this, MessengerKeys.Message, OnMessageStoredReceived);
            MessagingCenter.Subscribe<ColorsPageModel, DisplayColors>(this, MessengerKeys.Colours, OnColoursReceived);
            MessagingCenter.Subscribe<VisibilityPageModel, DisplayVisibility>(this, MessengerKeys.Visibilities, OnDisplayVisibilityReceived);
            MessagingCenter.Subscribe<VisibilityPageModel, TimeFormat>(this, MessengerKeys.TimeFormat, OnTimeFormatReceived);
            MessagingCenter.Subscribe<VisibilityPageModel, ViewMode>(this, MessengerKeys.ViewMode, OnViewModeReceived);

        }



        private void OnControlsFixed(PasswordPopupModel passwordPopupModel)
        {
            _isFixingControls = true;
        }
    }
}

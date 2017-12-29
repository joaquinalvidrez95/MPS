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

namespace MPS.ViewModel
{
    public class MainPageModel : BaseViewModel
    {
        private IDevice _connectedDevice;
        private bool _hasFeedback;
        private bool _isFixingControls;
        public ICommand BluetoothConnectionCommand { get; }
        private INavigation Navigation { get; }
        private const int Timeout = 2000;

        public MainPageModel(INavigation navigation)
        {
            BluetoothConnectionCommand = new Command(GoToBluetoothDevicesPageAsync);
            Navigation = navigation;
            CrossBluetoothLE.Current.Adapter.DeviceConnected += OnDeviceStateChanged;
            CrossBluetoothLE.Current.Adapter.DeviceDisconnected += OnDeviceStateChanged;

        }


        private async void OnDeviceSelected(BluetoothDevicesPageModel arg1, IDevice arg2)
        {
            if (arg2 == null)
            {
                return;
            }

            try
            {
                _connectedDevice = arg2;
                await CrossBluetoothLE.Current.Adapter.ConnectToDeviceAsync(_connectedDevice);

                var service = await _connectedDevice.GetServiceAsync(Guid.Parse(BluetoothHelper.BluetoothUuid.ServiceUuid));
                var characteristic = await service.GetCharacteristicAsync(Guid.Parse(BluetoothHelper.BluetoothUuid.CharacteristicUuid));
                characteristic.ValueUpdated += OnDataReceived;
                await characteristic.StartUpdatesAsync();
                _hasFeedback = false;

                Device.StartTimer(TimeSpan.FromMilliseconds(Timeout), () =>
                {
                    if (!_hasFeedback)
                    {
                        RequestParameters();
                    }
                    return !_hasFeedback;
                });
            }
            catch (DeviceConnectionException)
            {
                Debug.WriteLine("Error al conectar");
            }
        }

        private void OnDataReceived(object sender, CharacteristicUpdatedEventArgs e)
        {

            var bytes = e.Characteristic.Value;

            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    
            //});
            var x = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            Debug.WriteLine(x);
            switch (x[0].ToString())
            {
                case BluetoothHelper.BluetoothContract.Feedback:
                    _hasFeedback = true;
                    _isFixingControls = true;
                    MessagingCenter.Send(this, MessengerKeys.Power, (bytes[1] - 48) == 1);
                    MessagingCenter.Send(this, MessengerKeys.Speed, bytes[2] - 48);
                    MessagingCenter.Send(this, MessengerKeys.CurrentView, bytes[3] - 48);
                    MessagingCenter.Send(this, MessengerKeys.TimeFormat, (TimeFormat)(bytes[4] - 48));
                    var display = new DisplayColors
                    {
                        ColorUpperLineRgb =
                        {
                            Red = bytes[5] - 48,
                            Green = bytes[6] - 48,
                            Blue = bytes[7] - 48
                        },
                        ColorLowerLineRgb =
                        {
                            Red = bytes[8] - 48,
                            Green = bytes[9] - 48,
                            Blue = bytes[10] - 48
                        },
                        ColorBackgroundRgb =
                        {
                            Red = bytes[11] - 48,
                            Green = bytes[12] - 48,
                            Blue = bytes[13] - 48
                        }
                    };
                    MessagingCenter.Send(this, MessengerKeys.Colours, display);

                    MessagingCenter.Send(this, MessengerKeys.ViewMode, (ViewMode)(bytes[14] - 48));
                    var displayVisibility = new DisplayVisibility
                    {
                        IsTimeVisible = (bytes[15] - 48) == 1,
                        IsTemperatureVisible = (bytes[16] - 48) == 1,
                        IsDateVisible = (bytes[17] - 48) == 1,
                    };
                    MessagingCenter.Send(this, MessengerKeys.Visibilities, displayVisibility);


                    _isFixingControls = false;
                    break;
            }
        }

        private void RequestParameters()
        {
            WriteData(BluetoothHelper.BluetoothContract.Request + '\n');
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
            string data = MessengerKeys.Colours + arg2.ColorCode() + '\n';
            WriteData(data);
        }

        private void OnDateTimeReceived(MainParametersPageModel arg1, DateTime arg2)
        {
            string data =
                MessengerKeys.DateTime
                + arg2.ToString("HHmmssddMMyy")
                + '\n';
            WriteData(data);
        }

        private void OnDeviceStateChanged(object sender, DeviceEventArgs e)
        {
            MessagingCenter.Send(this, MessengerKeys.DeviceStatus, e.Device);
        }

        private void OnSpeedReceived(MainParametersPageModel arg1, int arg2)
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
            Debug.WriteLine("Written data: " + data);
        }

        private async void GoToBluetoothDevicesPageAsync()
        {
            await Navigation.PushAsync(new BluetoothDevicesPage());
        }


        protected override void Subscribe()
        {
            MessagingCenter.Subscribe<BluetoothDevicesPageModel, IDevice>(this, MessengerKeys.DeviceSelected, OnDeviceSelected);
            MessagingCenter.Subscribe<MainParametersPageModel, bool>(this, MessengerKeys.Power, OnPowerStatusReceived);
            MessagingCenter.Subscribe<MainParametersPageModel, int>(this, MessengerKeys.CurrentView, OnViewReceived);
            MessagingCenter.Subscribe<MainParametersPageModel, int>(this, MessengerKeys.Speed, OnSpeedReceived);
            MessagingCenter.Subscribe<MainParametersPageModel, DateTime>(this, MessengerKeys.DateTime, OnDateTimeReceived);
            //MessagingCenter.Subscribe<QuickMessagePopupModel, Message>(this, MessengerKeys.QuickMessage, OnQuickMessageReceived);
            MessagingCenter.Subscribe<QuickMessagePopupModel, Message>(this, MessengerKeys.Message, OnQuickMessageReceived);
            MessagingCenter.Subscribe<MessagePageModel, Message>(this, MessengerKeys.Message, OnMessageStoredReceived);
            MessagingCenter.Subscribe<ColorsPageModel, DisplayColors>(this, MessengerKeys.Colours, OnColoursReceived);
            MessagingCenter.Subscribe<VisibilityPageModel, DisplayVisibility>(this, MessengerKeys.Visibilities, OnDisplayVisibilityReceived);
            MessagingCenter.Subscribe<VisibilityPageModel, TimeFormat>(this, MessengerKeys.TimeFormat, OnTimeFormatReceived);
            MessagingCenter.Subscribe<VisibilityPageModel, ViewMode>(this, MessengerKeys.ViewMode, OnViewModeReceived);
        }


    }
}

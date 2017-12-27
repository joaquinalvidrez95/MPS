using MPS.Bluetooth;
using MPS.Utilities;
using MPS.View;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
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
        public ICommand BluetoothConnectionCommand { get; }
        private INavigation Navigation { get; }
        private const int Timeout = 500;

        public MainPageModel(INavigation navigation)
        {
            BluetoothConnectionCommand = new Command(GoToBluetoothDevicesPageAsync);
            Navigation = navigation;
            CrossBluetoothLE.Current.Adapter.DeviceConnected += OnDeviceStateChanged;
            CrossBluetoothLE.Current.Adapter.DeviceDisconnected += OnDeviceStateChanged;
            MessagingCenter.Subscribe<BluetoothDevicesPageModel, IDevice>(this, MessengerKeys.DeviceSelected, ConnectDevice);
            MessagingCenter.Subscribe<MainParametersPageModel, bool>(this, MessengerKeys.Power, SendPowerStatus);
            MessagingCenter.Subscribe<MainParametersPageModel, int>(this, MessengerKeys.CurrentView, SendViewAsync);
            MessagingCenter.Subscribe<MainParametersPageModel, int>(this, MessengerKeys.Speed, SendSpeedAsync);
            MessagingCenter.Subscribe<MainParametersPageModel, DateTime>(this, MessengerKeys.DateTime, SendDateTime);
            MessagingCenter.Subscribe<MainParametersPageModel, string>(this, MessengerKeys.QuickMessage, SendQuickMessage);
            MessagingCenter.Subscribe<MessagePageModel, string>(this, MessengerKeys.Message, SendMessageStored);
            MessagingCenter.Subscribe<ColorsPageModel, DisplayColors>(this, MessengerKeys.Colours, SendColours);

        }


        private async void ConnectDevice(BluetoothDevicesPageModel arg1, IDevice arg2)
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
                characteristic.ValueUpdated += (o, args) =>
                {
                    _hasFeedback = true;
                    var bytes = args.Characteristic.Value;
                    //Device.BeginInvokeOnMainThread(() =>
                    //{
                    //    labelInbox.Text = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                    //});
                    MessagingCenter.Send(this, MessengerKeys.Power, (bytes[1] - 48) == 1);
                    MessagingCenter.Send(this, MessengerKeys.Speed, bytes[2] - 48);
                    MessagingCenter.Send(this, MessengerKeys.CurrentView, bytes[3] - 48);
                    var display = new DisplayColors
                    {
                        ColorUpperLineRgb =
                        {
                            Red = bytes[4] - 48,
                            Green = bytes[5] - 48,
                            Blue = bytes[6] - 48
                        },
                        ColorLowerLineRgb =
                        {
                            Red = bytes[7] - 48,
                            Green = bytes[8] - 48,
                            Blue = bytes[9] - 48
                        },
                        ColorBackgroundRgb =
                        {
                            Red = bytes[10] - 48,
                            Green = bytes[11] - 48,
                            Blue = bytes[12] - 48
                        }
                    };
                    MessagingCenter.Send(this, MessengerKeys.Message, display);


                };

                await characteristic.StartUpdatesAsync();
                _hasFeedback = false;
                RequestParameters();

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

            }
        }

        private void RequestParameters()
        {
            WriteData(BluetoothHelper.BluetoothContract.Request + '\n');
        }

        private void SendPowerStatus(MainParametersPageModel arg1, bool arg2)
        {
            string data = BluetoothHelper.BluetoothContract.Power + (arg2 ? 1 : 0) + '\n';
            WriteData(data);
        }


        private void SendQuickMessage(MainParametersPageModel arg1, string arg2)
        {
            SendMessage(arg2);
        }

        private void SendMessageStored(MessagePageModel arg1, string arg2)
        {
            SendMessage(arg2);
        }

        private void SendMessage(string message)
        {
            var data = BluetoothHelper.BluetoothContract.Message + message + "   " + '\n';
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

        private void SendColours(ColorsPageModel arg1, DisplayColors arg2)
        {
            string data = BluetoothHelper.BluetoothContract.Colours + arg2.ColorCode() + '\n';
            WriteData(data);
        }

        private void SendDateTime(MainParametersPageModel arg1, DateTime arg2)
        {
            string data =
                BluetoothHelper.BluetoothContract.DateTime
                + arg2.ToString("HHmmssddMMyy")
                + '\n';
            WriteData(data);
        }

        private void OnDeviceStateChanged(object sender, DeviceEventArgs e)
        {
            MessagingCenter.Send(this, MessengerKeys.DeviceStatus, e.Device);
        }

        private void SendSpeedAsync(MainParametersPageModel arg1, int arg2)
        {
            string data = BluetoothHelper.BluetoothContract.Speed + arg2 + '\n';
            WriteData(data);
        }

        private void SendViewAsync(MainParametersPageModel arg1, int arg2)
        {
            string data = BluetoothHelper.BluetoothContract.View + arg2 + '\n';
            WriteData(data);
        }

        private async void WriteData(string data)
        {
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
        }

        private async void GoToBluetoothDevicesPageAsync()
        {
            await Navigation.PushAsync(new BluetoothDevicesPage());
        }


    }
}

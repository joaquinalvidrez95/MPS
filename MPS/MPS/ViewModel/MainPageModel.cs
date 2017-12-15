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
        public ICommand BluetoothConnectionCommand { get; }
        private INavigation Navigation { get; }

        public MainPageModel(INavigation navigation)
        {
            BluetoothConnectionCommand = new Command(GoToBluetoothDevicesPageAsync);
            Navigation = navigation;
            CrossBluetoothLE.Current.Adapter.DeviceConnected += OnDeviceStateChanged;
            CrossBluetoothLE.Current.Adapter.DeviceDisconnected += OnDeviceStateChanged;
            MessagingCenter.Subscribe<BluetoothDevicesPageModel, IDevice>(this, MessengerKeys.DeviceSelected, async (sender, arg) =>
            {
                if (arg == null)
                {
                    return;
                }

                try
                {
                    _connectedDevice = arg;
                    await CrossBluetoothLE.Current.Adapter.ConnectToDeviceAsync(_connectedDevice);

                }
                catch (DeviceConnectionException)
                {

                }
                //var service = await connectedDevice.GetServiceAsync(Guid.Parse(BluetoothUUID.ServiceUUID));
                //characteristic = await service.GetCharacteristicAsync(Guid.Parse(BluetoothUUID.CharacteristicUUID));
                //characteristic.ValueUpdated += (o, args) =>
                //{
                //    var bytes = args.Characteristic.Value;
                //    Device.BeginInvokeOnMainThread(() =>
                //    {
                //        labelInbox.Text = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                //    });

                //};

                //await characteristic.StartUpdatesAsync();
            });
            MessagingCenter.Subscribe<MainParametersPageModel, int>(this, MessengerKeys.CurrentView, SendViewAsync);
            MessagingCenter.Subscribe<MainParametersPageModel, int>(this, MessengerKeys.Speed, SendSpeedAsync);
            MessagingCenter.Subscribe<MainParametersPageModel, DateTime>(this, MessengerKeys.DateTime, SendDateTime);
            MessagingCenter.Subscribe<MainParametersPageModel, string>(this, MessengerKeys.QuickMessage, SendQuickMessage);
            MessagingCenter.Subscribe<MessagePageModel, string>(this, MessengerKeys.Message, SendMessage);
            MessagingCenter.Subscribe<ColorsPageModel, DisplayColors>(this, MessengerKeys.Colours, SendColours);

        }

        private void SendQuickMessage(MainParametersPageModel arg1, string arg2)
        {
            string data = BluetoothHelper.BluetoothContract.Message + "  " + arg2 + '\n';
            WriteData(data);
        }

        private void SendColours(ColorsPageModel arg1, DisplayColors arg2)
        {
            string data = BluetoothHelper.BluetoothContract.Colours + arg2.ColorCode() + '\n';
            WriteData(data);
        }

        private void SendMessage(MessagePageModel arg1, string arg2)
        {
            string data = BluetoothHelper.BluetoothContract.Message + arg2 + '\n';
            //string data = BluetoothHelper.BluetoothContract.Message + arg2 + "  " + '\n';
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
            if (_connectedDevice?.State != DeviceState.Connected) return;
            var service = await _connectedDevice.GetServiceAsync(Guid.Parse(BluetoothHelper.BluetoothUuid.ServiceUuid));
            var characteristic = await service.GetCharacteristicAsync(Guid.Parse(BluetoothHelper.BluetoothUuid.CharacteristicUuid));
            byte[] array = Encoding.UTF8.GetBytes(data);
            await characteristic.WriteAsync(array);
        }

        private async void GoToBluetoothDevicesPageAsync()
        {
            await Navigation.PushAsync(new BluetoothDevicesPage());
        }


    }
}

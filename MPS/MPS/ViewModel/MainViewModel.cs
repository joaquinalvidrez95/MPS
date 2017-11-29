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
    public class MainViewModel : BaseViewModel
    {
        private IDevice _connectedDevice;
        public ICommand BluetoothConnectionCommand { get; }
        private INavigation Navigation { get; }

        public MainViewModel(INavigation navigation)
        {
            BluetoothConnectionCommand = new Command(GoToBluetoothDevicesPageAsync);
            Navigation = navigation;
            CrossBluetoothLE.Current.Adapter.DeviceConnected += OnDeviceStateChanged;
            CrossBluetoothLE.Current.Adapter.DeviceDisconnected += OnDeviceStateChanged;
            MessagingCenter.Subscribe<BluetoothDevicesViewModel, IDevice>(this, MessengerKeys.DeviceSelected, async (sender, arg) =>
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
            MessagingCenter.Subscribe<MainParametersBaseViewModel, int>(this, MessengerKeys.CurrentView, SendViewAsync);
            MessagingCenter.Subscribe<MainParametersBaseViewModel, int>(this, MessengerKeys.Speed, SendSpeedAsync);
            MessagingCenter.Subscribe<MainParametersBaseViewModel, DateTime>(this, MessengerKeys.DateTime, SendDateTime);
            MessagingCenter.Subscribe<MessagePageViewModel, string>(this, MessengerKeys.Message, SendMessage);
            MessagingCenter.Subscribe<ColorsPageViewModel, DisplayColors>(this, MessengerKeys.Colours, SendColours);                   
        }
      
        private void SendColours(ColorsPageViewModel arg1, DisplayColors arg2)
        {
            string data = BluetoothHelper.BluetoothContract.Colours + arg2.ColorCode() + '\n';
            WriteData(data);
        }

        private void SendMessage(MessagePageViewModel arg1, string arg2)
        {
            string data = BluetoothHelper.BluetoothContract.Message + "  " + arg2 + '\n';
            WriteData(data);
        }

        private void SendDateTime(MainParametersBaseViewModel arg1, DateTime arg2)
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
       
        private void SendSpeedAsync(MainParametersBaseViewModel arg1, int arg2)
        {
            string data = BluetoothHelper.BluetoothContract.Speed + arg2 + '\n';
            WriteData(data);
        }

        private void SendViewAsync(MainParametersBaseViewModel arg1, int arg2)
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

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

namespace MPS.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IDevice connectedDevice;
        public ICommand BluetoothConnectionCommand { get; private set; }
        public INavigation Navigation { get; }

        public MainViewModel(INavigation navigation)
        {
            BluetoothConnectionCommand = new Command(goToBluetoothDevicesPageAsync);
            Navigation = navigation;
            CrossBluetoothLE.Current.Adapter.DeviceConnected += onDeviceStateChanged;
            CrossBluetoothLE.Current.Adapter.DeviceDisconnected += onDeviceStateChanged;
            MessagingCenter.Subscribe<BluetoothDevicesViewModel, IDevice>(this, MessengerKeys.DEVICE_SELECTED, async (sender, arg) =>
            {

                if (arg == null)
                {
                    return;
                }

                try
                {
                    connectedDevice = arg;
                    await CrossBluetoothLE.Current.Adapter.ConnectToDeviceAsync(connectedDevice);
                }
                catch (DeviceConnectionException d)
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
            MessagingCenter.Subscribe<MainParametersViewModel, int>(this, MessengerKeys.CURRENT_VIEW, sendViewAsync);
            MessagingCenter.Subscribe<MainParametersViewModel, int>(this, MessengerKeys.SPEED, sendSpeedAsync);
            MessagingCenter.Subscribe<MainParametersViewModel, DateTime>(this, MessengerKeys.DATE_TIME, sendDateTime);

        }

        private void sendDateTime(MainParametersViewModel arg1, DateTime arg2)
        {
            //string data =
            //    BluetoothHelper.BluetoothContract.DATE_TIME
            //    + arg2.Hour.ToString()
            //    + arg2.Minute.ToString()
            //    + arg2.Second.ToString()
            //    + arg2.Day.ToString()
            //    + arg2.Month.ToString()
            //    + arg2.Year.ToString()
            //    + '\n';
            string data =
                BluetoothHelper.BluetoothContract.DATE_TIME + arg2.ToString("HHmmssddMMyy")
                //+ arg2.Hour.ToString()
                //+ arg2.Minute.ToString()
                //+ arg2.Second.ToString()
                //+ arg2.Day.ToString()
                //+ arg2.Month.ToString()
                //+ arg2.Year.ToString()
                + '\n';
            writeData(data);
        }

        private void onDeviceStateChanged(object sender, DeviceEventArgs e)
        {
            MessagingCenter.Send(this, MessengerKeys.DEVICE_STATUS, e.Device);
        }

        private void sendSpeedAsync(MainParametersViewModel arg1, int arg2)
        {
            string data = BluetoothHelper.BluetoothContract.SPEED + arg2.ToString() + '\n';
            writeData(data);
        }

        private void sendViewAsync(MainParametersViewModel arg1, int arg2)
        {
            string data = BluetoothHelper.BluetoothContract.VIEW + arg2.ToString() + '\n';
            writeData(data);
        }

        private async void writeData(string data)
        {
            if (connectedDevice != null)
            {
                if (connectedDevice.State == DeviceState.Connected)
                {
                    var service = await connectedDevice.GetServiceAsync(Guid.Parse(BluetoothHelper.BluetoothUUID.SERVICE_UUID));
                    var characteristic = await service.GetCharacteristicAsync(Guid.Parse(BluetoothHelper.BluetoothUUID.CHARACTERISTIC_UUID));
                    byte[] array = Encoding.UTF8.GetBytes(data);
                    await characteristic.WriteAsync(array);
                }
            }

        }

        private async void goToBluetoothDevicesPageAsync()
        {
            await Navigation.PushAsync(new BluetoothDevicesPage());
        }


    }
}

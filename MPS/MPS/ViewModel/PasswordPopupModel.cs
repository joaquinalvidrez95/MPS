using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using MPS.Bluetooth;
using MPS.Utilities;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Exceptions;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace MPS.ViewModel
{
    public class PasswordPopupModel : BaseViewModel
    {
        private string _password;
        private bool _isErrorMessageVisible;
        private IDevice _connectedDevice;
        private bool _isWaitingForRequest;
        private string _connectionEror;
        private bool _hasFeedbackPin;
        private const int Timeout = 1500;
        private int i;

        public bool IsWaitingForRequest
        {
            get => _isWaitingForRequest;
            set { _isWaitingForRequest = value; OnPropertyChanged(); }
        }

        public string ConnectionEror
        {
            get => _connectionEror;
            set { _connectionEror = value; OnPropertyChanged(); }
        }

        public bool IsErrorMessageVisible
        {
            get => _isErrorMessageVisible;
            set { _isErrorMessageVisible = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set
            {
                IsErrorMessageVisible = false;
                _password = value;
                OnPropertyChanged();
            }
        }

        public ICommand DoneCommand { get; }

        public ICommand CancelCommand { get; }

        public PasswordPopupModel()
        {
            Password = "";
            DoneCommand = new Command(StartConnection);
            CancelCommand = new Command(CancelConnection);
            //CrossBluetoothLE.Current.Adapter.DeviceConnected += OnDeviceStateChanged;
            //CrossBluetoothLE.Current.Adapter.DeviceDisconnected += OnDeviceStateChanged;
            CrossBluetoothLE.Current.Adapter.DeviceConnectionLost += OnDeviceConnectionLost;
        }

        private async void OnDeviceConnectionLost(object sender, DeviceErrorEventArgs e)
        {
            Debug.WriteLine("-------Se ha desconectado--------");

            await PopupNavigation.PopAsync();
        }      

        private async void CancelConnection()
        {
            CrossBluetoothLE.Current.Adapter.DeviceConnectionLost -= OnDeviceConnectionLost;
            await PopupNavigation.PopAsync();
        }

        private void StartConnection()
        {
            i = 0;
            AskForPin(_connectedDevice);
            IsWaitingForRequest = true;
            Device.StartTimer(TimeSpan.FromMilliseconds(Timeout), () =>
            {
                if (!_hasFeedbackPin)
                {
                    i++;
                    if (i > 1)
                    {
                        ConnectionEror = (string)Application.Current.Resources["TextTimeExpired"];
                        IsErrorMessageVisible = true;
                        IsWaitingForRequest = false;
                    }
                    else
                    {
                        AskForPin(_connectedDevice);
                    }

                }
                return !_hasFeedbackPin && i > 1;
            });


        }

        protected override void Subscribe()
        {
            MessagingCenter.Subscribe<BluetoothDevicesPageModel, IDevice>(
                this,
                MessengerKeys.DeviceSelected,
                OnDeviceSelected);

        }

        private async void OnDeviceSelected(BluetoothDevicesPageModel sender, IDevice device)
        {
            if (device == null)
            {
                return;
            }

            _connectedDevice = device;

            var service = await device.GetServiceAsync(Guid.Parse(BluetoothHelper.BluetoothUuid.ServiceUuid));
            var characteristic = await service.GetCharacteristicAsync(Guid.Parse(BluetoothHelper.BluetoothUuid.CharacteristicUuid));
            characteristic.ValueUpdated += OnDataReceived;
            await characteristic.StartUpdatesAsync();

        }

        private void OnDataReceived(object sender, CharacteristicUpdatedEventArgs characteristicUpdatedEventArgs)
        {

            IsWaitingForRequest = false;
            var bytes = characteristicUpdatedEventArgs.Characteristic.Value;

            var x = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            Debug.WriteLine(x);
            switch (x[0].ToString())
            {
                case BluetoothHelper.BluetoothContract.PinOk:
                    _hasFeedbackPin = true;
                    ConnectionEror = (string)Application.Current.Resources["TextPasswordIncorrect"];
                    if (x[1] - 48 != 0)
                    {
                        RequestParameters();

                    }
                    else
                    {
                        IsErrorMessageVisible = true;
                    }

                    break;
                case BluetoothHelper.BluetoothContract.Request:
                    CancelConnection();
                    break;
            }
        }

        private void RequestParameters()
        {
            WriteData(_connectedDevice, BluetoothHelper.BluetoothContract.Request + '\n');
        }

        private void AskForPin(IDevice device)
        {
            WriteData(device, BluetoothHelper.BluetoothContract.Pin + Password + '\n');
        }

        private async void WriteData(IDevice device, string data)
        {
            //if (_isFixingControls) return;
            if (device?.State != DeviceState.Connected)
            {
                return;
            }
            var service = await device.GetServiceAsync(Guid.Parse(BluetoothHelper.BluetoothUuid.ServiceUuid));
            var characteristic = await service.GetCharacteristicAsync(Guid.Parse(BluetoothHelper.BluetoothUuid.CharacteristicUuid));
            var array = Encoding.UTF8.GetBytes(data);
            await characteristic.WriteAsync(array);
            Debug.WriteLine("Written data: " + data);
        }
    }
}

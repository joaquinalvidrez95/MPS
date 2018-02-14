using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MPS.Helper;
using MPS.View;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Exceptions;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace MPS.ViewModel
{
    public class BluetoothDevicesPageModel : BaseViewModel
    {       
        private ObservableCollection<IDevice> _devices;
        private IDevice _selectedDevice;       

        public ICommand ItemTappedCommand { get; }
        public ICommand ScanCommand { get; }

        public ObservableCollection<IDevice> Devices
        {
            get => _devices;
            set
            {
                _devices = value;
                OnPropertyChanged();
            }
        }

        public IDevice SelectedDevice
        {
            get => null;
            set
            {
                _selectedDevice = value;
                OnPropertyChanged();
            }
        }

        public BluetoothDevicesPageModel()
        {           
            _devices = new ObservableCollection<IDevice>();
            ItemTappedCommand = new Command(SelectDeviceAsync);
            ScanCommand = new Command(ScanDevices);
            SetupBluetoothAsync();
        }

        private async void ScanDevices()
        {
            await CrossBluetoothLE.Current.Adapter.StopScanningForDevicesAsync();
            _devices.Clear();
            await CrossBluetoothLE.Current.Adapter.StartScanningForDevicesAsync();
        }

        private async void SetupBluetoothAsync()
        {
            if (CrossBluetoothLE.Current.IsOn)
            {
                _devices.Clear();
                CrossBluetoothLE.Current.Adapter.DeviceDiscovered += OnDeviceDiscovered;
                await CrossBluetoothLE.Current.Adapter.StartScanningForDevicesAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(
                    "",
                    (string)Application.Current.Resources["DisplayAlertMessageBluetoothOff"],
                    (string)Application.Current.Resources["DisplayAlertCancelAccept"]
                );
                MessagingCenter.Send(this, MessengerKeys.OnPopAsync);
            }
        }

        private void OnDeviceDiscovered(object sender, DeviceEventArgs e)
        {
            //Debug.WriteLine("GUID: " + e.Device.Id);
            if (_devices.Contains(e.Device)) return;
            if (e.Device.Name == null) return;
            _devices.Add(e.Device);        
        }

        private async void SelectDeviceAsync()
        {
            if (_selectedDevice == null)
            {
                return;
            }

            await CrossBluetoothLE.Current.Adapter.StopScanningForDevicesAsync();
          //  Debug.WriteLine("Dispositivo seleccionado: " + _selectedDevice.Name + _selectedDevice.State);

            if (CrossBluetoothLE.Current.Adapter.ConnectedDevices != null)
            {
                if (CrossBluetoothLE.Current.Adapter.ConnectedDevices.Count > 0)
                {
                    var deviceToDisconnect = CrossBluetoothLE.Current.Adapter.ConnectedDevices[0];
                    MessagingCenter.Send(this, MessengerKeys.DeviceToDisconnect, deviceToDisconnect);
                }
            }

            MessagingCenter.Send(this, MessengerKeys.DeviceSelected, _selectedDevice);
            MessagingCenter.Send(this, MessengerKeys.OnPopAsync);

        }


    }
}

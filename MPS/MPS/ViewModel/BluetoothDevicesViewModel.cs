using MPS.Bluetooth;
using MPS.Utilities;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MPS.ViewModel
{
    public class BluetoothDevicesViewModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private string _text;
        private ObservableCollection<IDevice> _devices;
        private IDevice _deviceSelected;       
        //private IDevice connectedDevice;
        //private ICharacteristic characteristic;

        public ICommand ItemTappedCommand { get; set; }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<IDevice> Devices
        {
            get => _devices;
            set
            {
                _devices = value;
                OnPropertyChanged();
            }
        }
        public IDevice DeviceSelected
        {
            get => _deviceSelected;
            set
            {
                _deviceSelected = value;
                OnPropertyChanged();
            }
        }

        
        public BluetoothDevicesViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _devices = new ObservableCollection<IDevice>();                              
            ItemTappedCommand = new Command(ChangeLabelAsync);
            
            SetupBluetoothAsync();
            
        }

        private async void SetupBluetoothAsync()
        {
            _devices.Clear();
            CrossBluetoothLE.Current.Adapter.DeviceDiscovered += (s, a) =>
            {
                if (!_devices.Contains(a.Device))
                    _devices.Add(a.Device);
            };
            await CrossBluetoothLE.Current.Adapter.StartScanningForDevicesAsync();
        }

        private async void ChangeLabelAsync()
        {
            Text = DeviceSelected.Name;
            await CrossBluetoothLE.Current.Adapter.StopScanningForDevicesAsync();           
            await _navigation.PopAsync();
            MessagingCenter.Send(this, MessengerKeys.DeviceSelected, DeviceSelected);
        }
    }
}

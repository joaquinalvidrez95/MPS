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
    public class BluetoothDevicesViewModel : ViewModelBase
    {
        private INavigation navigation;
        private string text;
        private ObservableCollection<IDevice> devices;
        private IDevice deviceSelected;       
        //private IDevice connectedDevice;
        //private ICharacteristic characteristic;

        public ICommand ItemTappedCommand { get; set; }

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<IDevice> Devices
        {
            get
            {
                return devices;
            }
            set
            {
                devices = value;
                OnPropertyChanged();
            }
        }
        public IDevice DeviceSelected
        {
            get
            {
                return deviceSelected;
            }
            set
            {
                deviceSelected = value;
                OnPropertyChanged();
            }
        }

        
        public BluetoothDevicesViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            devices = new ObservableCollection<IDevice>();                  
            MessagingCenter.Subscribe<MainViewModel, string>(this, "Hi", (sender, arg) => {
            
                Text = arg;
            });
            ItemTappedCommand = new Command(changeLabelAsync);
            
            setupBluetoothAsync();
            
        }

        private async void setupBluetoothAsync()
        {
            devices.Clear();
            CrossBluetoothLE.Current.Adapter.DeviceDiscovered += (s, a) =>
            {
                if (!devices.Contains(a.Device))
                    devices.Add(a.Device);
            };
            await CrossBluetoothLE.Current.Adapter.StartScanningForDevicesAsync();
        }

        private async void changeLabelAsync()
        {
            Text = DeviceSelected.Name;
            await CrossBluetoothLE.Current.Adapter.StopScanningForDevicesAsync();
            await navigation.PopAsync();
            MessagingCenter.Send(this, MessengerKeys.DEVICE_SELECTED, DeviceSelected);
        }
    }
}

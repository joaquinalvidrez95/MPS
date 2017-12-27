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
using MPS.View;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace MPS.ViewModel
{
    public class BluetoothDevicesPageModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        //private string _text;
        private ObservableCollection<IDevice> _devices;
        private IDevice _deviceSelected;

        public ICommand ItemTappedCommand { get; }

        //public string Text
        //{
        //    get => _text;
        //    set
        //    {
        //        _text = value;
        //        OnPropertyChanged();
        //    }
        //}

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

        public BluetoothDevicesPageModel(INavigation navigation)
        {
            _navigation = navigation;
            _devices = new ObservableCollection<IDevice>();
            ItemTappedCommand = new Command(SelectDeviceAsync);

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

        private async void SelectDeviceAsync()
        {
            //Text = DeviceSelected.Name;
            //await PopupNavigation.PushAsync(new PasswordPopup());
            await CrossBluetoothLE.Current.Adapter.StopScanningForDevicesAsync();
            await _navigation.PopAsync();
            MessagingCenter.Send(this, MessengerKeys.DeviceSelected, DeviceSelected);
        }
    }
}

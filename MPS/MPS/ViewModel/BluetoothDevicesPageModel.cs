using MPS.Bluetooth;
using MPS.Utilities;
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
using MPS.View;
using Plugin.BLE.Abstractions.EventArgs;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace MPS.ViewModel
{
    public class BluetoothDevicesPageModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private ObservableCollection<IDevice> _devices;
        private IDevice _deviceSelected;

        public ICommand ItemTappedCommand { get; }

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
            if (CrossBluetoothLE.Current.IsOn)
            {
                _devices.Clear();
                CrossBluetoothLE.Current.Adapter.DeviceDiscovered += OnDeviceDiscovered;
                await CrossBluetoothLE.Current.Adapter.StartScanningForDevicesAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(
                    (string)Application.Current.Resources["DisplayAlertTitleError"],
                    (string) Application.Current.Resources["DisplayAlertMessageBluetoothOff"],
                    (string)Application.Current.Resources["DisplayAlertCancelAccept"]
                );
                await _navigation.PopAsync();
            }
        }

        private void OnDeviceDiscovered(object sender, DeviceEventArgs e)
        {
            //Debug.WriteLine("Dispositivo encontrado: " + e.Device.Id);

            if (_devices.Contains(e.Device)) return;
            if (e.Device.Name == null) return;
            _devices.Add(e.Device);
        }

        private async void SelectDeviceAsync()
        {
            //await PopupNavigation.PushAsync(new PasswordPopup());          
            await CrossBluetoothLE.Current.Adapter.StopScanningForDevicesAsync();
            await _navigation.PopAsync();
            MessagingCenter.Send(this, MessengerKeys.DeviceSelected, DeviceSelected);
        }

        protected override void Subscribe()
        {

        }
    }
}

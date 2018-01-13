﻿using MPS.Utilities;
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
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Exceptions;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace MPS.ViewModel
{
    public class BluetoothDevicesPageModel : BaseViewModel
    {
        private readonly INavigation _navigation;
        private ObservableCollection<IDevice> _devices;
        private IDevice _selectedDevice;

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
        public IDevice SelectedDevice
        {
            get => null;
            set
            {
                _selectedDevice = value;
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
                CrossBluetoothLE.Current.Adapter.DeviceConnected += OnDeviceStateChanged;

                //CrossBluetoothLE.Current.Adapter.DeviceDisconnected += OnDeviceStateChanged;
                //CrossBluetoothLE.Current.Adapter.DeviceConnectionLost += OnDeviceConnectionLost;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(
                    (string)Application.Current.Resources["DisplayAlertTitleError"],
                    (string)Application.Current.Resources["DisplayAlertMessageBluetoothOff"],
                    (string)Application.Current.Resources["DisplayAlertCancelAccept"]
                );
                await _navigation.PopAsync();
            }
        }

        private void OnDeviceConnectionLost(object sender, DeviceErrorEventArgs e)
        {

        }

        private void OnDeviceStateChanged(object sender, DeviceEventArgs e)
        {
            switch (e.Device.State)
            {
                case DeviceState.Disconnected:
                    break;
                case DeviceState.Connecting:
                    Debug.WriteLine("Conectando man");
                    break;
                case DeviceState.Connected:
                    //RequestParameters(e.Device);                    
                    Debug.WriteLine("--------Conectado-------");
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await PopupNavigation.PushAsync(new PasswordPopup());
                        MessagingCenter.Send(this, MessengerKeys.DeviceSelected, e.Device);
                    });
                    

                    break;
                case DeviceState.Limited:
                    break;
            }
        }

        private void OnDeviceDiscovered(object sender, DeviceEventArgs e)
        {            
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
            try
            {
                Debug.WriteLine("Conectando");
                await CrossBluetoothLE.Current.Adapter.StopScanningForDevicesAsync();
                Debug.WriteLine("Se dejó de escanear");

                await CrossBluetoothLE.Current.Adapter.ConnectToDeviceAsync(_selectedDevice);
                Debug.WriteLine("Conectando2");

            }
            catch (DeviceConnectionException)
            {
                Debug.WriteLine(GetType() + "Error al conectar");
            }
           

        }


    }
}

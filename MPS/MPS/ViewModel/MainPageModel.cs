﻿using MPS.Bluetooth;
using MPS.Utilities;
using MPS.View;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Plugin.BLE.Abstractions.EventArgs;
using MPS.Model;
using Rg.Plugins.Popup.Services;

namespace MPS.ViewModel
{
    public class MainPageModel : BaseViewModel
    {
        private IDevice _connectedDevice;
        private int _numberOfTrials;
        private const int MaxNumberOfTrials = 3;
        private bool _hasFeedbackPin;
        private bool _isFixingControls;
        private const int TimeoutForFixingControls = 20;
        private const int Timeout = 3000;
        private string _password = Settings.Password;
        private bool _isConnectionAutomatic = true;
        public ICommand BluetoothConnectionCommand { get; }
        public ICommand AboutCommand { get; }

        public MainPageModel()
        {
            BluetoothConnectionCommand = new Command(GoToBluetoothDevicesPageAsync);
            AboutCommand = new Command(GoToAboutPage);
            CrossBluetoothLE.Current.Adapter.DeviceConnected += OnDeviceStateChanged;
            CrossBluetoothLE.Current.Adapter.DeviceConnectionLost += OnDeviceConnectionLost;
            CrossBluetoothLE.Current.Adapter.DeviceDisconnected += OnDeviceStateChanged;
            AutoConnect();
        }

        private async void AutoConnect()
        {
            if (!CrossBluetoothLE.Current.IsOn) return;
            try
            {
                await CrossBluetoothLE.Current.Adapter.ConnectToKnownDeviceAsync(Settings.LastDeviceGuid);
                _isConnectionAutomatic = true;
            }
            catch (DeviceConnectionException)
            {
                Debug.WriteLine("No se pudo autoconectar");
            }
        }

        private void OnDeviceConnectionLost(object sender, DeviceErrorEventArgs e)
        {
            MessagingCenter.Send(this, MessengerKeys.DeviceStatus, e.Device);
            if (!e.Device.Equals(_connectedDevice)) return;
            var name = e.Device.Name ?? (string)Application.Current.Resources["TextUnnamedDevice"];
            Device.BeginInvokeOnMainThread(() => Application.Current.MainPage.DisplayAlert(
                "",
                (string)Application.Current.Resources["DisplayAlertMessageConexionLost"] + name,
                (string)Application.Current.Resources["DisplayAlertCancelAccept"]
            ));
            MessagingCenter.Send(this, MessengerKeys.ClosePasswordLogin);
        }

        private void OnPasswordReceived(PasswordPopupModel passwordPopupModel, string password)
        {
            Connect(password);
        }

        private void Connect(string password)
        {
            _hasFeedbackPin = false;
            _numberOfTrials = 0;
            _password = password;
            RequestParameters(password);
            if (!_isConnectionAutomatic)
            {
                MessagingCenter.Send(this, MessengerKeys.LoginState, PasswordLoginState.WaitingForRequest);
            }

            Device.StartTimer(TimeSpan.FromMilliseconds(Timeout), () =>
            {
                if (!_hasFeedbackPin)
                {
                    _numberOfTrials++;
                    if (_numberOfTrials > MaxNumberOfTrials)
                    {
                        if (!_isConnectionAutomatic)
                        {
                            MessagingCenter.Send(this, MessengerKeys.LoginState, PasswordLoginState.TimeoutExpired);
                        }
                        //else
                        //{

                        //}
                    }
                    else
                    {
                        RequestParameters(password);

                    }
                }
                //return !_hasFeedbackPin && _numberOfTrials > MaxNumberOfTrials;
                return _numberOfTrials < MaxNumberOfTrials;
            });
        }

        private void OnDeviceStateChanged(object sender, DeviceEventArgs e)
        {
            switch (e.Device.State)
            {
                case DeviceState.Disconnected:
                    MessagingCenter.Send(this, MessengerKeys.DeviceStatus, e.Device);
                    break;
                case DeviceState.Connecting:
                    break;
                case DeviceState.Connected:
                    SubcribeRead(e.Device);
                    _connectedDevice = e.Device;
                    if (!_isConnectionAutomatic)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await PopupNavigation.PushAsync(new PasswordPopup());
                        });
                    }
                    else
                    {
                        Connect(Settings.Password);
                    }


                    break;

            }
        }

        private async void GoToAboutPage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AboutPage());
        }

        private async void OnDeviceSelected(BluetoothDevicesPageModel arg1, IDevice device)
        {
            try
            {
                _isConnectionAutomatic = false;
                await CrossBluetoothLE.Current.Adapter.ConnectToDeviceAsync(device);
                _connectedDevice = device;

            }
            catch (DeviceConnectionException)
            {
                Debug.WriteLine("Error al conectar");
            }
        }

        private async void OnDeviceToDisconnectReceived(BluetoothDevicesPageModel bluetoothDevicesPageModel, IDevice device)
        {
            await CrossBluetoothLE.Current.Adapter.DisconnectDeviceAsync(device);

        }

        private async void OnDataReceived(object sender, CharacteristicUpdatedEventArgs e)
        {
            if (!e.Characteristic.CanRead) return;
            var bytes = e.Characteristic.Value;

            Debug.WriteLine("Inbox: " + e.Characteristic.StringValue + '\n');
            switch (e.Characteristic.StringValue[0].ToString())
            {
                case BluetoothHelper.BluetoothContract.OnPinStatusReceived:
                    if (e.Characteristic.StringValue[1] - 48 == 0)
                    {
                        _hasFeedbackPin = true;
                        MessagingCenter.Send(this, MessengerKeys.LoginState, PasswordLoginState.PasswordInvalid);
                        if (_isConnectionAutomatic)
                        {
                            await CrossBluetoothLE.Current.Adapter.DisconnectDeviceAsync(_connectedDevice);
                        }
                    }

                    break;

                case BluetoothHelper.BluetoothContract.OnFeedbackReceived:
                    _hasFeedbackPin = true;
                    UnsubcribeRead(_connectedDevice);
                    Settings.LastDeviceGuid = _connectedDevice.Id;
                    Settings.Password = _password;
                    _isFixingControls = true;
                    UnsubscribeConstrols();
                    new Feedbacker().SendUiParameters(bytes);
                    Device.StartTimer(TimeSpan.FromMilliseconds(TimeoutForFixingControls), () =>
                    {
                        Debug.WriteLine("Conexión exitosa");
                        SubscribeConstrols();
                        MessagingCenter.Send(this, MessengerKeys.ClosePasswordLogin);
                        _isFixingControls = false;
                        return false;
                    });
                    MessagingCenter.Send(this, MessengerKeys.DeviceStatus, _connectedDevice);
                    break;
            }
        }
        #region Send Data
        private void OnPowerStatusReceived(MainParametersPageModel arg1, bool arg2)
        {
            string data = MessengerKeys.Power + (arg2 ? 1 : 0) + '\n';
            WriteData(data);
        }

        private void OnQuickMessageReceived(QuickMessagePopupModel quickMessagePopupModel, Message message)
        {
            SendMessage(message.Text);
        }

        private void OnMessageStoredReceived(MessagePageModel arg1, Message message)
        {
            SendMessage(message.Text);
        }

        private void SendMessage(string message)
        {
            var data = MessengerKeys.Message + message + "   " + '\n';
            int i;
            const int max = 15;
            for (i = 0; i < data.Length / max; i++)
            {
                WriteData(data.Substring(i * max, max));
                Task.Delay(1);
            }
            if (data.Length % max != 0)
                WriteData(data.Substring(i * max, data.Length % max));
        }

        private void OnColoursReceived(ColorsPageModel arg1, DisplayColors arg2)
        {
            string data = MessengerKeys.Colours + arg2.GetColorCode() + '\n';
            WriteData(data);
        }

        private void OnDateTimeReceived(MainParametersPageModel arg1, DateTime arg2)
        {
            var data =
                MessengerKeys.DateTime
                + arg2.ToString("HHmmssddMMyy")
                + '\n';
            WriteData(data);
        }



        private void OnSpeedReceived(MessagePageModel messagePageModel, int arg2)
        {
            string data = MessengerKeys.Speed + arg2 + '\n';
            WriteData(data);
        }

        private void OnViewReceived(MainParametersPageModel arg1, int arg2)
        {
            string data = MessengerKeys.CurrentView + arg2 + '\n';
            WriteData(data);
        }

        private void OnDisplayVisibilityReceived(VisibilityPageModel visibilityPageModel, DisplayVisibility visibility)
        {
            var data = MessengerKeys.Visibilities
                + (visibility.IsTimeVisible ? 1 : 0)
                + (visibility.IsTemperatureVisible ? 1 : 0)
                + (visibility.IsDateVisible ? 1 : 0)
                + '\n';
            WriteData(data);
        }

        private void OnViewModeReceived(VisibilityPageModel visibilityPageModel, ViewMode viewMode)
        {
            string data = MessengerKeys.ViewMode + (int)viewMode + '\n';
            WriteData(data);
        }

        private void OnTimeFormatReceived(VisibilityPageModel visibilityPageModel, TimeFormat timeFormat)
        {
            string data = MessengerKeys.TimeFormat + (int)timeFormat + '\n';
            WriteData(data);
        }
        #endregion
        private void WriteData(string data)
        {
            if (_isFixingControls) return;
            if (_connectedDevice == null)
            {
                return;
            }
            if (_connectedDevice?.State != DeviceState.Connected)
            {
                MessagingCenter.Send(this, MessengerKeys.DeviceStatus, _connectedDevice);
                return;
            }

            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var service =
                        await _connectedDevice.GetServiceAsync(Guid.Parse(BluetoothHelper.BluetoothUuid.ServiceUuid));
                    var characteristic =
                        await service.GetCharacteristicAsync(
                            Guid.Parse(BluetoothHelper.BluetoothUuid.CharacteristicUuid));

                }
                catch (Exception)
                {
                    
                }
                var array = Encoding.UTF8.GetBytes(data);
                if (!characteristic.CanWrite) return;
                try
                {
                    await characteristic.WriteAsync(array);
                }
                catch (InvalidOperationException)
                {
                    Debug.WriteLine("No se pudo escribir");
                }
                Debug.WriteLine(GetType() + " Se envío: " + data + '\n');
            });

        }

        private async void GoToBluetoothDevicesPageAsync()
        {
            _isConnectionAutomatic = false;
            await Application.Current.MainPage.Navigation.PushAsync(new BluetoothDevicesPage());
        }

        protected override void Subscribe()
        {
            MessagingCenter.Subscribe<BluetoothDevicesPageModel, IDevice>(this, MessengerKeys.DeviceSelected, OnDeviceSelected);
            MessagingCenter.Subscribe<BluetoothDevicesPageModel, IDevice>(this, MessengerKeys.DeviceToDisconnect, OnDeviceToDisconnectReceived);
            MessagingCenter.Subscribe<PasswordPopupModel, string>(this, MessengerKeys.PasswordLogin, OnPasswordReceived);
            MessagingCenter.Subscribe<PasswordPopupModel>(this, MessengerKeys.OnLoginCancelled, OnLoginCancelledAsync);
        }

        private void SubscribeConstrols()
        {
            MessagingCenter.Subscribe<MainParametersPageModel, bool>(this, MessengerKeys.Power, OnPowerStatusReceived);
            MessagingCenter.Subscribe<MainParametersPageModel, int>(this, MessengerKeys.CurrentView, OnViewReceived);
            MessagingCenter.Subscribe<MessagePageModel, int>(this, MessengerKeys.Speed, OnSpeedReceived);
            MessagingCenter.Subscribe<MainParametersPageModel, DateTime>(this, MessengerKeys.DateTime, OnDateTimeReceived);
            MessagingCenter.Subscribe<QuickMessagePopupModel, Message>(this, MessengerKeys.Message, OnQuickMessageReceived);
            MessagingCenter.Subscribe<MessagePageModel, Message>(this, MessengerKeys.Message, OnMessageStoredReceived);
            MessagingCenter.Subscribe<ColorsPageModel, DisplayColors>(this, MessengerKeys.Colours, OnColoursReceived);
            MessagingCenter.Subscribe<VisibilityPageModel, DisplayVisibility>(this, MessengerKeys.Visibilities, OnDisplayVisibilityReceived);
            MessagingCenter.Subscribe<VisibilityPageModel, TimeFormat>(this, MessengerKeys.TimeFormat, OnTimeFormatReceived);
            MessagingCenter.Subscribe<VisibilityPageModel, ViewMode>(this, MessengerKeys.ViewMode, OnViewModeReceived);
        }

        private void UnsubscribeConstrols()
        {
            MessagingCenter.Unsubscribe<MainParametersPageModel, bool>(this, MessengerKeys.Power);
            MessagingCenter.Unsubscribe<MainParametersPageModel, int>(this, MessengerKeys.CurrentView);
            MessagingCenter.Unsubscribe<MessagePageModel, int>(this, MessengerKeys.Speed);
            MessagingCenter.Unsubscribe<MainParametersPageModel, DateTime>(this, MessengerKeys.DateTime);
            MessagingCenter.Unsubscribe<QuickMessagePopupModel, Message>(this, MessengerKeys.Message);
            MessagingCenter.Unsubscribe<MessagePageModel, Message>(this, MessengerKeys.Message);
            MessagingCenter.Unsubscribe<ColorsPageModel, DisplayColors>(this, MessengerKeys.Colours);
            MessagingCenter.Unsubscribe<VisibilityPageModel, DisplayVisibility>(this, MessengerKeys.Visibilities);
            MessagingCenter.Unsubscribe<VisibilityPageModel, TimeFormat>(this, MessengerKeys.TimeFormat);
            MessagingCenter.Unsubscribe<VisibilityPageModel, ViewMode>(this, MessengerKeys.ViewMode);
        }

        private async void OnLoginCancelledAsync(PasswordPopupModel passwordPopupModel)
        {
            await CrossBluetoothLE.Current.Adapter.DisconnectDeviceAsync(_connectedDevice);
        }

        private void RequestParameters(string password)
        {
            WriteData(BluetoothHelper.BluetoothContract.Request + password + '\n');
        }


        private async void SubcribeRead(IDevice device)
        {
            var service = await device.GetServiceAsync(Guid.Parse(BluetoothHelper.BluetoothUuid.ServiceUuid));
            var characteristic = await service.GetCharacteristicAsync(Guid.Parse(BluetoothHelper.BluetoothUuid.CharacteristicUuid));
            characteristic.ValueUpdated += OnDataReceived;
            Device.BeginInvokeOnMainThread(async () => await characteristic.StartUpdatesAsync());
        }

        private async void UnsubcribeRead(IDevice device)
        {
            var service = await device.GetServiceAsync(Guid.Parse(BluetoothHelper.BluetoothUuid.ServiceUuid));
            var characteristic = await service.GetCharacteristicAsync(Guid.Parse(BluetoothHelper.BluetoothUuid.CharacteristicUuid));
            characteristic.ValueUpdated -= OnDataReceived;

            Device.BeginInvokeOnMainThread(async () => await characteristic.StopUpdatesAsync());
        }
    }
}

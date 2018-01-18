using MPS.Bluetooth;
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
        private bool _hasFeedbackPin;
        private bool _isFixingControls;
        private const int TimeoutForFixingControls = 20;
        private const int Timeout = 2000;
        private string _password;
        private bool _isConnectionAutomatic=true;
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
            Device.BeginInvokeOnMainThread(() => Application.Current.MainPage.DisplayAlert(
                "",
                 (string)Application.Current.Resources["DisplayAlertMessageConexionLost"] + e.Device.Name,
                (string)Application.Current.Resources["DisplayAlertCancelAccept"]
            ));

            MessagingCenter.Send(this, MessengerKeys.DeviceStatus, e.Device);
            MessagingCenter.Send(this, MessengerKeys.ClosePasswordLogin);
        }

        private void OnPasswordReceived(PasswordPopupModel passwordPopupModel, string password)
        {
            Connect(password);
        }

        private void Connect(string password)
        {
            _numberOfTrials = 0;
            _password = password;
            AskForPin(password);
            MessagingCenter.Send(this, MessengerKeys.LoginState, PasswordLoginState.WaitingForRequest);
            Device.StartTimer(TimeSpan.FromMilliseconds(Timeout), () =>
            {
                if (!_hasFeedbackPin)
                {
                    _numberOfTrials++;
                    if (_numberOfTrials > 1)
                    {
                        MessagingCenter.Send(this, MessengerKeys.LoginState, PasswordLoginState.TimeoutExpired);
                    }
                    else
                    {
                        AskForPin(password);
                    }
                }
                return !_hasFeedbackPin && _numberOfTrials > 1;
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
                                        
                    _connectedDevice = e.Device;
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
                await CrossBluetoothLE.Current.Adapter.ConnectToDeviceAsync(device);
                _connectedDevice = device;
                _isConnectionAutomatic = false;              

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

            var bytes = e.Characteristic.Value;

            var x = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            Debug.WriteLine("Inbox: " + x);
            switch (x[0].ToString())
            {
                case BluetoothHelper.BluetoothContract.PinOk:
                    _hasFeedbackPin = true;
                    if (x[1] - 48 != 0)
                    {
                        RequestParameters();
                    }
                    else
                    {                     
                        MessagingCenter.Send(this, MessengerKeys.LoginState, PasswordLoginState.PasswordInvalid);
                        if (_isConnectionAutomatic)
                        {
                            await CrossBluetoothLE.Current.Adapter.DisconnectDeviceAsync(_connectedDevice);
                        }
                    }

                    break;

                case BluetoothHelper.BluetoothContract.Feedback:

                    Settings.LastDeviceGuid = _connectedDevice.Id;
                    Settings.Password = _password;
                    _isFixingControls = true;

                    new Feedbacker().SendUiParameters(bytes);
                    Device.StartTimer(TimeSpan.FromMilliseconds(TimeoutForFixingControls), () =>
                    {
                        Debug.WriteLine("Se acabó este pedo");
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
        private async void WriteData(string data)
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
            var service = await _connectedDevice.GetServiceAsync(Guid.Parse(BluetoothHelper.BluetoothUuid.ServiceUuid));
            var characteristic = await service.GetCharacteristicAsync(Guid.Parse(BluetoothHelper.BluetoothUuid.CharacteristicUuid));
            var array = Encoding.UTF8.GetBytes(data);
            await characteristic.WriteAsync(array);
            Debug.WriteLine(GetType() + "Se envío: " + data);
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
            //MessagingCenter.Subscribe<PasswordPopupModel, IDevice>(this, MessengerKeys.DeviceSelected, OnDeviceSelected);
            MessagingCenter.Subscribe<PasswordPopupModel, string>(this, MessengerKeys.PasswordLogin, OnPasswordReceived);
            MessagingCenter.Subscribe<PasswordPopupModel>(this, MessengerKeys.OnLoginCancelled, OnLoginCancelledAsync);
            //MessagingCenter.Subscribe<PasswordPopupModel>(this, MessengerKeys.FeedbackStarted, OnControlsFixed);
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

        private async void OnLoginCancelledAsync(PasswordPopupModel passwordPopupModel)
        {
            await CrossBluetoothLE.Current.Adapter.DisconnectDeviceAsync(_connectedDevice);
        }

        

        private void AskForPin(string password)
        {
            WriteData(BluetoothHelper.BluetoothContract.Pin + password + '\n');
        }

        private void RequestParameters()
        {
            WriteData(BluetoothHelper.BluetoothContract.Request + '\n');
        }


        private async void SubcribeRead(IDevice device)
        {
            var service = await device.GetServiceAsync(Guid.Parse(BluetoothHelper.BluetoothUuid.ServiceUuid));
            var characteristic = await service.GetCharacteristicAsync(Guid.Parse(BluetoothHelper.BluetoothUuid.CharacteristicUuid));
            characteristic.ValueUpdated += OnDataReceived;
            await characteristic.StartUpdatesAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using MPS.Bluetooth;
using MPS.Model;
using MPS.Utilities;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Exceptions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace MPS.ViewModel
{
    public class PasswordPopupModel : BaseViewModel
    {
        private string _password;
        //private bool _isErrorMessageVisible;
        //private IDevice _connectedDevice;
        //private bool _isWaitingForRequest;
        //private string _connectionEror;
        //private bool _hasFeedbackPin;
        //private const int Timeout = 2000;
        //private int _numberOfTrials;
        //private const int TimeoutForFixingControls = 100;
        private readonly PopupPage page;
        private bool _canConnect;
        private PasswordLoginState _loginState;

        public PasswordLoginState LoginState
        {
            get => _loginState;
            set
            {
                if (_loginState == value) return;
                _loginState = value;
                CanConnect = value != PasswordLoginState.WaitingForRequest;
                OnPropertyChanged();
            }
        }

        //public bool IsWaitingForRequest
        //{
        //    get => _isWaitingForRequest;
        //    set
        //    {
        //        _isWaitingForRequest = value;
        //        CanConnect = !value;
        //        OnPropertyChanged();
        //    }
        //}

        public bool CanConnect
        {
            get => LoginState != PasswordLoginState.WaitingForRequest && Password?.Length == (int)Application.Current.Resources["PasswordLength"];
            set { _canConnect = value; OnPropertyChanged(); }
        }


        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                CanConnect = Password?.Length == (int)Application.Current.Resources["PasswordLength"];
                //IsErrorMessageVisible = false;
                LoginState = PasswordLoginState.Normal;
                OnPropertyChanged();
            }
        }

        public ICommand DoneCommand { get; }

        public ICommand CancelCommand { get; }

        public PasswordPopupModel(PopupPage page)
        {
            Password = Settings.Password;
            DoneCommand = new Command(StartConnection);
            CancelCommand = new Command(CancelConnection);
            this.page = page;
        }

        private void CancelConnection()
        {
            MessagingCenter.Send(this, MessengerKeys.OnLoginCancelled);
            ClosePopup();
        }

        private async void ClosePopup()
        {
            MessagingCenter.Unsubscribe<MainPageModel>(this, MessengerKeys.ClosePasswordLogin);
      
            MessagingCenter.Unsubscribe<MainPageModel>(this, MessengerKeys.LoginState);

            await PopupNavigation.RemovePageAsync(page);
        }

        private void StartConnection()
        {
            //SubcribeRead(_connectedDevice);
            //_numberOfTrials = 0;
            //IsErrorMessageVisible = false;
            //AskForPin(_connectedDevice);
            //IsWaitingForRequest = true;
            //Device.StartTimer(TimeSpan.FromMilliseconds(Timeout), () =>
            //{
            //    if (!_hasFeedbackPin)
            //    {
            //        _numberOfTrials++;
            //        if (_numberOfTrials > 1)
            //        {
            //            ConnectionEror = (string)Application.Current.Resources["TextTimeExpired"];
            //            IsErrorMessageVisible = true;
            //            IsWaitingForRequest = false;
            //        }
            //        else
            //        {
            //            AskForPin(_connectedDevice);
            //        }

            //    }
            //    return !_hasFeedbackPin && _numberOfTrials > 1;
            //});
            MessagingCenter.Send(this, MessengerKeys.PasswordLogin, Password);

        }

        protected override void Subscribe()
        {
            //MessagingCenter.Subscribe<MainPageModel, IDevice>(this, MessengerKeys.DeviceSelected, OnDeviceSelected);
            MessagingCenter.Subscribe<MainPageModel>(this, MessengerKeys.ClosePasswordLogin, CloseLogin);
            //MessagingCenter.Subscribe<MainPageModel>(this, MessengerKeys.PasswordInvalid, OnPasswordInvalid);
            MessagingCenter.Subscribe<MainPageModel, PasswordLoginState>(this, MessengerKeys.LoginState, OnLoginStateChanged);
            //MessagingCenter.Subscribe<MainPageModel>(this, MessengerKeys.LoginTimeoutExpired, OnTimeoutConnectionExpired);
            //MessagingCenter.Subscribe<MainPageModel, bool>(this, MessengerKeys.IsWaitingForRequest, (model, b) =>
            //    {
            //        IsWaitingForRequest = b;
            //    });

        }

        private void OnLoginStateChanged(MainPageModel mainPageModel, PasswordLoginState passwordLoginState)
        {
            LoginState = passwordLoginState;
            Debug.WriteLine(LoginState);
        }



        private void CloseLogin(MainPageModel mainPageModel)
        {      
            ClosePopup();
        }

        //private void OnDeviceSelected(MainPageModel sender, IDevice device)
        //{
        //    if (device == null)
        //    {
        //        return;
        //    }

        //    _connectedDevice = device;


        //}

        //private void OnDataReceived(object sender, CharacteristicUpdatedEventArgs characteristicUpdatedEventArgs)
        //{

        //    var bytes = characteristicUpdatedEventArgs.Characteristic.Value;

        //    var x = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        //    Debug.WriteLine("Inbox: " + x);
        //    switch (x[0].ToString())
        //    {
        //        case BluetoothHelper.BluetoothContract.PinOk:
        //            _hasFeedbackPin = true;
        //            ConnectionEror = (string)Application.Current.Resources["TextPasswordIncorrect"];
        //            if (x[1] - 48 != 0)
        //            {
        //                RequestParameters();
        //            }
        //            else
        //            {
        //                IsErrorMessageVisible = true;
        //                IsWaitingForRequest = false;
        //                UnsubcribeRead();
        //            }

        //            break;

        //        case BluetoothHelper.BluetoothContract.Feedback:
        //            Settings.LastDeviceGuid = _connectedDevice.Id;
        //            Settings.Password = Password;
        //            IsWaitingForRequest = false;
        //            UnsubcribeRead();
        //            MessagingCenter.Send(this, MessengerKeys.FeedbackStarted);
        //            SendUiParameters(bytes);
        //            Device.StartTimer(TimeSpan.FromMilliseconds(TimeoutForFixingControls), () =>
        //            {
        //                Debug.WriteLine("Se acabó este pedo");
        //                MessagingCenter.Send(this, MessengerKeys.DeviceSelected, _connectedDevice);
        //                ClosePopup();
        //                return false;
        //            });

        //            break;
        //    }
        //}

        //private async void UnsubcribeRead()
        //{
        //    var service = await _connectedDevice.GetServiceAsync(Guid.Parse(BluetoothHelper.BluetoothUuid.ServiceUuid));
        //    var characteristic = await service.GetCharacteristicAsync(Guid.Parse(BluetoothHelper.BluetoothUuid.CharacteristicUuid));
        //    characteristic.ValueUpdated -= OnDataReceived;
        //    await characteristic.StopUpdatesAsync();
        //}

        //private async void SubcribeRead(IDevice device)
        //{
        //    var service = await device.GetServiceAsync(Guid.Parse(BluetoothHelper.BluetoothUuid.ServiceUuid));
        //    var characteristic = await service.GetCharacteristicAsync(Guid.Parse(BluetoothHelper.BluetoothUuid.CharacteristicUuid));
        //    characteristic.ValueUpdated += OnDataReceived;
        //    await characteristic.StartUpdatesAsync();
        //}



        //private async void WriteData(IDevice device, string data)
        //{
        //    if (device?.State != DeviceState.Connected)
        //    {
        //        return;
        //    }
        //    var service = await device.GetServiceAsync(Guid.Parse(BluetoothHelper.BluetoothUuid.ServiceUuid));
        //    var characteristic = await service.GetCharacteristicAsync(Guid.Parse(BluetoothHelper.BluetoothUuid.CharacteristicUuid));
        //    var array = Encoding.UTF8.GetBytes(data);
        //    try
        //    {
        //        await characteristic.WriteAsync(array);
        //    }
        //    catch (InvalidOperationException)
        //    {
        //        Debug.WriteLine("No se pudo escribir");
        //    }
        //    Debug.WriteLine("Written data: " + data);
        //}


    }
}

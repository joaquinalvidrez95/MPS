using MPS.Utilities;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using MPS.Model;
using MPS.View;
using Rg.Plugins.Popup.IOS;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace MPS.ViewModel
{
    public class MainParametersPageModel : BaseViewModel
    {
        //private const double STEP_VALUE = 1.0;
        private string _currentDateTime;
        private double _speed;
        int _currentView;
        private string _text;
        private bool _isBluetoothConnected;
        private string _message;
        private bool _isDisplayEnabled;

        public ICommand DateTimeCommand { get; }
        public ICommand ToggleViewCommand { get; }

        public ICommand QuickMessageCommand { get; }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public string CurrentDateTime
        {
            get => _currentDateTime;
            set
            {
                _currentDateTime = value;
                OnPropertyChanged();
            }
        }

        public double Speed
        {
            get => _speed;
            set
            {
                //value = Math.Round(value / STEP_VALUE);
                //value = value * STEP_VALUE;
                value = Math.Round(value);
                if ((int)value != (int)_speed)
                {
                    MessagingCenter.Send(this, MessengerKeys.Speed, (int)value);
                }
                _speed = value;
                OnPropertyChanged();
            }
        }

        private int CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public bool IsDisplayEnabled
        {
            get => _isDisplayEnabled;
            set { _isDisplayEnabled = value; OnPropertyChanged(); }
        }

        public string Text
        {
            get => _text;
            private set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        public bool IsBluetoothConnected
        {
            get => _isBluetoothConnected;
            set
            {
                _isBluetoothConnected = value;
                OnPropertyChanged();
            }
        }

        public ICommand PowerCommand { get; }

        public MainParametersPageModel()
        {
            DateTimeCommand = new Command(UpdateDateTime);
            ToggleViewCommand = new Command(ToggleView);
            PowerCommand = new Command(TogglePower);
            CurrentView = 0;
            Speed = 0;
            MessagingCenter.Subscribe<MainPageModel, IDevice>(this, MessengerKeys.DeviceStatus, OnDeviceStatusChanged);
            QuickMessageCommand = new Command(SendQuickMessage);
            MessagingCenter.Subscribe<QuickMessagePopupModel, string>(this, MessengerKeys.QuickMessage, OnQuickMessageAdded);
            MessagingCenter.Subscribe<MainPageModel, int>(this, MessengerKeys.Speed, UpdateSpeed);
            MessagingCenter.Subscribe<MainPageModel, int>(this, MessengerKeys.CurrentView, UpdateView);
            MessagingCenter.Subscribe<MainPageModel, bool>(this, MessengerKeys.Power, UpdatePowerFromFeedback);         
        }

        private void UpdatePowerFromFeedback(MainPageModel mainPageModel, bool b)
        {
            IsDisplayEnabled = b;
        }

        private void TogglePower()
        {
            IsDisplayEnabled = !IsDisplayEnabled;
            MessagingCenter.Send(this, MessengerKeys.Power, IsDisplayEnabled);
        }

        private void UpdateView(MainPageModel arg1, int arg2)
        {
            CurrentView = arg2;
        }

        private void UpdateSpeed(MainPageModel arg1, int arg2)
        {
            Speed = arg2;
        }

        private void OnQuickMessageAdded(QuickMessagePopupModel arg1, string text)
        {
            MessagingCenter.Send(this, MessengerKeys.QuickMessage, text);
        }

        private void OnDeviceStatusChanged(MainPageModel arg1, IDevice arg2)
        {
            switch (arg2.State)
            {
                case DeviceState.Connected:
                    IsBluetoothConnected = true;
                    break;
                case DeviceState.Disconnected:
                    IsBluetoothConnected = false;
                    break;
            }
        }

        private void ToggleView()
        {
            CurrentView++;
            CurrentView = CurrentView % 3;
            MessagingCenter.Send(this, MessengerKeys.CurrentView, CurrentView);
             //PopupNavigation.PushAsync(new PasswordPopup());
        }

        private void UpdateDateTime()
        {
            DateTime now = DateTime.Now.ToLocalTime();
            if (DateTime.Now.IsDaylightSavingTime())
            {
                now = now.AddHours(1);
            }
            string currentTime = $"Current Time: {now}";
            CurrentDateTime = currentTime;
            Text = CurrentDateTime;
            MessagingCenter.Send(this, MessengerKeys.DateTime, now);
        }

        private async void SendQuickMessage()
        {
            MessagingCenter.Send(this, MessengerKeys.Message, Message);
            await PopupNavigation.PushAsync(new QuickMessagePopup());
        }

    }
}

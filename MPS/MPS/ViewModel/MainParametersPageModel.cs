﻿using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using MPS.Helper;
using MPS.View;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace MPS.ViewModel
{
    public class MainParametersPageModel : BaseViewModel
    {

        private string _currentDateTime;

        //private bool _isBluetoothConnected;
        private string _message;
        private bool _isDisplayEnabled;

        public DeviceState DeviceState
        {
            get => _deviceState;
            set { _deviceState = value; OnPropertyChanged(); }
        }

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

        private int _currentView;
        private DeviceState _deviceState;

        public bool IsDisplayEnabled
        {
            get => _isDisplayEnabled;
            set { _isDisplayEnabled = value; OnPropertyChanged(); }
        }

        //public bool IsBluetoothConnected
        //{
        //    get => _isBluetoothConnected;
        //    set
        //    {
        //        _isBluetoothConnected = value;
        //        OnPropertyChanged();
        //    }
        //}

        public ICommand PowerCommand { get; }

        public MainParametersPageModel()
        {
            DateTimeCommand = new Command(UpdateDateTime);
            ToggleViewCommand = new Command(ToggleView);
            PowerCommand = new Command(TogglePower);
            _currentView = 0;
            QuickMessageCommand = new Command(SendQuickMessage);

        }

        private void UpdatePowerFromFeedback(Feedbacker feedbacker, bool b)
        {
            IsDisplayEnabled = b;
        }

        private void TogglePower()
        {
            IsDisplayEnabled = !IsDisplayEnabled;
            MessagingCenter.Send(this, MessengerKeys.Power, IsDisplayEnabled);
        }

        private void UpdateView(Feedbacker feedbacker, int arg2)
        {
            _currentView = arg2;
        }


        private void OnDeviceStatusChanged(MainPageModel arg1, IDevice arg2)
        {
            //switch (arg2.State)
            //{
            //    case DeviceState.Connected:
            //        IsBluetoothConnected = true;
            //        break;
            //    case DeviceState.Disconnected:
            //        IsBluetoothConnected = false;
            //        break;
            //    case DeviceState.Limited:
            //        IsBluetoothConnected = false;
            //        break;
            //}
            DeviceState = arg2.State;
        }

        private void ToggleView()
        {
            _currentView++;
            _currentView = _currentView % 3;
            MessagingCenter.Send(this, MessengerKeys.CurrentView, _currentView);
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
            MessagingCenter.Send(this, MessengerKeys.DateTime, now);
        }

        private async void SendQuickMessage()
        {
            await PopupNavigation.PushAsync(new QuickMessagePopup());
        }

        protected override void SubscribeMessagingCenter()
        {
            MessagingCenter.Subscribe<MainPageModel, IDevice>(this, MessengerKeys.DeviceStatus, OnDeviceStatusChanged);           
            MessagingCenter.Subscribe<Feedbacker, int>(this, MessengerKeys.CurrentView, UpdateView);            
            MessagingCenter.Subscribe<Feedbacker, bool>(this, MessengerKeys.Power, UpdatePowerFromFeedback);
        }
    }
}

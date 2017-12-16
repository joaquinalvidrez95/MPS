﻿using MPS.Utilities;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        //enum TypeOfView { FirstView = 0, SecondView, ThirdView };
        int _currentView;
        private string _text;
        private Color _statusColor;
        private string _message;

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

        public string Text
        {
            get => _text;
            private set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        public Color StatusColor
        {
            get => _statusColor;
            set
            {
                _statusColor = value;
                OnPropertyChanged();
            }
        }



        public MainParametersPageModel()
        {
            DateTimeCommand = new Command(UpdateDateTime);
            ToggleViewCommand = new Command(ToggleView);
            CurrentView = 0;
            Speed = 0;
            MessagingCenter.Subscribe<MainPageModel, IDevice>(this, MessengerKeys.DeviceStatus, OnDeviceStatusChanged);
            StatusColor = Color.Red;
            QuickMessageCommand = new Command(SendQuickMessage);
            MessagingCenter.Subscribe<QuickMessagePopupModel, string>(this, MessengerKeys.QuickMessage, OnQuickMessageAdded);
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
                    StatusColor = Color.LawnGreen;
                    break;
                case DeviceState.Disconnected:
                    StatusColor = Color.Red;
                    break;
            }
        }

        private void ToggleView()
        {
            CurrentView++;
            CurrentView = CurrentView % 3;
            MessagingCenter.Send(this, MessengerKeys.CurrentView, CurrentView);


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

using MPS.Utilities;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MPS.ViewModel
{
    public class MainParametersViewModel : ViewModelBase
    {
        private const double STEP_VALUE = 1.0;
        private string currentDateTime;
        private double speed;
        enum TypeOfView { FIRST_VIEW = 0, SECOND_VIEW, THIRD_VIEW };
        int currentView;
        private string text;
        private Color statusColor;

        public ICommand DateTimeCommand { get; private set; }
        public ICommand ToggleViewCommand { get; private set; }
        public string CurrentDateTime
        {
            get
            {
                return currentDateTime;
            }
            set
            {
                currentDateTime = value;
                OnPropertyChanged();
            }
        }

        public double Speed
        {
            get
            {
                return speed;
            }
            set
            {
                //value = Math.Round(value / STEP_VALUE);
                //value = value * STEP_VALUE;
                value = Math.Round(value);
                if (((int)value) != ((int)Speed))
                {
                    speed = value;
                    OnPropertyChanged();
                    MessagingCenter.Send(this, MessengerKeys.SPEED, (int)Speed);
                }
            }
        }

        private int CurrentView
        {
            get => currentView;
            set
            {
                currentView = value;
                OnPropertyChanged();
            }
        }

        public string Text
        {
            get
            {
                return text;
            }
            private set
            {
                text = value;
                OnPropertyChanged();
            }
        }

        public Color StatusColor { get => statusColor;
            set
            {
                statusColor = value;
                OnPropertyChanged();
            }
        }

        public MainParametersViewModel(INavigation navigation)
        {
            DateTimeCommand = new Command(updateDateTime);
            ToggleViewCommand = new Command(toggleView);
            CurrentView = 0;
            Speed = 0;
            MessagingCenter.Subscribe<MainViewModel, IDevice>(this, MessengerKeys.DEVICE_STATUS, onDeviceStatusChanged);
            StatusColor = Color.Red;          
        }

        private void onDeviceStatusChanged(MainViewModel arg1, IDevice arg2)
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
               
        private void toggleView()
        {
            CurrentView++;
            CurrentView = CurrentView % 4;
            MessagingCenter.Send(this, MessengerKeys.CURRENT_VIEW, CurrentView);            

        }

        private void updateDateTime()
        {
            DateTime now = DateTime.Now.ToLocalTime();
            if (DateTime.Now.IsDaylightSavingTime() == true)
            {
                now = now.AddHours(1);
            }
            string currentTime = (string.Format("Current Time: {0}", now));
            CurrentDateTime = currentTime;
            Text = CurrentDateTime;
            MessagingCenter.Send(this, MessengerKeys.DATE_TIME, now);
        }



    }
}

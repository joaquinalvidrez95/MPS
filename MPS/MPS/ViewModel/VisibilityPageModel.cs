using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPS.Model;
using MPS.Utilities;
using Xamarin.Forms;

namespace MPS.ViewModel
{
    public class VisibilityPageModel : BaseViewModel
    {
        private bool _isTimeVisible;
        private bool _isDateVisible;
        private bool _isTemperatureVisible;
        private int _timeFormatSelected;
        private int _viewModeSelected;

        public bool IsTimeVisible
        {
            get => _isTimeVisible;
            set
            {
                _isTimeVisible = value;                
                OnPropertyChanged();
                MessagingCenter.Send(this, MessengerKeys.Visibilities, GetDisplayVisibility());
            }
        }

        private DisplayVisibility GetDisplayVisibility()
        {
            return new DisplayVisibility { IsDateVisible = IsDateVisible, IsTemperatureVisible = IsTemperatureVisible, IsTimeVisible = IsTimeVisible };
        }

        public bool IsDateVisible
        {
            get => _isDateVisible;
            set
            {
                _isDateVisible = value;                
                OnPropertyChanged();
                MessagingCenter.Send(this, MessengerKeys.Visibilities, GetDisplayVisibility());
            }
        }

        public bool IsTemperatureVisible
        {
            get => _isTemperatureVisible;
            set
            {
                _isTemperatureVisible = value;                
                OnPropertyChanged();
                MessagingCenter.Send(this, MessengerKeys.Visibilities, GetDisplayVisibility());
            }
        }

        public int TimeFormatSelected
        {
            get => _timeFormatSelected;
            set
            {
                _timeFormatSelected = value;
                MessagingCenter.Send(this, MessengerKeys.TimeFormat, (TimeFormat)value);
                OnPropertyChanged();
            }
        }

        public int ViewModeSelected
        {
            get => _viewModeSelected;
            set
            {
                _viewModeSelected = value;
                MessagingCenter.Send(this, MessengerKeys.ViewMode, (ViewMode)value);
                OnPropertyChanged();
            }
        }

        public VisibilityPageModel()
        {
        }

        protected override void Subscribe()
        {
            //MessagingCenter.Subscribe<MainPageModel, DisplayVisibility>(this, MessengerKeys.Visibilities, OnVisibilityReceived);
            MessagingCenter.Subscribe<PasswordPopupModel, DisplayVisibility>(this, MessengerKeys.Visibilities, OnVisibilityReceived);
            //MessagingCenter.Subscribe<MainPageModel, TimeFormat>(this, MessengerKeys.TimeFormat, OnTimeFormatReceived);
            MessagingCenter.Subscribe<PasswordPopupModel, TimeFormat>(this, MessengerKeys.TimeFormat, OnTimeFormatReceived);
            //MessagingCenter.Subscribe<MainPageModel, ViewMode>(this, MessengerKeys.ViewMode, OnViewModeReceived);
            MessagingCenter.Subscribe<PasswordPopupModel, ViewMode>(this, MessengerKeys.ViewMode, OnViewModeReceived);
        }

        private void OnViewModeReceived(PasswordPopupModel passwordPopupModel, ViewMode viewMode)
        {
            ViewModeSelected = (int)viewMode;
        }

        private void OnTimeFormatReceived(PasswordPopupModel passwordPopupModel, TimeFormat timeFormat)
        {
            TimeFormatSelected = (int)timeFormat;

        }

        private void OnVisibilityReceived(PasswordPopupModel passwordPopupModel, DisplayVisibility displayVisibility)
        {
            IsDateVisible = displayVisibility.IsDateVisible;
            IsTemperatureVisible = displayVisibility.IsTemperatureVisible;
            IsTimeVisible = displayVisibility.IsTimeVisible;
        }
    }
}

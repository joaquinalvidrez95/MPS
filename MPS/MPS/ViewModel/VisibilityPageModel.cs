using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPS.Helper;
using MPS.Model;
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
        private bool _isFixingControls;

        public bool IsTimeVisible
        {
            get => _isTimeVisible;
            set
            {
                _isTimeVisible = value;
                OnPropertyChanged();
                if (_isFixingControls)
                {
                    _isFixingControls = false;
                }
                else
                {
                    MessagingCenter.Send(this, MessengerKeys.Visibilities, GetDisplayVisibility());
                }

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
                if (_isFixingControls)
                {
                    _isFixingControls = false;
                }
                else
                {
                    MessagingCenter.Send(this, MessengerKeys.Visibilities, GetDisplayVisibility());
                }

            }
        }

        public bool IsTemperatureVisible
        {
            get => _isTemperatureVisible;
            set
            {
                _isTemperatureVisible = value;
                OnPropertyChanged();
                if (_isFixingControls)
                {
                    _isFixingControls = false;
                }
                else
                {
                    MessagingCenter.Send(this, MessengerKeys.Visibilities, GetDisplayVisibility());
                }
                
            }
        }

        public int TimeFormatSelected
        {
            get => _timeFormatSelected;
            set
            {
                _timeFormatSelected = value;
                
                OnPropertyChanged();
                if (_isFixingControls)
                {
                    _isFixingControls = false;
                }
                else
                {
                    MessagingCenter.Send(this, MessengerKeys.TimeFormat, (TimeFormat)value);
                }
                
            }
        }

        public int ViewModeSelected
        {
            get => _viewModeSelected;
            set
            {
                _viewModeSelected = value;
                OnPropertyChanged();
                if (_isFixingControls)
                {
                    _isFixingControls = false;
                }
                else
                {
                    MessagingCenter.Send(this, MessengerKeys.ViewMode, (ViewMode)value);
                }                               
            }
        }

        public VisibilityPageModel()
        {
        }

        protected override void Subscribe()
        {
            MessagingCenter.Subscribe<Feedbacker, DisplayVisibility>(this, MessengerKeys.Visibilities, OnVisibilityReceived);
            MessagingCenter.Subscribe<Feedbacker, TimeFormat>(this, MessengerKeys.TimeFormat, OnTimeFormatReceived);
            MessagingCenter.Subscribe<Feedbacker, ViewMode>(this, MessengerKeys.ViewMode, OnViewModeReceived);
        }

        private void OnViewModeReceived(Feedbacker feedbacker, ViewMode viewMode)
        {
            _isFixingControls = true;
            ViewModeSelected = (int)viewMode;
        }

        private void OnTimeFormatReceived(Feedbacker feedbacker, TimeFormat timeFormat)
        {
            _isFixingControls = true;
            TimeFormatSelected = (int)timeFormat;
        }

        private void OnVisibilityReceived(Feedbacker feedbacker, DisplayVisibility displayVisibility)
        {
            _isFixingControls = true;
            IsDateVisible = displayVisibility.IsDateVisible;
            _isFixingControls = true;
            IsTemperatureVisible = displayVisibility.IsTemperatureVisible;
            _isFixingControls = true;
            IsTimeVisible = displayVisibility.IsTimeVisible;
        }
    }
}

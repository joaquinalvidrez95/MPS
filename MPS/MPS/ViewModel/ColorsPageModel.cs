using MPS.Model;
using MPS.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MPS.ViewModel
{
    public class ColorsPageModel : BaseViewModel
    {

        private int _selectedIndex;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                UpdateRgbColors();
                OnPropertyChanged();
            }
        }

        public ICommand ColorsCommand { get; }

        private DisplayColors _displayColors;

        public double RedValue
        {
            get => _displayColors.GetRedByIndex(SelectedIndex);
            set
            {
                value = Math.Round(value);
                var colorValue = _displayColors.GetColorCodeByIndex(SelectedIndex)[DisplayColorRgb.IndexRed] - 48;

                try
                {
                    _displayColors.SetColorCodeByIndex(SelectedIndex, (int)value + GreenValue.ToString() + BlueValue);
                    if ((int)value != colorValue)
                    {
                        MessagingCenter.Send(this, MessengerKeys.Colours, _displayColors);
                    }
                }
                catch (ColorException)
                {

                }
                OnPropertyChanged();
            }
        }

        public double GreenValue
        {
            get => _displayColors.GetGreenByIndex(SelectedIndex);
            set
            {
                value = Math.Round(value);
                var colorValue = _displayColors.GetGreenByIndex(SelectedIndex);

                try
                {
                    _displayColors.SetColorCodeByIndex(SelectedIndex, RedValue.ToString() + (int) value + BlueValue);
                    if ((int)value != colorValue)
                    {
                        MessagingCenter.Send(this, MessengerKeys.Colours, _displayColors);
                    }
                }
                catch (ColorException)
                {
                    
                }
                
                OnPropertyChanged();
            }
        }

        public double BlueValue
        {
            get => _displayColors.GetBlueByIndex(SelectedIndex);
            set
            {
                value = Math.Round(value);
                var colorValue = _displayColors.GetBlueByIndex(SelectedIndex);
                try
                {
                    _displayColors.SetColorCodeByIndex(SelectedIndex, RedValue.ToString() + GreenValue + (int) value);
                    if ((int)value != colorValue)
                    {
                        MessagingCenter.Send(this, MessengerKeys.Colours, _displayColors);
                    }
                }
                catch (ColorException)
                {
                    
                }
                
                OnPropertyChanged();
            }
        }


        public ColorsPageModel()
        {
            ColorsCommand = new Command<Color>(ChangeColorAsync);
            _displayColors = new DisplayColors();

        }

        private void OnColoursReceived(MainPageModel arg1, DisplayColors arg2)
        {
            _displayColors = arg2;
            UpdateRgbColors();
        }

        private void ChangeColorAsync(Color newColor)
        {
            if (_displayColors.SetColorByIndex(SelectedIndex, newColor))
            {
                MessagingCenter.Send(this, MessengerKeys.Colours, _displayColors);
                UpdateRgbColors();
            }
            else
            {
                DisplayColorError();
            }
        }

        private async void DisplayColorError()
        {
            await Application.Current.MainPage.DisplayAlert(
                (string)Application.Current.Resources["DisplayAlertTitleError"],
                (string) Application.Current.Resources["DisplayAlertMessageColorException"],
                (string)Application.Current.Resources["DisplayAlertCancelAccept"]
            );
        }

        private void UpdateRgbColors()
        {
            RedValue = _displayColors.GetRedByIndex(SelectedIndex);
            GreenValue = _displayColors.GetGreenByIndex(SelectedIndex);
            BlueValue = _displayColors.GetBlueByIndex(SelectedIndex);
        }

        protected override void Subscribe()
        {
            MessagingCenter.Subscribe<MainPageModel, DisplayColors>(
                this,
                MessengerKeys.Colours,
                OnColoursReceived);
        }
    }
}

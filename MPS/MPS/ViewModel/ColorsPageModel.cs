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
                var colorValue = _displayColors.GetRedByIndex(SelectedIndex);
                _displayColors.SetRedByIndex(SelectedIndex, (int)value);

                if ((int)value != colorValue)
                {
                    MessagingCenter.Send(this, MessengerKeys.Colours, _displayColors);
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
                _displayColors.SetGreenByIndex(SelectedIndex, (int)value);

                if ((int)value != colorValue)
                {
                    MessagingCenter.Send(this, MessengerKeys.Colours, _displayColors);
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
                _displayColors.SetBlueByIndex(SelectedIndex, (int)value);

                if ((int)value != colorValue)
                {
                    MessagingCenter.Send(this, MessengerKeys.Colours, _displayColors);
                }
                OnPropertyChanged();
            }
        }

   
        public ColorsPageModel()
        {
            ColorsCommand = new Command<Color>(ChangeColor);
            _displayColors = new DisplayColors();          
           
        }

        private void OnColoursReceived(MainPageModel arg1, DisplayColors arg2)
        {
            _displayColors = arg2;
            UpdateRgbColors();
        }
      
        private void ChangeColor(Color newColor)
        {
     
            _displayColors.SetColorByIndex(SelectedIndex, newColor);
            MessagingCenter.Send(this, MessengerKeys.Colours, _displayColors);
            UpdateRgbColors();
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

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
    public class ColorsPageViewModel : ViewModelBase
    {
        private readonly Color _borderColorDefault = Color.Black;
        private readonly Color _borderColorHighlight = Color.Gray;
        private const int BorderWithDefault = 1;
        private const int BorderWithHighlight = 3;

        private readonly ColorTypeConverter _colorTypeConv = new ColorTypeConverter();
        private Color _borderColorLowerLine;
        private int _borderWidthLowerLineButton;
        private Color _borderColorUpperLineButton;
        private int _borderWidthUpperLineButton;
        enum ButtonSelected { UpperLine = 0, LowerLine, Background, None }

        ButtonSelected _state = ButtonSelected.None;
        private Color _borderColorBackgroundButton;
        private int _borderWidthBackgroundButton;
        private int _selectedIndex;

        public Color BorderColorUpperLineButton
        {
            get => _borderColorUpperLineButton;
            set
            {
                _borderColorUpperLineButton = value;
                OnPropertyChanged();
            }
        }
        public int BorderWidthUpperLineButton
        {
            get => _borderWidthUpperLineButton;
            set
            {
                _borderWidthUpperLineButton = value;
                OnPropertyChanged();
            }
        }

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

        private void UpdateRgbColors()
        {
            RedValue = _displayColors.GetRedByIndex(SelectedIndex);
            GreenValue = _displayColors.GetGreenByIndex(SelectedIndex);
            BlueValue = _displayColors.GetBlueByIndex(SelectedIndex);
        }

        public ICommand ColorsCommand { get; }
        public ICommand SetUpperLineColorCommand { get; }
        public ICommand SetLowerLineColorCommand { get; }
        public ICommand SetBackgroundColorCommand { get; }
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

        public Color ColorUpperLine
        {
            get => _displayColors.ColorUpperLine;
            set
            {
                _displayColors.ColorUpperLine = value;
                OnPropertyChanged();
            }
        }

        public Color ColorLowerLine
        {
            get => _displayColors.ColorLowerLine;
            set
            {
                _displayColors.ColorLowerLine = value;
                OnPropertyChanged();
            }
        }

        public Color ColorBackground
        {
            get => _displayColors.ColorBackground;
            set
            {
                _displayColors.ColorBackground = value;
                OnPropertyChanged();
            }
        }

        public Color BorderColorLowerLineButton
        {
            get => _borderColorLowerLine;
            set
            {
                _borderColorLowerLine = value;
                OnPropertyChanged();
            }
        }

        public int BorderWidthLowerLineButton
        {
            get => _borderWidthLowerLineButton;
            set
            {
                _borderWidthLowerLineButton = value;
                OnPropertyChanged();
            }
        }

        public Color BorderColorBackgroundButton
        {
            get => _borderColorBackgroundButton;
            set
            {
                _borderColorBackgroundButton = value;
                OnPropertyChanged();
            }
        }
        public int BorderWidthBackgroundButton
        {
            get => _borderWidthBackgroundButton;
            set
            {
                _borderWidthBackgroundButton = value;
                OnPropertyChanged();
            }
        }

        public ColorsPageViewModel()
        {
            ColorsCommand = new Command<string>(ChangeColor);
            _displayColors = new DisplayColors();
            SetUpperLineColorCommand = new Command(HighlightUpperLineButton);
            SetLowerLineColorCommand = new Command(HighlightLowerLineButton);
            SetBackgroundColorCommand = new Command(HighlightBackgroundButton);

            //ColorBackground = Color.Aqua;
            //    ColorLowerLine = Color.BlueViolet,
            //    ColorUpperLine = Color.Chartreuse,
            BorderColorLowerLineButton = _borderColorDefault;
            BorderColorUpperLineButton = _borderColorDefault;
            BorderColorBackgroundButton = _borderColorDefault;
            BorderWidthLowerLineButton = BorderWithDefault;
            BorderWidthBackgroundButton = BorderWithDefault;
            BorderWidthUpperLineButton = BorderWithDefault;
        }

        private void HighlightBackgroundButton()
        {
            ClearButtons();
            _state = ButtonSelected.Background;
            SelectedIndex = (int)_state;
            BorderWidthBackgroundButton = BorderWithHighlight;
            BorderColorBackgroundButton = _borderColorHighlight;
        }

        private void HighlightLowerLineButton()
        {
            ClearButtons();
            _state = ButtonSelected.LowerLine;
            SelectedIndex = (int)_state;
            BorderWidthLowerLineButton = BorderWithHighlight;
            BorderColorLowerLineButton = _borderColorHighlight;

        }

        private void ClearButtons()
        {
            BorderColorUpperLineButton = _borderColorDefault;
            BorderColorLowerLineButton = _borderColorDefault;
            BorderColorBackgroundButton = _borderColorDefault;
            BorderWidthUpperLineButton = BorderWithDefault;
            BorderWidthLowerLineButton = BorderWithDefault;
            BorderWidthBackgroundButton = BorderWithDefault;
        }

        private void ChangeColor(string colorName)
        {
            Color color = (Color)_colorTypeConv.ConvertFromInvariantString(colorName);
            switch (_state)
            {
                case ButtonSelected.None:
                    break;
                case ButtonSelected.UpperLine:
                    ColorUpperLine = color;
                    _displayColors.ColorUpperLineName = colorName;
                    BorderColorUpperLineButton = _borderColorDefault;
                    BorderWidthUpperLineButton = BorderWithDefault;
                    MessagingCenter.Send(this, MessengerKeys.Colours, _displayColors);
                    UpdateRgbColors();
                    break;
                case ButtonSelected.LowerLine:
                    ColorLowerLine = color;
                    _displayColors.ColorLowerLineName = colorName;
                    BorderColorLowerLineButton = _borderColorDefault;
                    BorderWidthLowerLineButton = BorderWithDefault;
                    MessagingCenter.Send(this, MessengerKeys.Colours, _displayColors);
                    UpdateRgbColors();
                    break;
                case ButtonSelected.Background:
                    ColorBackground = color;
                    _displayColors.ColorBackgroundName = colorName;
                    BorderColorBackgroundButton = _borderColorDefault;
                    BorderWidthBackgroundButton = BorderWithDefault;
                    MessagingCenter.Send(this, MessengerKeys.Colours, _displayColors);
                    UpdateRgbColors();
                    break;
            }
            _state = ButtonSelected.None;
        }

        private void HighlightUpperLineButton()
        {
            ClearButtons();
            _state = ButtonSelected.UpperLine;
            SelectedIndex = (int)_state;
            BorderWidthUpperLineButton = BorderWithHighlight;
            BorderColorUpperLineButton = _borderColorHighlight;
        }


    }
}

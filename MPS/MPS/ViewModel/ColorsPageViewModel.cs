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
        enum ButtonSelected { None = 0, UpperLine, LowerLine, Background }

        public enum PickerColor { UpperLine = 0, LowerLine, Background }
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
                OnPropertyChanged();
            }
        }


        public ICommand ColorsCommand { get; }
        public ICommand SetUpperLineColorCommand { get; }
        public ICommand SetLowerLineColorCommand { get; }
        public ICommand SetBackgroundColorCommand { get; }
        private DisplayColors _displayColors;

        public double RedValue
        {
            get
            {                
                switch ((PickerColor)SelectedIndex)
                {
                    case PickerColor.UpperLine:
                        return _displayColors.UpperLineRed;
                    case PickerColor.LowerLine:
                        return _displayColors.LowerLineRed;
                    case PickerColor.Background:
                        return _displayColors.BackgroundLineRed;
                    default:
                        return _displayColors.UpperLineRed;
                }
            }
            set
            {
                value = Math.Round(value);
                var colorValue = 0;
                switch ((PickerColor)SelectedIndex)
                {
                    case PickerColor.UpperLine:
                        colorValue = _displayColors.UpperLineRed;
                        _displayColors.UpperLineRed = (int)value;
                        break;
                    case PickerColor.LowerLine:
                        colorValue = _displayColors.LowerLineRed;
                        _displayColors.LowerLineRed = (int)value;
                        break;
                    case PickerColor.Background:
                        colorValue = _displayColors.BackgroundLineRed;
                        _displayColors.BackgroundLineRed = (int)value;
                        break;                
                }

                if ((int)value != colorValue)
                {
                    MessagingCenter.Send(this, MessengerKeys.ColoursRgb, _displayColors);
                }
                OnPropertyChanged();
            }
        }

        public double GreenValue
        {           
            get
            {
                switch ((PickerColor)SelectedIndex)
                {
                    case PickerColor.UpperLine:
                        return _displayColors.UpperLineGreen;                  
                    case PickerColor.LowerLine:
                        return _displayColors.LowerLineGreen;
                    case PickerColor.Background:
                        return _displayColors.BackgroundLineGreen;
                    default:
                        return _displayColors.UpperLineGreen;
                }
            }
            set
            {
                value = Math.Round(value);
                int colorValue = 0;
                switch ((PickerColor)SelectedIndex)
                {
                    case PickerColor.UpperLine:
                        colorValue = _displayColors.UpperLineGreen;
                        _displayColors.UpperLineGreen = (int)value;
                        break;
                    case PickerColor.LowerLine:
                        colorValue = _displayColors.LowerLineGreen;
                        _displayColors.LowerLineGreen = (int)value;
                        break;
                    case PickerColor.Background:
                        colorValue = _displayColors.BackgroundLineGreen;
                        _displayColors.BackgroundLineGreen = (int)value;
                        break;
                }
               
                if ((int)value != colorValue)
                {
                    MessagingCenter.Send(this, MessengerKeys.ColoursRgb, _displayColors);
                }
                else
                {
                    OnPropertyChanged();
                }
            }
        }

        public double BlueValue
        {           
            get
            {
                switch ((PickerColor)SelectedIndex)
                {
                    case PickerColor.UpperLine:
                        return _displayColors.UpperLineBlue;
                    case PickerColor.LowerLine:
                        return _displayColors.LowerLineBlue;
                    case PickerColor.Background:
                        return _displayColors.BackgroundLineBlue;
                    default:
                        return _displayColors.UpperLineBlue;
                }
            }
            set
            {
                value = Math.Round(value);
                int colorValue = 0;               
                switch ((PickerColor)SelectedIndex)
                {
                    case PickerColor.UpperLine:
                        colorValue = _displayColors.UpperLineBlue;
                        _displayColors.UpperLineBlue = (int)value;
                        break;
                    case PickerColor.LowerLine:
                        colorValue = _displayColors.LowerLineBlue;
                        _displayColors.LowerLineBlue = (int)value;
                        break;
                    case PickerColor.Background:
                        colorValue = _displayColors.BackgroundLineBlue;
                        _displayColors.BackgroundLineBlue = (int)value;
                        break;
                }

                if ((int)value != colorValue)
                {
                    MessagingCenter.Send(this, MessengerKeys.ColoursRgb, _displayColors);
                }
                else
                {
                    OnPropertyChanged();
                }
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
            BorderWidthBackgroundButton = BorderWithHighlight;
            BorderColorBackgroundButton = _borderColorHighlight;
        }

        private void HighlightLowerLineButton()
        {
            ClearButtons();
            _state = ButtonSelected.LowerLine;
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
                    _displayColors.StringColorUpperLine = colorName;
                    BorderColorUpperLineButton = _borderColorDefault;
                    BorderWidthUpperLineButton = BorderWithDefault;
                    MessagingCenter.Send(this, MessengerKeys.Colours, _displayColors);
                    break;
                case ButtonSelected.LowerLine:
                    ColorLowerLine = color;
                    _displayColors.StringColorLowerLine = colorName;
                    BorderColorLowerLineButton = _borderColorDefault;
                    BorderWidthLowerLineButton = BorderWithDefault;
                    MessagingCenter.Send(this, MessengerKeys.Colours, _displayColors);
                    break;
                case ButtonSelected.Background:
                    ColorBackground = color;
                    _displayColors.StringColorBackground = colorName;
                    BorderColorBackgroundButton = _borderColorDefault;
                    BorderWidthBackgroundButton = BorderWithDefault;
                    MessagingCenter.Send(this, MessengerKeys.Colours, _displayColors);
                    break;
            }
            _state = ButtonSelected.None;
            //MessagingCenter.Send(this, MessengerKeys.Colours, DisplayColors);
        }

        private void HighlightUpperLineButton()
        {
            ClearButtons();            
            _state = ButtonSelected.UpperLine;
            BorderWidthUpperLineButton = BorderWithHighlight;
            BorderColorUpperLineButton = _borderColorHighlight;
        }


    }
}

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
        private Color BORDER_COLOR_DEFAULT = Color.Black;
        private Color BORDER_COLOR_HIGHLIGHT = Color.Gray;
        private const int BORDER_WITH_DEFAULT = 1;
        private const int BORDER_WITH_HIGHLIGHT = 3;

        private ColorTypeConverter _colorTypeConv = new ColorTypeConverter();
        private Color _borderColorLowerLine;
        private int _borderWidthLowerLineButton;
        private Color _borderColorUpperLineButton;
        private int _borderWidthUpperLineButton;
        enum ButtonSelected { None = 0, UpperLine, LowerLine, Background };
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


        public ICommand ColorsCommand { get; private set; }
        public ICommand SetUpperLineColorCommand { get; private set; }
        public ICommand SetLowerLineColorCommand { get; private set; }
        public ICommand SetBackgroundColorCommand { get; private set; }
        private DisplayColors DisplayColors { get; set; }

        public double RedValue
        {
            get
            {
                switch (SelectedIndex)
                {
                    case 0:
                        return DisplayColors.UpperLineRed;
                    case 1:
                        return DisplayColors.LowerLineRed;
                    case 2:
                        return DisplayColors.BackgroundLineRed;
                    default:
                        return DisplayColors.UpperLineRed;
                }
            }
            set
            {
                value = Math.Round(value);
                int colorValue = 0;
                switch (SelectedIndex)
                {
                    case 0:
                        colorValue = DisplayColors.UpperLineRed;
                        DisplayColors.UpperLineRed = (int)value;
                        break;
                    case 1:
                        colorValue = DisplayColors.LowerLineRed;
                        DisplayColors.LowerLineRed = (int)value;
                        break;
                    case 2:
                        colorValue = DisplayColors.BackgroundLineRed;
                        DisplayColors.BackgroundLineRed = (int)value;
                        break;
                }

                if ((int)value != colorValue)
                {
                    MessagingCenter.Send(this, MessengerKeys.ColoursRgb, DisplayColors);
                }
                OnPropertyChanged();
            }
        }

        public double GreenValue
        {
            get => DisplayColors.UpperLineGreen;
            set
            {
                value = Math.Round(value);
                int colorValue = DisplayColors.UpperLineGreen;
                DisplayColors.UpperLineGreen = (int)value;
                if ((int)value != colorValue)
                {
                    MessagingCenter.Send(this, MessengerKeys.ColoursRgb, DisplayColors);
                }
                else
                {
                    OnPropertyChanged();
                }
            }
        }

        public double BlueValue
        {
            get => DisplayColors.UpperLineBlue;
            set
            {
                value = Math.Round(value);
                int colorValue = DisplayColors.UpperLineBlue;
                DisplayColors.UpperLineBlue = (int)value;
                if ((int)value != colorValue)
                {
                    MessagingCenter.Send(this, MessengerKeys.ColoursRgb, DisplayColors);
                }
                else
                {
                    OnPropertyChanged();
                }
            }
        }

        public Color ColorUpperLine
        {
            get => DisplayColors.ColorUpperLine;
            set
            {
                DisplayColors.ColorUpperLine = value;
                OnPropertyChanged();
            }
        }

        public Color ColorLowerLine
        {
            get => DisplayColors.ColorLowerLine;
            set
            {
                DisplayColors.ColorLowerLine = value;
                OnPropertyChanged();
            }
        }

        public Color ColorBackground
        {
            get => DisplayColors.ColorBackground;
            set
            {
                DisplayColors.ColorBackground = value;
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
            DisplayColors = new DisplayColors();
            SetUpperLineColorCommand = new Command(HighlightUpperLineButton);
            SetLowerLineColorCommand = new Command(HighlightLowerLineButton);
            SetBackgroundColorCommand = new Command(HighlightBackgroundButton);
        }

        private void HighlightBackgroundButton()
        {
            ClearButtons();
            //HighlightButton(ButtonSelected.BACKGROUND, BorderColorBackgroundButton, BorderWidthBackgroundButton);
            _state = ButtonSelected.Background;
            BorderWidthBackgroundButton = BORDER_WITH_HIGHLIGHT;
            BorderColorBackgroundButton = BORDER_COLOR_HIGHLIGHT;
        }

        private void HighlightLowerLineButton()
        {
            ClearButtons();
            //HighlightButton(ButtonSelected.LOWER_LINE, BorderColorLowerLineButton, BorderWidthLowerLineButton);
            _state = ButtonSelected.LowerLine;
            BorderWidthLowerLineButton = BORDER_WITH_HIGHLIGHT;
            BorderColorLowerLineButton = BORDER_COLOR_HIGHLIGHT;

        }

        private void ClearButtons()
        {
            BorderColorUpperLineButton = BORDER_COLOR_DEFAULT;
            BorderColorLowerLineButton = BORDER_COLOR_DEFAULT;
            BorderColorBackgroundButton = BORDER_COLOR_DEFAULT;
            BorderWidthUpperLineButton = BORDER_WITH_DEFAULT;
            BorderWidthLowerLineButton = BORDER_WITH_DEFAULT;
            BorderWidthBackgroundButton = BORDER_WITH_DEFAULT;
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
                    DisplayColors.StringColorUpperLine = colorName;
                    BorderColorUpperLineButton = BORDER_COLOR_DEFAULT;
                    BorderWidthUpperLineButton = BORDER_WITH_DEFAULT;
                    break;
                case ButtonSelected.LowerLine:
                    ColorLowerLine = color;
                    DisplayColors.StringColorLowerLine = colorName;
                    BorderColorLowerLineButton = BORDER_COLOR_DEFAULT;
                    BorderWidthLowerLineButton = BORDER_WITH_DEFAULT;
                    break;
                case ButtonSelected.Background:
                    ColorBackground = color;
                    DisplayColors.StringColorBackground = colorName;
                    BorderColorBackgroundButton = BORDER_COLOR_DEFAULT;
                    BorderWidthBackgroundButton = BORDER_WITH_DEFAULT;
                    break;
            }
            _state = ButtonSelected.None;
            MessagingCenter.Send(this, MessengerKeys.Colours, DisplayColors);
        }

        private void HighlightUpperLineButton()
        {
            ClearButtons();
            //HighlightButton(ButtonSelected.UPPER_LINE, BorderColorUpperLineButton, BorderWidthUpperLineButton);
            _state = ButtonSelected.UpperLine;
            BorderWidthUpperLineButton = BORDER_WITH_HIGHLIGHT;
            BorderColorUpperLineButton = BORDER_COLOR_HIGHLIGHT;
        }

        /*  private void HighlightButton(ButtonSelected buttonSelected, Color BorderColorProperty, int BorderWithProperty)
          {
              state = buttonSelected;
              BorderWithProperty = BORDER_WITH_HIGHLIGHT;
              BorderColorProperty = BORDER_COLOR_HIGHLIGHT;
          }     */


    }
}

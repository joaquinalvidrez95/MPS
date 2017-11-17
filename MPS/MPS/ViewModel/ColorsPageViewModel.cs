using MPS.Model;
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

        private ColorTypeConverter colorTypeConv = new ColorTypeConverter();
        private Color borderColorLowerLine;
        private int borderWidthLowerLineButton;
        private Color borderColorUpperLineButton;
        private int borderWidthUpperLineButton;
        enum ButtonSelected { NONE = 0, UPPER_LINE, LOWER_LINE, BACKGROUND };
        ButtonSelected state;
        private Color borderColorBackgroundButton;
        private int borderWidthBackgroundButton;

        public Color BorderColorUpperLineButton
        {
            get
            {
                return borderColorUpperLineButton;
            }
            set
            {
                borderColorUpperLineButton = value;
                OnPropertyChanged();
            }
        }
        public int BorderWidthUpperLineButton
        {
            get
            {
                return borderWidthUpperLineButton;
            }
            set
            {
                borderWidthUpperLineButton = value;
                OnPropertyChanged();
            }
        }


        public ICommand ColorsCommand { get; private set; }
        public ICommand SetUpperLineColorCommand { get; private set; }
        public ICommand SetLowerLineColorCommand { get; private set; }
        public ICommand SetBackgroundColorCommand { get; private set; }
        private DisplayColors DisplayColors { get; set; }

        public Color ColorUpperLine
        {
            get
            {
                return DisplayColors.ColorUpperLine;
            }
            set
            {
                DisplayColors.ColorUpperLine = value;
                OnPropertyChanged();
            }
        }

        public Color ColorLowerLine
        {
            get
            {
                return DisplayColors.ColorLowerLine;
            }
            set
            {
                DisplayColors.ColorLowerLine = value;
                OnPropertyChanged();
            }
        }

        public Color ColorBackground
        {
            get
            {
                return DisplayColors.ColorBackground;
            }
            set
            {
                DisplayColors.ColorBackground = value;
                OnPropertyChanged();
            }
        }

        public Color BorderColorLowerLineButton
        {
            get
            {
                return borderColorLowerLine;
            }
            set
            {
                borderColorLowerLine = value;
                OnPropertyChanged();
            }
        }

        public int BorderWidthLowerLineButton
        {
            get
            {
                return borderWidthLowerLineButton;
            }
            set
            {
                borderWidthLowerLineButton = value;
                OnPropertyChanged();
            }
        }

        public Color BorderColorBackgroundButton
        {
            get
            {
                return borderColorBackgroundButton;
            }
            set
            {
                borderColorBackgroundButton = value;
                OnPropertyChanged();
            }
        }
        public int BorderWidthBackgroundButton
        {
            get
            {
                return borderWidthBackgroundButton;
            }
            set
            {
                borderWidthBackgroundButton = value;
                OnPropertyChanged();
            }
        }

        public ColorsPageViewModel()
        {
            ColorsCommand = new Command<string>(changeColor);
            DisplayColors = new DisplayColors();
            SetUpperLineColorCommand = new Command(highlightUpperLineButton);
            SetLowerLineColorCommand = new Command(highlightLowerLineButton);
            SetBackgroundColorCommand = new Command(highlightBackgroundButton);
        }

        private void highlightBackgroundButton()
        {
            clearButtons();
            highlightButton(ButtonSelected.BACKGROUND, BorderColorBackgroundButton, BorderWidthBackgroundButton);
            BorderWidthBackgroundButton = BORDER_WITH_HIGHLIGHT;
            BorderColorBackgroundButton = BORDER_COLOR_HIGHLIGHT;
        }

        private void highlightLowerLineButton()
        {
            clearButtons();
            highlightButton(ButtonSelected.LOWER_LINE, BorderColorLowerLineButton, BorderWidthLowerLineButton);
            BorderWidthLowerLineButton = BORDER_WITH_HIGHLIGHT;
            BorderColorLowerLineButton = BORDER_COLOR_HIGHLIGHT;
            
        }

        private void clearButtons()
        {            
            BorderColorUpperLineButton = BORDER_COLOR_DEFAULT;
            BorderColorLowerLineButton = BORDER_COLOR_DEFAULT;
            BorderColorBackgroundButton = BORDER_COLOR_DEFAULT;
            BorderWidthUpperLineButton = BORDER_WITH_DEFAULT;
            BorderWidthLowerLineButton = BORDER_WITH_DEFAULT;
            BorderWidthBackgroundButton = BORDER_WITH_DEFAULT;           
        }

        private void changeColor(string colorName)
        {
            Color color = (Color)colorTypeConv.ConvertFromInvariantString(colorName);
            switch (state)
            {
                case ButtonSelected.NONE:
                    break;
                case ButtonSelected.UPPER_LINE:
                    ColorUpperLine = color;
                    BorderColorUpperLineButton = BORDER_COLOR_DEFAULT;
                    BorderWidthUpperLineButton = BORDER_WITH_DEFAULT;
                    break;
                case ButtonSelected.LOWER_LINE:
                    ColorLowerLine = color;
                    BorderColorLowerLineButton = BORDER_COLOR_DEFAULT;
                    BorderWidthLowerLineButton = BORDER_WITH_DEFAULT;
                    break;
                case ButtonSelected.BACKGROUND:
                    ColorBackground = color;
                    BorderColorBackgroundButton = BORDER_COLOR_DEFAULT;
                    BorderWidthBackgroundButton = BORDER_WITH_DEFAULT;
                    break;
            }
            state = ButtonSelected.NONE;
        }

        private void highlightUpperLineButton()
        {
            clearButtons();
            highlightButton(ButtonSelected.UPPER_LINE, BorderColorUpperLineButton, BorderWidthUpperLineButton);
            BorderWidthUpperLineButton = BORDER_WITH_HIGHLIGHT;
            BorderColorUpperLineButton = BORDER_COLOR_HIGHLIGHT;
        }

        private void highlightButton(ButtonSelected buttonSelected, Color BorderColorProperty, int BorderWithProperty)
        {
            state = buttonSelected;
            BorderWithProperty = BORDER_WITH_HIGHLIGHT;
            BorderColorProperty = BORDER_COLOR_HIGHLIGHT;
        }

        private void clearButton(string colorName, Color BackgroundColorButton, Color BorderColorButton, int BorderWidthButton)
        {
            BackgroundColorButton = (Color)colorTypeConv.ConvertFromInvariantString(colorName);
            BorderColorButton = Color.Black;
            BorderWidthButton = 1;
            state = ButtonSelected.NONE;
        }


    }
}

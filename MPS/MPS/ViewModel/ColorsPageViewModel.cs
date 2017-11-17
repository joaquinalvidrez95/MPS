using MPS.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MPS.ViewModel
{
    public class ColorsPageViewModel
    {
        private Boolean isButtonPushed = false;
        ColorTypeConverter colorTypeConv = new ColorTypeConverter();
        public ICommand ColorsCommand { get; private set; }
        public ICommand SetUpperLineColorCommand { get; private set; }
        public DisplayColors DisplayColors { get; set; }
        public ColorsPageViewModel()
        {
            ColorsCommand = new Command<string>(changeColor);
            DisplayColors = new DisplayColors();
        }

        private void changeColor(string color)
        {
            //DisplayColors.ColorNameUpperLine = "Red";
            //DisplayColors.ColorLowerLine = Color.Black;
            isButtonPushed = true;
            DisplayColors.ColorUpperLine =  (Color)colorTypeConv.ConvertFromInvariantString(color);          


        }
    }
}

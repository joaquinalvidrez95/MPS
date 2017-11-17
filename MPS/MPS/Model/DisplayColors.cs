using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MPS.Model
{
    public class DisplayColors 
    {
        Color colorUpperLine;
        Color colorLowerLine;
        Color colorBackground;       

        public Color ColorLowerLine
        {
            get
            {
                return colorLowerLine;
            }
            set
            {
                colorLowerLine = value;
                //PropertyChanged(this, new PropertyChangedEventArgs("ColorLowerLine"));
            }
        }

        public Color ColorUpperLine
        {
            get
            {
                return colorUpperLine;
            }
            set
            {
                colorUpperLine = value;
                //PropertyChanged(this, new PropertyChangedEventArgs("ColorUpperLine"));
            }
        }

        public Color ColorBackground
        {
            get
            {
                return colorBackground;
            }
            set
            {
                colorBackground = value;
              //  PropertyChanged(this, new PropertyChangedEventArgs("ColorBackground"));
            }
        }
 

        //public event PropertyChangedEventHandler PropertyChanged;

    }
}

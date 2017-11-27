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
        public Color ColorLowerLine { get; set; }

        public Color ColorUpperLine { get; set; }

        public Color ColorBackground { get; set; }

        public string GetColorCode()
        {
            return ConvertColorToString(StringColorUpperLine)
                   + ConvertColorToString(StringColorLowerLine)
                   + ConvertColorToString(StringColorBackground);
        }

        public string ConvertColorToString(string colorName)
        {
            switch (colorName)
            {
                case "Red":
                    return "500";
                case "Orange":
                    return "510";
                case "Yellow":
                    return "420";
                case "LawnGreen":
                    return "350";
                case "SpringGreen":
                    return "051";
                case "Cyan":
                    return "053";
                case "DodgerBlue":
                    return "035";
                case "Blue":
                    return "005";
                case "BlueViolet":
                    return "205";
                case "Magenta":
                    return "505";
                case "Black":
                    return "000";
                case "White":
                    return "555";
                default:
                    return "000";
            }
        }

        public string StringColorBackground { get; set; }
        public string StringColorLowerLine { get; set; }
        public string StringColorUpperLine { get; set; }
        public int UpperLineRed { get; set; }
        public int UpperLineGreen { get; set; }
        public int UpperLineBlue { get; set; }
        public int LowerLineRed { get; set; }
        public int LowerLineGreen { get; set; }
        public int LowerLineBlue { get; set; }
        public int BackgroundLineRed { get; set; }
        public int BackgroundLineGreen { get; set; }
        public int BackgroundLineBlue { get; set; }

        public string ColorCodeRgb => UpperLineRed
                                      + UpperLineGreen.ToString()
                                      + UpperLineBlue
                                      + LowerLineRed
                                      + LowerLineGreen
                                      + LowerLineBlue
                                      + BackgroundLineRed
                                      + BackgroundLineGreen
                                      + BackgroundLineBlue;
    }
}

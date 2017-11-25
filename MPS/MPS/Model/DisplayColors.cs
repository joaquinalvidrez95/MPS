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
            string code = "";

            switch (colorName)
            {
                case "Red":
                    code = "500";
                    break;
                case "Orange":
                    code = "510";
                    break;
                case "Yellow":
                    code = "420";
                    break;
                case "LawnGreen":
                    code = "350";
                    break;
                case "SpringGreen":
                    code = "051";
                    break;
                case "Cyan":
                    code = "053";
                    break;
                case "DodgerBlue":
                    code = "035";
                    break;
                case "Blue":
                    code = "005";
                    break;
                case "BlueViolet":
                    code = "205";
                    break;
                case "Magenta":
                    code = "505";
                    break;
                case "Black":
                    code = "000";
                    break;
                case "White":
                    code = "555";
                    break;
            }


            return code;
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
